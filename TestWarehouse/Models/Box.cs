using System.ComponentModel.DataAnnotations;

namespace TestWarehouse.Models; 

/// <summary>
/// Модель коробки
/// </summary>
public class Box : StorageItem
{
    /// <summary>
    /// Дата производства
    /// </summary>
    [Required(ErrorMessage = "Дата производства обязательна для заполнения.")]
    public DateTime? ManufactureDate { get; set; }
    
    /// <summary>
    /// Срок годности
    /// </summary>
    public DateTime? ExpirationDate { get; set; }
    
    /// <summary>
    /// Паллет, на котором находится коробка
    /// </summary>
    public Pallet? Pallet { get; set; }
    
    /// <summary>
    /// Идентификатор паллета
    /// </summary>
    public Guid PalletId { get; set; }

    /// <summary>
    /// Конструктор по умполчанию
    /// </summary>
    public Box() {}
    
    /// <summary>
    /// Конструктор с параметрами
    /// </summary>
    /// <param name="id"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="depth"></param>
    /// <param name="weight"></param>
    /// <param name="manufactureDate"></param>
    /// <param name="expirationDate"></param>
    /// <param name="palletId"></param>
    public Box(Guid id, double width, double height, double depth, double weight, Guid palletId, DateTime? manufactureDate = null,
        DateTime? expirationDate = null)
    {
        Id = id;
        Width = width;
        Height = height;
        Depth = depth;
        Weight = weight;
        ManufactureDate = manufactureDate;
        ExpirationDate = expirationDate;
        PalletId = palletId;

        if (ManufactureDate.HasValue && !ExpirationDate.HasValue)
        {
            CalculateExpirationDate();
        }
    }
    
    /// <summary>
    /// Метод для вычисления срока годности из даты производства
    /// </summary>
    public void CalculateExpirationDate()
    {
        if (ManufactureDate.HasValue)
        {
            ExpirationDate = ManufactureDate.Value.AddDays(100);
        }
    }

    /// <summary>
    /// Метод для расчета объема коробки
    /// </summary>
    /// <returns>Объем коробки</returns>
    public override double GetVolume()
    {
        return Width * Height * Depth;
    }
}
