using ImoveisPT.Models;

namespace ImoveisPT.ViewModels
{

    // ApplicationUsers
    public record GetApplicationUserViewModel(string Id, string UserName, string Email, List<GetAdvertisementViewModel> Advertisements, List<GetFavoriteViewModel> Favorites);
    public record UpdateApplicationUserViewModel(string UserName, string Email, List<CreateFavoriteViewModel> Favorites);

    // Advertisements
    public record GetAdvertisementViewModel(
    int Id,
    string Title,
    string Description,
    decimal Price,
    string AddressLocation,
    string AddressStreet,
    int AddressNumber,
    string PostalCode,
    int? Bedrooms,
    int? Bathrooms,
    decimal? Area,
    string ContactPhoneNumber,
    string PropertyType,
    string Status,
    string? Picture
    );


    public record CreateAdvertisementViewModel(
        string Title, 
        string Description,
        decimal Price,
        string AddressLocation,
        string AddressStreet,
        int AddressNumber,
        string PostalCode,
        int? Bedrooms,
        int? Bathrooms,
        decimal? Area,
        string ContactPhoneNumber,
        PropertyType PropertyType,
        string? Picture
        );
   

    public record UpdateAdvertisementViewModel(
        string Title,
        string Description,
        decimal Price,
        string AddressLocation,
        string AddressStreet,
        int AddressNumber,
        string PostalCode,
        int? Bedrooms,
        int? Bathrooms,
        decimal? Area,
        string ContactPhoneNumber,
        PropertyType PropertyType,
        PropertyStatus Status,
        string? Picture
        );




    // Favorites
    public record GetFavoriteViewModel(int Id, string UserId, int AdvertisementId);
    public record CreateFavoriteViewModel(int AdvertisementId);
    

}
