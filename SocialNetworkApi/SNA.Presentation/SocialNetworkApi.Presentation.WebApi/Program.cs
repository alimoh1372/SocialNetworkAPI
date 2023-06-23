using System.Text.Encodings.Web;
using System.Text.Unicode;
using _00_Framework.Application;
using _00_Framework.Domain;
using SocialNetworkApi.Infrastructure.Bootstrapper;
using SocialNetworkApi.Presentation.WebApi.Tools;


// Add services to the container.
var builder = WebApplication.CreateBuilder(args);
string connectionString = builder.Configuration.GetConnectionString("socialNetworkApiConnectionStringHome");
//string connectionString = builder.Configuration.GetConnectionString("socialNetworkApiConnectionStringNoc");
Configuration.Configure(builder.Services,connectionString);

//wire up and register the needed services
builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
builder.Services.AddTransient<IFileUpload, FileUpload>();

builder.Services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Arabic));


builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthorization();

app.MapControllers();

app.Run();
