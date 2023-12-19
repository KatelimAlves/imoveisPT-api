using ImoveisPT.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ImoveisPT.Data;

public class ImoveisPTContext : IdentityDbContext<ApplicationUser>
{
    public ImoveisPTContext(DbContextOptions<ImoveisPTContext> options)
        : base(options)
    {
    }

    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<Advertisement> Advertisements { get; set; }
    public DbSet<Favorite> Favorites { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}


/*
    A classe criada ImoveisPTContext herda da classe DbContext, 
    uma classe do Entity Framework(EF) usada para interagir com banco de dados.

    A classe ImoveisPTContext é o principal ponto de coordenação entre o meu modelo de dados e o banco de dados.
    Permite eu eu interaja com o banco de dados usando suas classes de entidade.

    O construtor aceita o parâmetro options do tipo DbContextOptions<ImoveisPTContext>.
    Este parâmetro é usado para configurar o contrexto do banco de dados.

    A propriedade PropertyTypes é um DbSet<PropertyType>. O Dbset é uma classe do EF que representa uma coleção de uma entidade específica.
    PropertyTypes representa uma coleção de entidades PropertyType. As operações CRUD são realizadas nesta coleção.

    =null!
    É apenas uma maniera de dizer ao compulador para ignorar o aviso de nulidade, já que EF inicializa as props do DbCOntext em null.
 */
