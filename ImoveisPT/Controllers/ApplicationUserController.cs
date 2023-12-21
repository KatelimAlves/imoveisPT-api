using ImoveisPT.Data;
using ImoveisPT.Models;
using ImoveisPT.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ImoveisPT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUsersController : ControllerBase
    {
        private readonly ImoveisPTContext _context;

        public ApplicationUsersController(ImoveisPTContext context)
        {
            _context = context;
        }

        // GET: api/ApplicationUsers
        //[Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetApplicationUserViewModel>>> GetApplicationUsers()
        {
            var applicationUsers = await _context.ApplicationUsers
                  .Include(user => user.Favorites)
                  .Include(user => user.Advertisements)
                  .ToListAsync();

            var userViewModelList = new List<GetApplicationUserViewModel>();

            foreach (var user in applicationUsers)
            {
                var userViewModel = MapUser(user);
                userViewModelList.Add(userViewModel);
            }

            return userViewModelList;
        }

        // GET: api/ApplicationUsers/5
        //[Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<GetApplicationUserViewModel>> GetApplicationUser(string id)
        {
            var applicationUser = await _context.ApplicationUsers
                 .Include(user => user.Favorites)
                 .Include(user => user.Advertisements)
                 .Where(user => user.Id == id)
                 .FirstOrDefaultAsync();

            if (applicationUser == null)
            {
                return NotFound();
            }

            return MapUser(applicationUser);
        }

        // POST: api/ApplicationUsers/5/favorites
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost("{id}/favorites")]
        public async Task<IActionResult> AddFavorite([FromRoute] string id, CreateFavoriteViewModel createFavoriteViewModel)
        {
            var applicationUser = await _context.ApplicationUsers
            .Include(user => user.Favorites)
            .Where(user => user.Id == id)
            .FirstOrDefaultAsync();

            if (applicationUser is null) { return NotFound(); }

            var advertisement = await _context.Advertisements
            .Where(advertisement => advertisement.Id == createFavoriteViewModel.AdvertisementId)
            .FirstOrDefaultAsync();

            if (advertisement is null) { return NotFound(); }

            applicationUser.Favorites.Add(new Favorite(advertisement.Id));

            _context.Entry(applicationUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }


        // DELETE: api/ApplicationUsers/5
        [Authorize]
        [HttpDelete("{userId}/favorites/{favoriteId}")]
        public async Task<IActionResult> DeleteApplicationUser(string userId, int favoriteId)
        {
            var applicationUser = await _context.ApplicationUsers.FindAsync(userId);

            if (applicationUser == null)
            {
                return NotFound();
            }

            var favorite = await _context.Favorites
            .Where(favorite => favorite.Id == favoriteId)
            .FirstOrDefaultAsync();

            if (favorite is null) { return NotFound(); }

            applicationUser.Favorites.Remove(favorite);
            
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ApplicationUserExists(string id)
        {
            return (_context.ApplicationUsers?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        static GetApplicationUserViewModel MapUser(ApplicationUser applicationUser)
        {
            var advertisementViewModels = new List<GetAdvertisementViewModel>();
            var favoriteViewModels = new List<GetFavoriteViewModel>();


            foreach (var advertisement in applicationUser.Advertisements)
            {
                var advertisementViewModel = new GetAdvertisementViewModel
                    (
                        advertisement.Id,
                        advertisement.Title,
                        advertisement.Description,
                        advertisement.Price,
                        advertisement.AddressLocation,
                        advertisement.AddressStreet,
                        advertisement.AddressNumber,
                        advertisement.PostalCode,
                        advertisement.Bedrooms,
                        advertisement.Bathrooms,
                        advertisement.Area,
                        advertisement.ContactPhoneNumber,
                        advertisement.PropertyType.ToString(),
                        advertisement.Status.ToString(),
                        advertisement.Picture);

                advertisementViewModels.Add(advertisementViewModel);
            }

            foreach (var favorite in applicationUser.Favorites)
            {
                var favoriteViewModel = new GetFavoriteViewModel
                    (
                        favorite.Id,
                        favorite.UserId,
                        favorite.AdvertisementId
                    );

                favoriteViewModels.Add(favoriteViewModel);
            }

            return new GetApplicationUserViewModel(
                applicationUser.Id, 
                applicationUser.UserName, 
                applicationUser.Email,
                applicationUser.FirstName,
                applicationUser.LastName,
                advertisementViewModels, 
                favoriteViewModels);
        }
    }
}
