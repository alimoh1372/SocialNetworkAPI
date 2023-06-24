using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using _00_Framework.Application;
using _00_Framework.Domain;
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

builder.Services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Arabic));


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
});

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
