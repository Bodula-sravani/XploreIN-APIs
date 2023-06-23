using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XploreIN.Models
{
    public class DestinationMedia
    {
        [Key]

        public int Id { get; set; }

        [ForeignKey("RuralDestination")]
        public int Destination_id { get; set; }
        public RuralDestination RuralDestination { get; set; }

        [Required]
        public string Media_URL { get; set; }

        public DateTime Created_At { get; set; }    


    }
}
