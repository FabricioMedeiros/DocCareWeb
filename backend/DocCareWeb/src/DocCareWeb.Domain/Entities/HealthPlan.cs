namespace DocCareWeb.Domain.Entities
{
    public class HealthPlan : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public ICollection<HealthPlanItem> Items { get; set; } = new List<HealthPlanItem>();
    }
}
