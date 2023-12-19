using Microsoft.AspNetCore.Identity;

namespace ImoveisPT.Models
{
    public class ApplicationUser : IdentityUser
    {        
        public virtual ICollection<Advertisement> Advertisements { get; set; } //Indica relação 1:M
        public virtual ICollection<Favorite> Favorites { get; set; }

        public ApplicationUser()
        {
        }

        public ApplicationUser(string userName, string email)
        {
            UserName = userName;
            Email = email;
        }
    }
}
