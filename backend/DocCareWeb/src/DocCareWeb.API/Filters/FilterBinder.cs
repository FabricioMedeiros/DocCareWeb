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
                var value = bindingContext.HttpContext.Request.Query[key].ToString();

                var normalizedKey = key.Replace("[", ".").Replace("]", "");

                if (!string.IsNullOrWhiteSpace(value))
                {
                    filters[normalizedKey] = value;
                }
            }

            bindingContext.Result = ModelBindingResult.Success(filters);
            return Task.CompletedTask;
        }
    }
}
