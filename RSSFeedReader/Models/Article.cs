using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RSSFeedReader.Models
{
    public class Article
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } 
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public string? Link { get; set; }
        [DisplayName("Publishing Date")]
        public DateTime PublishDate { get; set; }
        [ForeignKey("Feed")]
        public int FeedId { get; set; }
        public Feed? Feed { get; set; }
    }
}
