namespace PinjamKelas.Api.Dtos
{
    public class CreateUserDto
    {
        public required string Username { get; set; }
        public required string Role { get; set; }
    }
}