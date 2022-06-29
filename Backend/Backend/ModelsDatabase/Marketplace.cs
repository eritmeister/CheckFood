using System.ComponentModel.DataAnnotations;

namespace WebApi1.ModelsDatabase;

public class Marketplace
{
    [Key]
    public uint Id { get; set; }

    public string? MarketPlaceName { get; set; }

    public List<Product> Products { get; set; } = new();
}