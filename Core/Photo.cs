using System.ComponentModel.DataAnnotations.Schema;

namespace Core
{
    public class Photo:EntityBase
    {
        public string Url { get; set; }
        public bool IsMain { get; set; }
        public int PublicId { get; set; }
        [ForeignKey("User")]
        public string userId { get; set; }
        public AppUser User { get; set; }
    }
}