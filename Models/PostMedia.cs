using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace XploreIN.Models
{
    public class PostMedia
    {
        [Key]

        public int Id { get; set; }

        [ForeignKey("UserPost")]
        public int PostId { get; set; }
        public UserPost UserPost { get; set; }

        [Required]
        public string Media_URL { get; set; }

        public DateTime Created_At { get; set; }
    }
}
