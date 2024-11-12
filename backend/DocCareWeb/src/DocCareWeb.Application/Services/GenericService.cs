using AutoMapper;
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
                    // Verifica se a propriedade existe na entidade
                    var property = typeof(TEntity).GetProperty(filter.Key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                    if (property == null) continue;
                 
                    var comparison = CreateNestedComparisonExpression(parameter, property.Name, filter.Value); // Usar property.Name
                    combinedExpression = Expression.AndAlso(combinedExpression, comparison);
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
            Expression comparison = propertyType switch
            {
                Type _ when typeof(string).IsAssignableFrom(propertyType) =>
                    Expression.Call(left, "Contains", null, Expression.Constant(filterValue, typeof(string))),

                Type _ when typeof(bool).IsAssignableFrom(propertyType) =>
                    Expression.Equal(left, Expression.Constant(bool.Parse(filterValue))),

                Type _ when typeof(DateTime).IsAssignableFrom(propertyType) =>
                    Expression.Equal(left, Expression.Constant(DateTime.Parse(filterValue))),

                Type _ when propertyType.IsPrimitive || propertyType.IsValueType =>
                    Expression.Equal(left, Expression.Constant(Convert.ChangeType(filterValue, propertyType))),

                _ when propertyType.IsEnum || Nullable.GetUnderlyingType(propertyType)?.IsEnum == true =>
                    Expression.Equal(left, Expression.Constant(ParseEnum(propertyType, filterValue))),

                _ when propertyType == typeof(Guid) || Nullable.GetUnderlyingType(propertyType) == typeof(Guid) =>
                    Expression.Equal(left, Expression.Constant(Guid.Parse(filterValue))),

                _ => throw new NotSupportedException($"Unsupported property type: {propertyType}")
            };

            return comparison;
        }

        private static Expression CreateNestedComparisonExpression(ParameterExpression parameter, string propertyPath, string filterValue)
        {
            // Divida o caminho da propriedade (ex: "doctor[id]") em partes
            var properties = propertyPath.Split(new[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);

            Expression expression = parameter;

            // Navega pelas propriedades
            foreach (var prop in properties)
            {
                expression = Expression.PropertyOrField(expression, prop);
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
