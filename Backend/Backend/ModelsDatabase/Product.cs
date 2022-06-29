using System.ComponentModel.DataAnnotations;

namespace WebApi1.ModelsDatabase;

public class Product
{
    /// <summary>
    /// первичный ключ
    /// </summary>
    [Key]
    public uint Id { get; set; }

    /// <summary>
    /// название продукта
    /// </summary>
    public string? ProductName { get; set; }
    
    /// <summary>
    /// магазин
    /// </summary>
    public Marketplace? Marketplace { get; set; }

    /// <summary>
    /// цена в копейках
    /// </summary>
    public int Price { get; set; }

    /// <summary>
    /// ссылка на картинку
    /// </summary>
    public string? Picture { get; set; }
}