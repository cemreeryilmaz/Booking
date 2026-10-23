using Booking.Web.Options;
using Booking.Web.Services;
using Booking.Web.Services.Api;

var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();

// Booking API ayarlarını appsettings.json -> "BookingApi" bölümünden bağla
builder.Services.Configure<BookingApiOptions>(
    builder.Configuration.GetSection(BookingApiOptions.SectionName));

// Otel akışını yürüten üst seviye servis (arama/listeleme/detay) typed HttpClient olarak kaydedilir.
builder.Services.AddHttpClient<IBookingApiService, BookingApiService>();

// Tüm uç noktaları kapsayan düşük seviyeli istemci ve kategori servisleri.
// BookingApiClient kendi HttpClient'ını alır; kategori servisleri bu istemciyi kullanır.
builder.Services.AddHttpClient<BookingApiClient>();
builder.Services.AddScoped<IHotelsApi, HotelsApi>();
builder.Services.AddScoped<IFlightsApi, FlightsApi>();
builder.Services.AddScoped<ICarsApi, CarsApi>();
builder.Services.AddScoped<ITaxiApi, TaxiApi>();
builder.Services.AddScoped<IAttractionsApi, AttractionsApi>();
builder.Services.AddScoped<IMetaApi, MetaApi>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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
