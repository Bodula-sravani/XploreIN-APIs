using System.ComponentModel.DataAnnotations;

namespace XploreIN.Models
{
    public class RuralDestination
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Activites { get; set; }
        public string? Specialities { get; set; }
        public string? Accomidation { get; set; }

    }
}
