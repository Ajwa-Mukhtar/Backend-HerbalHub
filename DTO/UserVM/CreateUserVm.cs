namespace HerbalHub.DTO.UserVM
{
    public class CreateUserVm
    {
 

        public string UserEmail { get; set; } = "";
        public string UserName { get; set; } = "";
        public string Password { get; set; } = "";
        public string UserType { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = ""; 
        public string Gender { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public DateTime? DateOfBirth { get; set; }

        public string Address { get; set; } = "";
    }

}
