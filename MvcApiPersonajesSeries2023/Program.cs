using Azure.Storage.Blobs;
using MvcApiPersonajesSeries2023.Helpers;
using MvcApiPersonajesSeries2023.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//RECUPERAMOS TODAS LAS CLAVES
string azureKeys =
    builder.Configuration.GetConnectionString("azurestoragekeys");
string personajesContainer =
    builder.Configuration.GetValue<string>("AzureContainers:personajescontainer");
//CREAMOS NUESTRO CLIENTE PARA ACCEDER AL SERVICIO DE AZURE BLOBS
//MEDIANTE NUESTRAS CLAVES
BlobServiceClient blobServiceClient = new BlobServiceClient(azureKeys);
//CREAMOS NUESTRO CONTAINER CLIENT CON EL NOMBRE DEL CONTENEDOR
BlobContainerClient containerClient =
    blobServiceClient.GetBlobContainerClient(personajesContainer);
builder.Services.AddTransient<BlobContainerClient>(x => containerClient);
//PONEMOS NUESTRO SERVICE STORAGE BLOBS EN LA APLICACION
builder.Services.AddTransient<ServiceStorageBlobs>();

string apiSeries =
    builder.Configuration.GetValue<string>("ApiUrls:ApiSeriesPersonajes");
builder.Services.AddTransient<ServiceSeries>(z => new ServiceSeries(apiSeries));
builder.Services.AddTransient<HelperPathProvider>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
