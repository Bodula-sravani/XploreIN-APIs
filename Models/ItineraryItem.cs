using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XploreIN.Models
{
    public class ItineraryItem
    {

        [Key]
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? Date { get; set; }
        public string? Todos { get; set; }

        [ForeignKey("UserItineraries")]
        public int ItineraryId { get; set; }
        public UserItineraries UserItineraries { get; set;}
    }
}
