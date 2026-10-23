namespace Booking.Web.Models
{
    /// <summary>API Gezgini'nde tek bir uç noktayı temsil eder.</summary>
    /// <param name="Group">Kategori anahtarı (meta, hotels, flights, cars, taxi, attraction).</param>
    /// <param name="Name">Uç nokta adı (ör. getLanguages).</param>
    /// <param name="Path">Tam yol (ör. /api/v1/meta/getLanguages).</param>
    /// <param name="Description">Kısa Türkçe açıklama.</param>
    /// <param name="IsLive">Gezginde canlı denenebiliyor mu?</param>
    public record ApiEndpointInfo(string Group, string Name, string Path, string Description, bool IsLive);

    /// <summary>Bir kategori başlığı ve altındaki uç noktalar.</summary>
    /// <param name="Title">Kategori görünen adı.</param>
    /// <param name="Endpoints">Kategori altındaki uç noktalar.</param>
    public record ApiEndpointGroup(string Title, List<ApiEndpointInfo> Endpoints);
}
