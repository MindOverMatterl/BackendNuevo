namespace Domain.Dtos
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = null!;
        public int UserTypeId { get; set; }
        public string UserType { get; set; } = null!;
    }
}