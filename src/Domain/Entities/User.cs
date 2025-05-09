using Domain.Enums;

namespace Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public required string LastName { get; set; }
        public required string FirstName { get; set; }
        public required string Email { get; set; }
        public string? PasswordHash { get; set; }
        public UserRoles Role { get; set; }
        public int? LogoAttachmentId { get; set; }
    }
}
