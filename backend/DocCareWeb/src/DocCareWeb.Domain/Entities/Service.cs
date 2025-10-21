namespace DocCareWeb.Domain.Entities
{
    public class Service : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public decimal BasePrice { get; set; }
        public ICollection<HealthPlanItem> HealthPlanItems { get; set; } = new List<HealthPlanItem>();
    }
}
