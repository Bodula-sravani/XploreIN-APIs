using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XploreIN.Models
{
    public class UserPost
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Destination { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime Created_date { get; set; }

        [Required]

        public int Rating { get; set; }

        [ForeignKey("User")]

        public string UserId { get; set; }

        public IdentityUser User { get; set; }

    }
}
