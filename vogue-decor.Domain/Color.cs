﻿using NpgsqlTypes;
using vogue_decor.Domain.Interfaces;

namespace vogue_decor.Domain;

/// <summary>
/// Класс цвета
/// </summary>
public class Color : IBaseFilterItem, IEngName
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string EngName { get; set; } = null!;
    public NpgsqlTsVector? SearchVector { get; set; }
}