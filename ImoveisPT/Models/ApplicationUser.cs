using Microsoft.AspNetCore.Identity;

namespace ImoveisPT.Models
{
    public class ApplicationUser : IdentityUser
    {        
        public virtual ICollection<Advertisement> Advertisements { get; set; } //Indica relação 1:M
        public virtual ICollection<Favorite> Favorites { get; set; }
        
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public ApplicationUser()
        {
        }

        public ApplicationUser(string userName, string email, string firstName, string lastName)
        {
            UserName = userName;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
