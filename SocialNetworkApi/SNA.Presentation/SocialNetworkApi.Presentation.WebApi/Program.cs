using System.Reflection;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using _00_Framework.Application;
using _00_Framework.Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SocialNetworkApi.Infrastructure.Bootstrapper;
using SocialNetworkApi.Presentation.WebApi.Tools;


// Add services to the container.
var builder = WebApplication.CreateBuilder(args);
//string connectionString = builder.Configuration.GetConnectionString("socialNetworkApiConnectionStringHome");
string connectionString = builder.Configuration.GetConnectionString("socialNetworkApiConnectionStringNoc");
Configuration.Configure(builder.Services,connectionString);

//wire up and register the needed services
builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
builder.Services.AddTransient<IFileUpload, FileUpload>();
builder.Services.AddSingleton<IAuthHelper, AuthHelper>();
builder.Services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Arabic));
builder.Services.Configure<Jwt>(builder.Configuration.GetSection("Jwt"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddApiVersioning(options =>
//{
//    options.ReportApiVersions = true;
//    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
//    options.AssumeDefaultVersionWhenUnspecified = true;

//   // options.ApiVersionReader = new QueryStringApiVersionReader("hps-api-version");
//    //options.ApiVersionReader = new HeaderApiVersionReader("X-API-Version");
//});

//builder.Services.AddVersionedApiExplorer(options =>
//{
//    options.GroupNameFormat = "'v'VVV";
//    options.SubstituteApiVersionInUrl = true;
//});

builder.Services.AddAuthentication(configOpt =>
{
    configOpt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    configOpt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    configOpt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters
    {

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SigningKey"])),
        ValidateIssuer = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        TokenDecryptionKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:EncryptKey"])),
        RequireExpirationTime = true,
        ClockSkew = TimeSpan.Zero
    };
    opt.SaveToken = true;
    opt.RequireHttpsMetadata = false;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swgGenOpt =>
{
    swgGenOpt.SwaggerDoc("v1",new OpenApiInfo
    {
        Title = "Social Network Api V1",
        Description = "This is an api to work with social network operations...",
        Version = "v1.0",
    });
    var xmlDoc = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    swgGenOpt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlDoc));
    swgGenOpt.AddSecurityDefinition("Bearer",new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Description = "Please Insert jwt token with Bearer first of it",
        Name = "Authorization"
    });
    swgGenOpt.AddSecurityRequirement(new OpenApiSecurityRequirement {
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


builder.Services.AddHttpContextAccessor();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opt =>
    {
        opt.SwaggerEndpoint("/swagger/v1/swagger.json", "Social network Api v1");
    });
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthorization();

app.MapControllers();

app.Run();
