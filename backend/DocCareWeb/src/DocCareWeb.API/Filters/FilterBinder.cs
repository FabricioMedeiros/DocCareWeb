using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DocCareWeb.API.Filters
{
    public class FilterBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var filters = new Dictionary<string, string>();

            foreach (var key in bindingContext.HttpContext.Request.Query.Keys)
            {

                if (key.Equals("pageNumber", StringComparison.OrdinalIgnoreCase) ||
                    key.Equals("pageSize", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                var value = bindingContext.HttpContext.Request.Query[key].ToString();

                if (!string.IsNullOrWhiteSpace(value))
                {
                    filters[key] = value;
                }
            }

            bindingContext.Result = ModelBindingResult.Success(filters);
            return Task.CompletedTask;
        }
    }
}
