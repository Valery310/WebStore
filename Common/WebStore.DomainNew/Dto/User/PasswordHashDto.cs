namespace WebStore.Domain.Dto.User
{
    public class PasswordHashDto
    {
        public Entities.User User { get; set; }
        public string Hash { get; set; }
    }
}
