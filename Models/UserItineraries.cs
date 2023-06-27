using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XploreIN.Models
{
    public class UserItineraries
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public DateTime Created_date { get; set; }

        [Required]
        public DateTime Start_date { get; set; }

        [Required]
        public DateTime End_date { get; set;}

        [ForeignKey("User")]
        public string UserId { get; set; }
        public IdentityUser User { get; set; }
    }
}
