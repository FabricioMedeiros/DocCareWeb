using DocCareWeb.Application.Notifications;

namespace DocCareWeb.Application.Services
{
    public abstract class BaseService
    {
        private readonly INotificator _notificator;

        #region Notificator
        protected BaseService(INotificator notificator)
        {
            _notificator = notificator;
        }

        protected void Notify(string message)
        {
            _notificator.AddNotification(new Notification(message));
        }

        protected bool IsValidOperation()
        {
            return !_notificator.HasNotification();
        }

        #endregion

        #region Helpers
        protected List<int> GetDuplicatedIds<T>(IEnumerable<T> items, Func<T, int> selector)
        {
            return items
                .GroupBy(selector)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();
        }

        protected async Task<List<int>> GetInvalidIdsAsync(IEnumerable<int> ids, Func<IEnumerable<int>, Task<HashSet<int>>> getExistingIdsFunc)
        {
            var existingIds = await getExistingIdsFunc(ids);
            return ids.Where(id => !existingIds.Contains(id)).ToList();
        }

        protected async Task<bool> ValidateIdsAsync<T>(
            IEnumerable<T> items,
            Func<T, int> selector,
            Func<IEnumerable<int>, Task<HashSet<int>>> getExistingIdsFunc,
            string label = "Id")
        {
            var duplicated = GetDuplicatedIds(items, selector);
            if (duplicated.Any())
            {
                Notify($"Os seguintes {label}s estão duplicados: {string.Join(", ", duplicated)}");
                return false;
            }

            var ids = items.Select(selector).Distinct().ToList();
            var invalid = await GetInvalidIdsAsync(ids, getExistingIdsFunc);
            if (invalid.Any())
            {
                Notify($"Os seguintes {label}s não existem: {string.Join(", ", invalid)}");
                return false;
            }

            return true;
        }

        #endregion
    }
}
