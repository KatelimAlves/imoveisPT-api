using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImoveisPT.Models
{
    public enum PropertyStatus
    {
        Disponível,
        Reservado,
        Vendido
    }

    public enum PropertyType
    {
        Apartamento,
        Armazém,
        Escritório,
        Estúdio,
        Garagem,
        Loja,
        Moradia,
        Prédio,
        Quinta,
        Terreno
    }

    public class Advertisement
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [MaxLength(300)]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        [MaxLength(30)]
        public string AddressLocation { get; set; }

        [Required]
        [MaxLength(30)]
        public string AddressStreet { get; set; }

        [Required]
        public int AddressNumber { get; set; }

        [Required]
        [MaxLength(8)]
        public string PostalCode { get; set; }

        public int? Bedrooms { get; set; }

        public int? Bathrooms { get; set; }

        public decimal? Area { get; set; }

        [Required]
        [MaxLength(20)]
        public string ContactPhoneNumber { get; set; }

        public DateTime CreatedAt { get; set; } //Entity entende e cria automaticamente.

        public string? Picture { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; } = null!;

        [Required]
        public PropertyType PropertyType { get; set; }


        [Required]
        public PropertyStatus Status { get; set; }

        public virtual ICollection<Favorite>? Favorites { get; set; }


        public Advertisement()
        {

        }

        public Advertisement(
            string title,
            string description,
            decimal price,
            string addressLocation,
            string addressStreet,
            int addressNumber,
            string postalCode,
            int? bedrooms,
            int? bathrooms,
            decimal? area,
            string contactPhoneNumber,
            string userId,
            PropertyType propertyType,
            string? picture
            )
        {
            Title = title;
            Description = description;
            Price = price;
            AddressLocation = addressLocation;
            AddressStreet = addressStreet;
            AddressNumber = addressNumber;
            PostalCode = postalCode;
            Bedrooms = bedrooms;
            Bathrooms = bathrooms;
            Area = area;
            ContactPhoneNumber = contactPhoneNumber;
            UserId = userId;
            PropertyType = propertyType;
            Status = PropertyStatus.Disponível;
            Picture = picture;
            CreatedAt = DateTime.UtcNow;
        }
    }
}

/* A Palavra chave virtual é usada para permitir o lazy loading (carregamento lento) de entidades relacionadas.
 * As propriedades são carregadas automaticamento do bd quando são acessadas pela primeira vez.
 */