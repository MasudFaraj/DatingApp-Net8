using System.ComponentModel.DataAnnotations.Schema;

namespace API.EntitiesModels
{
    [Table("Photos")]   // ef create this 'Photos' table
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public bool IsMean { get; set; }
        public string PublicId { get; set; } = string.Empty;
        public AppUser? AppUser { get; set; }
        public int AppUserId { get; set; }
    }
}
