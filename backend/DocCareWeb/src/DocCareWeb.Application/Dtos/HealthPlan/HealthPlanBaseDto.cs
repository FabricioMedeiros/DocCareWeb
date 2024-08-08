namespace DocCareWeb.Application.Dtos.HealthPlan
{
    public abstract class HealthPlanBaseDto
    {
        public required string Description { get; set; }
        public required decimal Cost { get; set; }
    }
}
