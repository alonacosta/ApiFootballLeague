using ApiFootballLeague.Helpers;
using ApiFootballLeague.Models;
using ApiFootballLeague.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddIdentity<AspNetUser, IdentityRole>(cfg =>
{
    cfg.Tokens.AuthenticatorTokenProvider = TokenOptions.DefaultAuthenticatorProvider;
    cfg.SignIn.RequireConfirmedEmail = true;
    cfg.User.RequireUniqueEmail = true;
    cfg.Password.RequireDigit = false;  //disable carateres (no app real deve ser manter true)
    cfg.Password.RequiredUniqueChars = 0;
    cfg.Password.RequireLowercase = false; // true
    cfg.Password.RequireUppercase = false; // true
    cfg.Password.RequireNonAlphanumeric = false; // true
    cfg.Password.RequiredLength = 6; // 8-12               
})
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<LeagueDbContext>();

// configures the application to authenticate users using JWT tokens,
// verifying the issuer, audience, lifetime and signature key of the issuer
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            //defines the valid issuer and audience for the JWT token obtained from the application
            ValidAudience = builder.Configuration["JWT:Audience"],
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            //Defines the signing key used to sign and verify the JWT token.
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]!))
        };      
    });



builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Football League API", Version = "v1" });

    // Define um esquema securo para JWT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });
    // Implementa a autenticação em todos os endpoints da API
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


//var connection = builder.Configuration.GetConnectionString("DefaultConnection");
var connection = builder.Configuration.GetConnectionString("SomeeConnection");
builder.Services.AddDbContext<LeagueDbContext>(option => option.UseSqlServer(connection));

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IBlobHelper, BlobHelper>();
builder.Services.AddScoped<IUserHelper, UserHelper>();
builder.Services.AddScoped<IMailHelper, MailHelper>();


//builder.Services.AddSwaggerGen();

// Configuração de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    var context = app.Services.GetService<LeagueDbContext>();
    await context!.Database.MigrateAsync();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseHttpsRedirection();

// Adicione o middleware de CORS
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
