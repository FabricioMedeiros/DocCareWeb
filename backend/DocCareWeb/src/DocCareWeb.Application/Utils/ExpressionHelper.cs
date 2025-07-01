using System.Linq.Expressions;
using System.Reflection;

namespace DocCareWeb.Application.Utils
{
    public static class ExpressionHelper
    {
        public static object ConvertToType(Type targetType, string value)
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

        public static Expression CreateComparisonExpression(Expression left, Type propertyType, string filterValue)
        {
            var targetType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;
            var filterValues = filterValue.Split(',').Select(v => v.Trim()).ToList();

            if (filterValues.Count > 1)
            {
                var parsedValues = filterValues.Select(value => ConvertToType(targetType, value)).ToList();

                var listType = typeof(List<>).MakeGenericType(targetType);
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

            if (targetType == typeof(string))
            {
                var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                var right = Expression.Constant(filterValue, typeof(string));
                return Expression.Call(left, containsMethod!, right);
            }

            object parsedValue = ConvertToType(targetType, filterValue);
            Expression rightExpr = Expression.Constant(parsedValue, targetType);

            return Expression.Equal(left, Expression.Convert(rightExpr, left.Type));
        }

        public static Expression CreateNestedComparisonExpression(ParameterExpression parameter, string propertyPath, string filterValue)
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
            return CreateComparisonExpression(expression, propertyType, filterValue);
        }
    }
}
