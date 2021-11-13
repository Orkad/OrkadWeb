namespace OrkadWeb.Logic.Users.Commands
{
    public class LoginResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }
    }
}
