namespace TodoApi.DTO
{
    public class UserRequestDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class CreateUserResponseDTO
    {
        public string status { get; set; }
        public string message { get; set; }
    }

    public class LoginRequestDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class LoginResponseDTO
    {
        public string status { get; set; }
        public string message { get; set; }
        public string accessToken { get; set; }
    }

    public class GetUserResponseDTO
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string IsLock { get; set; }
        public string FailCount { get; set; }
    }

    public class UnlockUserRequestDTO
    {
        public string UserID { get; set; }
    }
}