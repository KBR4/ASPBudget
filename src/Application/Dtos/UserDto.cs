namespace Application.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public required string LastName { get; set; }
        public required string FirstName { get; set; }
        public required string Email { get; set; }
        public string? Role { get; set; }
        public int? LogoAttachmentId { get; set; }
        public string? LogoAttachmentUrl { get; set; }
    }
}
