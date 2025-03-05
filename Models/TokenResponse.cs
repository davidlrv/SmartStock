namespace SmartStock.Models
{
    public class TokenResponse
    {
        public string Token { get; set; }
    }
    public class UserData
    {
        public Data Data { get; set; }
        public bool Failed { get; set; }
        public string Message { get; set; }
        public bool Succeeded { get; set; }
    }

    public class Data
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public bool Verified { get; set; }
        public string JwToken { get; set; }
        public DateTime IssuedOn { get; set; }
        public DateTime ExpiresOn { get; set; }
    }
}
