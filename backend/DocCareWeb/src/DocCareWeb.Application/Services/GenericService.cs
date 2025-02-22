﻿using AutoMapper;
using DocCareWeb.Application.Interfaces;
using DocCareWeb.Application.Notifications;
using DocCareWeb.Domain.Entities;
using DocCareWeb.Domain.Interfaces;
using FluentValidation;
using System.Linq.Expressions;
using System.Reflection;

namespace DocCareWeb.Application.Services
{
    public class GenericService<TEntity, TCreateDto, TUpdateDto, TListDto> : BaseService, IGenericService<TEntity, TCreateDto, TUpdateDto, TListDto>
        where TEntity : BaseEntity
        where TCreateDto : class
        where TUpdateDto : class
        where TListDto : class
    {
        protected readonly IGenericRepository<TEntity> _repository;
        private readonly IValidator<TCreateDto> _createValidator;
        private readonly IValidator<TUpdateDto> _updateValidator;
        protected readonly IMapper _mapper;

        public GenericService(
            IGenericRepository<TEntity> repository,
            IValidator<TCreateDto> createValidator,
            IValidator<TUpdateDto> updateValidator,
            IMapper mapper,
            INotificator notificator)
            : base(notificator)
        {
            _repository = repository;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _mapper = mapper;
        }

        public virtual async Task<PagedResult<TListDto>> GetAllAsync(Dictionary<string, string>? filters, int? pageNumber = null, int? pageSize = null)
        {
            var parameter = Expression.Parameter(typeof(TEntity), "entity");
            Expression combinedExpression = Expression.Constant(true);

            if (filters != null)
            {
                foreach (var filter in filters)
                {
                    try
                    {
                        var comparison = CreateNestedComparisonExpression(parameter, filter.Key, filter.Value);
                        combinedExpression = Expression.AndAlso(combinedExpression, comparison);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro ao criar expressão para filtro '{filter.Key}': {ex.Message}");
                        continue;
                    }
                }
            }

            var lambda = Expression.Lambda<Func<TEntity, bool>>(combinedExpression, parameter);

            var entities = await _repository.GetAllAsync(lambda);
            var entitiesQuery = entities.AsQueryable();

            int totalRecords = entitiesQuery.Count();

            if (pageNumber.HasValue && pageSize.HasValue)
            {
                entitiesQuery = entitiesQuery.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            var pagedResult = new PagedResult<TListDto>
            {
                Page = pageNumber ?? 1,
                PageSize = pageSize ?? totalRecords,
                TotalRecords = totalRecords,
                Items = _mapper.Map<IEnumerable<TListDto>>(entitiesQuery.ToList())
            };

            return pagedResult;
        }

        public virtual async Task<TListDto?> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return _mapper.Map<TListDto>(entity);
        }

        public virtual async Task<TEntity?> GetByIdAsync(int id, bool returnEntity)
        {
            return await _repository.GetByIdAsync(id);
        }

        public virtual async Task<TListDto?> AddAsync(TCreateDto createDto)
        {          
            var entity = _mapper.Map<TEntity>(createDto);
            var createdEntity = await _repository.AddAsync(entity);
            return _mapper.Map<TListDto>(createdEntity);
        }

        public virtual async Task<TListDto?> AddAsync(TEntity entity)
        {
            var createdEntity = await _repository.AddAsync(entity);
            return _mapper.Map<TListDto>(createdEntity);
        }

        public virtual async Task UpdateAsync(TUpdateDto updateDto)
        {     
            var entity = _mapper.Map<TEntity>(updateDto);
            await _repository.UpdateAsync(entity);
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            await _repository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        private static Expression CreateComparisonExpression(Expression left, Type propertyType, string filterValue)
        {
            Type underlyingType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

            var filterValues = filterValue.Split(',').Select(v => v.Trim()).ToList();

            if (filterValues.Count > 1)
            {
                var parsedValues = filterValues.Select(value => ConvertToType(underlyingType, value)).ToList();

                var listType = typeof(List<>).MakeGenericType(underlyingType);
                var typedList = Activator.CreateInstance(listType);

                var addMethod = listType.GetMethod("Add");
                foreach (var val in parsedValues)
                {
                    addMethod!.Invoke(typedList, new[] { val });
                }

                var valuesExpression = Expression.Constant(typedList);
                var containsMethod = listType.GetMethod("Contains");

                return Expression.Call(valuesExpression, containsMethod!, left);
            }

            object parsedValue = ConvertToType(underlyingType, filterValue);
            Expression right = Expression.Constant(parsedValue, underlyingType);

            if (underlyingType == typeof(string))
            {
                var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                return Expression.Call(left, containsMethod!, right);
            }

            return Expression.Equal(left, Expression.Convert(right, left.Type));
        }

        private static object ConvertToType(Type targetType, string value)
        {
            Type underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;
            return underlyingType switch
            {
                { } when underlyingType == typeof(bool) => bool.Parse(value),
                { } when underlyingType == typeof(DateTime) => DateTime.Parse(value),
                { } when underlyingType == typeof(Guid) => Guid.Parse(value),
                { } when underlyingType.IsEnum => Enum.Parse(underlyingType, value),
                _ => Convert.ChangeType(value, underlyingType!)
            };
        }

        private static Expression CreateNestedComparisonExpression(ParameterExpression parameter, string propertyPath, string filterValue)
        {
            var properties = propertyPath.Split('.', StringSplitOptions.RemoveEmptyEntries);

            Expression expression = parameter;

            foreach (var prop in properties)
            {
                var propertyInfo = expression.Type.GetProperty(prop, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (propertyInfo == null)
                {
                    throw new InvalidOperationException($"Property '{prop}' not found on type '{expression.Type.Name}'");
                }

                expression = Expression.Property(expression, propertyInfo);
            }

            var propertyType = ((MemberExpression)expression).Type;

            var comparison = CreateComparisonExpression(expression, propertyType, filterValue);

            return comparison;
        }       

        private static object ParseEnum(Type enumType, string value)
        {
            enumType = Nullable.GetUnderlyingType(enumType) ?? enumType;
            return Enum.Parse(enumType, value);
        }    
    }
}
