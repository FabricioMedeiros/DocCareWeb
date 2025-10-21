namespace DocCareWeb.Domain.Entities
{
    public class HealthPlanItem : BaseEntity
    {
        public int HealthPlanId { get; set; }
        public HealthPlan HealthPlan { get; set; } = null!;
        public int ServiceId { get; set; }
        public Service Service { get; set; } = null!;
        public decimal Price { get; set; }
    }
}
