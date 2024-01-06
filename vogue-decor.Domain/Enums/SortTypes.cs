namespace vogue_decor.Domain.Enums;

/// <summary>
/// Типы сортировки
/// </summary>
public enum SortTypes
{
    /// <summary>
    /// По рейтингу (возрастание)
    /// </summary>
    BY_RATING_ASC,
    /// <summary>
    /// По рейтингу (убывание)
    /// </summary>
    BY_RATING_DESC,
    /// <summary>
    /// По новизне (возрастание)
    /// </summary>
    BY_NOVELTY_ASC,
    /// <summary>
    /// По новизне (убывание)
    /// </summary>
    BY_NOVELTY_DESC,
    /// <summary>
    /// По популярности (убывание)
    /// </summary>
    BY_POPULARITY,
    /// <summary>
    /// По цене (возрастание)
    /// </summary>
    BY_PRICE_ASC,
    /// <summary>
    /// По цене (убывание)
    /// </summary>
    BY_PRICE_DESC
}