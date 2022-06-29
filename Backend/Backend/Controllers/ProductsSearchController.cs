using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi1.ModelsDatabase;
using WebApi1.ViewsAPI;

namespace WebApi1.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsSearchController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ProductsSearchController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// получить список товаров
    /// </summary>
    /// <param name="productName">название товара</param>
    /// <param name="searchMarketPlaceNames">в каких магазинах искать</param>
    /// <returns></returns>
    [HttpGet("/GetProductInfo")]
    public async Task<ProductInfo[]> GetProductInfo([FromQuery] string? productName,
        [FromQuery] string?[]? searchMarketPlaceNames = null)
    {
        if (string.IsNullOrWhiteSpace(productName))
            return new ProductInfo[] { };

        var productNameLower = productName.ToLower();

        ProductInfo[] productInfos;
        if (searchMarketPlaceNames != null && searchMarketPlaceNames.Length!=0)
        {
            string[] searchMarketPlaceNamesNormalized =
                searchMarketPlaceNames.Where(s => !string.IsNullOrWhiteSpace(s)).ToArray()!;

            if (searchMarketPlaceNamesNormalized.Length == 0)
                return new ProductInfo[] { };

            productInfos = await _context.Marketplaces.AsNoTracking()
                .Where(x => searchMarketPlaceNamesNormalized.Contains(x.MarketPlaceName)).Select(m =>
                    new
                    {
                        MarketplaceName = m.MarketPlaceName,
                        Product = m.Products.FirstOrDefault(p =>
                            p.ProductName != null && p.ProductName.ToLower().Contains(productNameLower))
                    }).Where(n => n.Product != null)
                .Select(n =>
                    new ProductInfo()
                    {
                        ShopName = n.MarketplaceName,
                        ProductName = n.Product!.ProductName,
                        Price = n.Product.Price, 
                        Picture = n.Product.Picture
                    }
                ).AsSplitQuery().ToArrayAsync();
        }
        else
        {
            productInfos = await _context.Products.Where(p => p.ProductName!.ToLower().Contains(productNameLower)).Select(
                    p =>
                        new ProductInfo()
                        {
                            ShopName = p.Marketplace!.MarketPlaceName,
                            ProductName = p.ProductName,
                            Price = p.Price,
                            Picture = p.Picture
                        }
                ).OrderBy(x => x.Price).ToArrayAsync();
        }
        return productInfos;
    }
}