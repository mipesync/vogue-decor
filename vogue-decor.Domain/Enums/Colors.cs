using System.Xml.Serialization;

namespace vogue_decor.Domain.Enums
{
    /// <summary>
    /// Перечисление цветов
    /// </summary>
    public enum Colors
    {
        /// <summary>
        /// Пустой
        /// </summary>
        [XmlEnum("0")]
        NONE,
        /// <summary>
        /// Золотой
        /// </summary>
        [XmlEnum("1")]
        ЗОЛОТ,
        /// <summary>
        /// Бронзовый
        /// </summary>
        [XmlEnum("2")]
        БРОНЗ,
        /// <summary>
        /// Матовый серый
        /// </summary>
        [XmlEnum("3")]
        МАТОВ,
        /// <summary>
        /// никелевый
        /// </summary>
        [XmlEnum("4")]
        НИКЕЛ,
        /// <summary>
        /// Белый
        /// </summary>
        [XmlEnum("5")]
        БЕЛ,
        /// <summary>
        /// Чёрный
        /// </summary>
        [XmlEnum("6")]
        ЧЁРН,
        /// <summary>
        /// Прозрачный
        /// </summary>
        [XmlEnum("7")]
        ПРОЗРАЧН,
        /// <summary>
        /// Бежевый
        /// </summary>
        [XmlEnum("8")]
        БЕЖЕВ
    }
}
