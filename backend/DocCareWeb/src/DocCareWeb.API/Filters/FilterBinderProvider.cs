using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DocCareWeb.API.Filters
{
    public class FilterBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType == typeof(Dictionary<string, string>))
            {
                return new FilterBinder();
            }

            return null;
        }
    }

}
