    namespace PathTalesFront.Model
{
    public class RegisterRequest
    {
        public string Name { get; set; }          
        public string Email { get; set; }         
        public string Password { get; set; }       

        public string Token { get; set; } = null;  
        public string Role { get; set; } = "member";
        public bool IsBlocked { get; set; } = false;
        public string BlockReason { get; set; } = null;
    }
}
