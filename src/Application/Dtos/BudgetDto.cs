namespace Application.Dtos
{
    public class BudgetDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public string? Description { get; set; }
        public int CreatorId { get; set; }
    }
}
