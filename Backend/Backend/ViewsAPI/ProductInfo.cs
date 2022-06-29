namespace WebApi1.ViewsAPI;

public class ProductInfo
{
    /// <summary>
    /// название продукта
    /// </summary>
    public string? ProductName { get; set; }
    
    /// <summary>
    /// название магазина
    /// </summary>
    public string? ShopName { get; set; }

    /// <summary>
    /// цена в копейках
    /// </summary>
    public int Price { get; set; }

    /// <summary>
    /// ссылка на картинку
    /// </summary>
    public string? Picture { get; set; }
}