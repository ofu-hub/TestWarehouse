namespace TestWarehouse.Models;

/// <summary>
/// Абстрактный класс для хранения информации
/// </summary>
public abstract class StorageItem
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Ширина
    /// </summary>
    public double Width { get; set; }
    
    /// <summary>
    /// Высота
    /// </summary>
    public double Height { get; set; }
    
    /// <summary>
    /// Глубина
    /// </summary>
    public double Depth { get; set; }
    
    /// <summary>
    /// Вес
    /// </summary>
    public double Weight { get; set; }

    /// <summary>
    /// Абстрактный метод для получения объема
    /// </summary>
    /// <returns>Объем</returns>
    public abstract double GetVolume();
}
