using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ImoveisPT.Models;
using ImoveisPT.ViewModels;
using Microsoft.CodeAnalysis;
using ImoveisPT.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace ImoveisPT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvertisementsController : ControllerBase
    {
        private readonly ImoveisPTContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _acessor;

        public AdvertisementsController(
            ImoveisPTContext context, 
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor acessor)
        {
            _context = context;
            _acessor = acessor;
            _userManager = userManager;
        }

        // GET: api/Advertisements
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetAdvertisementViewModel>>> GetAdvertisements(
            [FromQuery] int bedrooms, 
            [FromQuery] int bathrooms,
            [FromQuery] PropertyType propertyType,
            [FromQuery] string? location,
            [FromQuery] int minPrice,
            [FromQuery] int maxPrice
            )
        {
            var advertisements = await _context.Advertisements
                .Where(advertisement => bedrooms <= 0 || advertisement.Bedrooms == bedrooms)
                .Where(advertisement => bathrooms <= 0 || advertisement.Bathrooms == bathrooms)
                .Where(advertisement => propertyType <= 0 || advertisement.PropertyType == propertyType)
                .Where(advertisement => location == null || advertisement.AddressLocation == location)
                .Where(advertisement => minPrice <= 0 || advertisement.Price >= minPrice)
                .Where(advertisement => maxPrice <= 0 || advertisement.Price <= maxPrice)
                .ToListAsync();

            var advertisementsModelList = new List<GetAdvertisementViewModel>();

            foreach (var advertisement in advertisements)
            {
                var advertisementViewModel = MapAdvertisement(advertisement);
                advertisementsModelList.Add(advertisementViewModel);
            }
            return advertisementsModelList;
        }

        // GET: api/Advertisements/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetAdvertisementViewModel>> GetAdvertisement(int id)
        {
            var advertisement = await _context.Advertisements
                  .Where(advertisement => advertisement.Id == id)
                  .FirstOrDefaultAsync();
 
            if (advertisement == null)
            {
                return NotFound();
            }

            return MapAdvertisement(advertisement);
        }

        // PUT: api/Advertisements/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAdvertisement(int id, UpdateAdvertisementViewModel advertisementViewModel)
        {
            var advertisement = await _context.Advertisements
                .Where(advertisement => advertisement.Id == id)
                .FirstOrDefaultAsync();

            if (advertisement is null) { return NotFound();  }

            advertisement.Title = advertisementViewModel.Title;
            advertisement.Description = advertisementViewModel.Description;
            advertisement.Price = advertisementViewModel.Price;
            advertisement.AddressLocation = advertisementViewModel.AddressLocation;
            advertisement.AddressStreet = advertisementViewModel.AddressStreet;
            advertisement.AddressNumber = advertisementViewModel.AddressNumber;
            advertisement.PostalCode = advertisementViewModel.PostalCode;
            advertisement.Bedrooms = advertisementViewModel.Bedrooms;
            advertisement.Bathrooms = advertisementViewModel.Bathrooms;
            advertisement.Area = advertisementViewModel.Area;
            advertisement.ContactPhoneNumber = advertisementViewModel.ContactPhoneNumber;
            advertisement.PropertyType = advertisementViewModel.PropertyType;
            advertisement.Status = advertisementViewModel.Status;
            advertisement.Picture = advertisementViewModel.Picture;

            _context.Entry(advertisement).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdvertisementExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Advertisements
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Advertisement>> PostAdvertisement([FromBody] CreateAdvertisementViewModel advertisementViewModel)
        {
            var userName = _acessor.HttpContext.User.Identity.Name;
            var userId = await _context.ApplicationUsers
                .Where(a => a.UserName == userName)
                .Select(a => a.Id)
                .FirstOrDefaultAsync();

            var advertisement = new Advertisement(
                advertisementViewModel.Title,
                advertisementViewModel.Description,
                advertisementViewModel.Price,
                advertisementViewModel.AddressLocation,
                advertisementViewModel.AddressStreet,
                advertisementViewModel.AddressNumber,
                advertisementViewModel.PostalCode,
                advertisementViewModel.Bedrooms,
                advertisementViewModel.Bathrooms,
                advertisementViewModel.Area,
                advertisementViewModel.ContactPhoneNumber,
                userId,
                advertisementViewModel.PropertyType,
                advertisementViewModel.Picture
                );

            _context.Advertisements.Add(advertisement);
            await _context.SaveChangesAsync();

            return CreatedAtAction(null, null);
        }

        // DELETE: api/Advertisements/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdvertisement(int id)
        {
            var advertisement = await _context.Advertisements.FindAsync(id);

            if (advertisement == null)
            {
                return NotFound();
            }

            _context.Advertisements.Remove(advertisement);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AdvertisementExists(int id)
        {
            return (_context.Advertisements?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        static GetAdvertisementViewModel MapAdvertisement (Advertisement advertisement)
        {
            return new GetAdvertisementViewModel(
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
                /*advertisement.UserId,*/
                advertisement.PropertyType.ToString(),
                advertisement.Status.ToString(),
                advertisement.Picture);
        }
    }
}
