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

    public async Task AddChanType(string name)
    {
        var chanType = new ChandelierType { Name = name };

        await _dbContext.ChandelierTypes.AddAsync(chanType, CancellationToken.None);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
    }

    public async Task DeleteChanType(int id)
    {
        var chanType = await _dbContext.ChandelierTypes.FirstOrDefaultAsync(u => u.Id == id, CancellationToken.None);

        if (chanType is null)
            throw new NotFoundException("Тип люстры не найден");

        _dbContext.ChandelierTypes.Remove(chanType);
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

    public async Task<FiltersResponseDto> GetFilters()
    {
        var result = new FiltersResponseDto();
        
        var colors = await _dbContext.Colors.ToListAsync();
        var chanTypes = await _dbContext.ChandelierTypes.ToListAsync();
        var productTypes = await _dbContext.ProductTypes.ToListAsync();

        foreach (var color in colors)
        {
            result.Colors.Add(new ColorFilterLookup()
            {
                Id = color.Id,
                Name = color.Name,
                EngName = color.EngName
            });
        }
        
        foreach (var chanType in chanTypes)
        {
            result.ChandelierTypes.Add(new FilterLookup
            {
                Id = chanType.Id,
                Name = chanType.Name
            });
        }
        
        foreach (var productType in productTypes)
        {
            result.ProductTypes.Add(new FilterLookup
            {
                Id = productType.Id,
                Name = productType.Name
            });
        }

        return result;
    }
}