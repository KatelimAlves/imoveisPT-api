using ImoveisPT.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using ImoveisPT.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();
    //Add ImoveisPtContext no container DI e configura o EF para usar o banco de dados em mem�ria chamado "Im�veisPT".
builder.Services.AddDbContext<ImoveisPTContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ImoveisPTContext>()
    .AddDefaultTokenProviders();

// Adding Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})

// Adding Jwt Bearer
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
    };
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


// O container de inje��o de depend�ncia(DI) � usado para criar e fornecer inst�ncias de classes.
/* Ex.: Supondo que exista depend�ncias entre classes. Classe Carro depende de classe Motor.
 Sem DI, a classe motor teria que ser instanciada dentro do construtor de carro.
 Com DI, n�o � preciso instanciar. Basta passar motor como um par�metro ao construtor de Carro.
 O DI ger� as instancias.
A inje��o de depend�ncia (DI) � um padr�o de design que ajuda a reduzir o acoplamento entre classes e a tornar o c�digo mais modular e test�vel.
 */


/*
Um banco de dados em mem�ria � um BD que � armazenado na mem�ria do computador, em vez de ser armazenado na em um BD f�sico.
� �til para testes, pois o BD em mem�oria � criado e destu�do rapidamente.
 */
