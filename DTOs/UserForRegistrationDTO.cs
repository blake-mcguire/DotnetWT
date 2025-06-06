namespace DotnetPI.DTOs
{
    public partial class UserForRegistrationDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
        public string FirstName { get; set; } 
        public string LastName { get; set; }  
        public string Gender { get; set; }



        public UserForRegistrationDTO()
        {
            if (Email == null)
            {
                Email = "";
            }
            if (Password == null)
            {
                Password = "";
            }
            if (PasswordConfirm == null)
            {
                PasswordConfirm = "";
            }
            FirstName ??= ""; 
            LastName ??= "";
            Gender ??= "";
        }
    }
}