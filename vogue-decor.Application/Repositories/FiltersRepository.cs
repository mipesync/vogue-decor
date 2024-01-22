using vogue_decor.Application.Common.Exceptions;
using vogue_decor.Application.DTOs.FilterDTOs;
using vogue_decor.Application.Interfaces;
using vogue_decor.Application.Interfaces.Repositories;
using vogue_decor.Domain;
using Microsoft.EntityFrameworkCore;

namespace vogue_decor.Application.Repositories;

public class FiltersRepository : IFiltersRepository
{
    private readonly IDBContext _dbContext;

    public FiltersRepository(IDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddColor(string name, string engName)
    {
        var color = new Color { Name = name, EngName = engName};

        await _dbContext.Colors.AddAsync(color, CancellationToken.None);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
    }

    public async Task DeleteColor(int id)
    {
        var color = await _dbContext.Colors.FirstOrDefaultAsync(u => u.Id == id, CancellationToken.None);

        if (color is null)
            throw new NotFoundException("Цвет не найден");

        _dbContext.Colors.Remove(color);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
    }

    public async Task AddProductType(string name)
    {
        var productType = new ProductType { Name = name };

        await _dbContext.ProductTypes.AddAsync(productType, CancellationToken.None);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
    }

    public async Task DeleteProductType(int id)
    {
        var productType = await _dbContext.ProductTypes.FirstOrDefaultAsync(u => u.Id == id, CancellationToken.None);

        if (productType is null)
            throw new NotFoundException("Тип товара не найден");

        _dbContext.ProductTypes.Remove(productType);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
    }

    public async Task AddChandelierType(string name)
    {
        var chandelierType = new ChandelierType { Name = name };

        await _dbContext.ChandelierTypes.AddAsync(chandelierType, CancellationToken.None);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
    }

    public async Task DeleteChandelierType(int id)
    {
        var chandelierType = await _dbContext.ChandelierTypes.FirstOrDefaultAsync(u => u.Id == id, CancellationToken.None);

        if (chandelierType is null)
            throw new NotFoundException("Тип люстры не найден");

        _dbContext.ChandelierTypes.Remove(chandelierType);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
    }

    public async Task AddCategory(string name, int productTypeId)
    {
        var category = new Category { Name = name, ProductTypeId = productTypeId};

        await _dbContext.Categories.AddAsync(category, CancellationToken.None);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
    }

    public async Task DeleteCategory(int id)
    {
        var category = await _dbContext.Categories.FirstOrDefaultAsync(u => u.Id == id, CancellationToken.None);

        if (category is null)
            throw new NotFoundException("Категория не найдена");

        _dbContext.Categories.Remove(category);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
    }

    public async Task AddMaterial(string name)
    {
        var material = new Material { Name = name };

        await _dbContext.Materials.AddAsync(material, CancellationToken.None);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
    }

    public async Task DeleteMaterial(int id)
    {
        var material = await _dbContext.Materials.FirstOrDefaultAsync(u => u.Id == id, CancellationToken.None);

        if (material is null)
            throw new NotFoundException("Материал не найден");

        _dbContext.Materials.Remove(material);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
    }

    public async Task AddStyle(string name)
    {
        var style = new Style { Name = name };

        await _dbContext.Styles.AddAsync(style, CancellationToken.None);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
    }

    public async Task DeleteStyle(int id)
    {
        var style = await _dbContext.Styles.FirstOrDefaultAsync(u => u.Id == id, CancellationToken.None);

        if (style is null)
            throw new NotFoundException("Стиль не найден");

        _dbContext.Styles.Remove(style);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
    }

    public async Task<FiltersResponseDto> GetFilters()
    {
        var result = new FiltersResponseDto();
    
        var colors = await _dbContext.Colors.ToListAsync(CancellationToken.None);
        var productTypes = await _dbContext.ProductTypes.ToListAsync(CancellationToken.None);
        var chandelierTypes = await _dbContext.ChandelierTypes.ToListAsync(CancellationToken.None);
        var categories = await _dbContext.Categories.ToListAsync(CancellationToken.None);
        var materials = await _dbContext.Materials.ToListAsync(CancellationToken.None);
        var styles = await _dbContext.Styles.ToListAsync(CancellationToken.None);
    
        result.Colors = colors.Select(c => new ColorFilterLookup
        {
            Id = c.Id,
            Name = c.Name,
            EngName = c.EngName
        }).ToList();
    
        result.ProductTypes = productTypes.Select(p => new FilterLookup
        {
            Id = p.Id,
            Name = p.Name
        }).ToList();
    
        result.ChandelierTypes = chandelierTypes.Select(c => new FilterLookup
        {
            Id = c.Id,
            Name = c.Name
        }).ToList();
    
        result.Categories = categories.Select(c => new CategoryFilterLookup
        {
            Id = c.Id,
            Name = c.Name,
            ProductId = c.ProductTypeId
        }).ToList();
    
        result.Materials = materials.Select(m => new FilterLookup
        {
            Id = m.Id,
            Name = m.Name
        }).ToList();
    
        result.Styles = styles.Select(s => new FilterLookup
        {
            Id = s.Id,
            Name = s.Name
        }).ToList();

        return result;
    }

}