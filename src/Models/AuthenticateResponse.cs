namespace ICICI.Models
{
    public class AuthenticateResponse
    {
        public int Id { get; set; }        
        public string Username { get; set; }
        public string Token { get; set; }


        public AuthenticateResponse(ApplicationUser user, string token)
        {
            Id = user.Id;
            Username = user.UserName;
            Token = token;
        }
    }
}
