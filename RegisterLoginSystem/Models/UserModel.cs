using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace RegisterLoginSystem.Models
{
    public class User
    {
        [Key, Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string LastName { get; set; }
        [Required]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}")]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$")]

        public string Password { get; set; }

        public bool ShowSettings { get; set; }

        [NotMapped]
        [Required]
        [System.ComponentModel.DataAnnotations.Compare("Password")]
        public string ConfirmPassword { get; set; }
        public string FullName()
        {
            return FirstName + " " + LastName;
        }

        //public List<UserSetting> UserSettings { get; set; }
    }
}