using System.ComponentModel.DataAnnotations;

namespace ImoveisPT.Models
{
    public class Favorite
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; } = null!;

        [Required]
        public int AdvertisementId { get; set; }

        public virtual Advertisement Advertisement { get; set; } = null!;


        public Favorite()
        {

        }

        public Favorite(int advertisementId)
        {
            AdvertisementId = advertisementId;
        }
    }
}
