namespace DotnetAPI.Models
{
    public partial class UserJobInfoDTO
    {
        
        public string JobTitle { get; set; } 
        public string Department { get; set; } 



        public UserJobInfoDTO()
        {
            
           JobTitle ??= ""; 
           Department ??= "";
        }
    }
}