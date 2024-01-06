using System.Xml.Serialization;

namespace vogue_decor.Domain.Enums
{
    /// <summary>
    /// Перечисление типов товаров
    /// </summary>
    public enum ProductTypes
    {
        /// <summary>
        /// Пустой тип
        /// </summary>
        [XmlEnum("0")]
        NONE,
        /// <summary>
        /// Люстра
        /// </summary>
        [XmlEnum("1")]
        ЛЮСТР,
        /// <summary>
        /// Бра
        /// </summary>
        [XmlEnum("2")]
        БРА,
        /// <summary>
        /// Напольная лампа
        /// </summary>
        [XmlEnum("3")]
        НАПОЛ,
        /// <summary>
        /// Настольная лампа
        /// </summary>
        [XmlEnum("4")]
        НАСТОЛ,
        /// <summary>
        /// Подвесной светильник
        /// </summary>
        [XmlEnum("5")]
        ПОДВЕС,
        /// <summary>
        /// Уличное освещение
        /// </summary>
        [XmlEnum("6")]
        УЛИЧ,
        /// <summary>
        /// Аксессуар
        /// </summary>
        [XmlEnum("7")]
        АКСЕСС
    }
}
