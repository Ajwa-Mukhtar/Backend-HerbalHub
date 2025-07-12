using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HerbalHub.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Column(TypeName = "NVARCHAR")]
        [MaxLength(450)]
        public string UserEmail { get; set; } = "";
        [Column(TypeName = "NVARCHAR")]
        [MaxLength(200)]
        public string UserName { get; set; } = "";
        [Column(TypeName = "NVARCHAR")]
        [MaxLength(200)]
        public string Password { get; set; } = "";
        [Column(TypeName = "NVARCHAR")]
        [MaxLength(100)]
        public string UserType { get; set; } = "";

        [Column(TypeName = "NVARCHAR")]
        [MaxLength(25)]
        public string FirstName { get; set; } = "";
        [Column(TypeName = "NVARCHAR")]
        [MaxLength(25)]
        public string LastName { get; set; } = "";
        [Column(TypeName = "NVARCHAR")]
        [MaxLength(100)]
        public string Gender { get; set; } = "";
        [Column(TypeName = "NVARCHAR")]
        [MaxLength(100)]
        public string PhoneNumber { get; set; } = "";
        [DataType(DataType.DateTime)]
        public DateTime DateOfBirth { get; set; }

        [Column(TypeName = "NVARCHAR")]
        [MaxLength(1000)]
        public string Address { get; set; } = "";

        [Column(TypeName = "NVARCHAR")]
        [MaxLength(300)]
        public string ProfileImageUrl { get; set; } = "";
        public bool IsEmailVerified { get; set; } = false;
        public bool IsBlocked { get; set; } = false;


    }
}
