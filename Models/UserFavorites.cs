using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XploreIN.Models
{
    public class UserFavorites
    {

        [Key]

        public int Id { get; set; }

        [Required]
        public string PlaceName { get; set; }

        [Required]
        public string PhotoUrl { get; set; }

        [ForeignKey("User")]

        public string UserId { get; set; }

        public IdentityUser User { get; set; }


    }
}
