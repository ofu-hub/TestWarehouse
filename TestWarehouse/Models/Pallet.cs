namespace TestWarehouse.Models;

/// <summary>
/// Модель паллеты
/// </summary>
public class Pallet : StorageItem
{
    /// <summary>
    /// Базовый вес паллеты
    /// </summary>
    private const double PalletBaseWeight = 30;

    /// <summary>
    /// Список коробок на паллете
    /// </summary>
    public List<Box> Boxes = new List<Box>();

    /// <summary>
    /// Срок годности паллеты
    /// </summary>
    public DateTime? ExpirationDate { get; private set; }

    /// <summary>
    /// Конструктор по умолчанию
    /// </summary>
    public Pallet() {}
    
    /// <summary>
    /// Конструктор с параметрами
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <param name="width">Ширина</param>
    /// <param name="height">Высота</param>
    /// <param name="depth">Вес</param>
    public Pallet(Guid id, double width, double height, double depth)
    {
        Id = id;
        Width = width;
        Height = height;
        Depth = depth;
        Weight = PalletBaseWeight; // Начальный вес паллеты без коробок
    }

    /// <summary>
    /// Метод для добавления коробки на паллету
    /// </summary>
    /// <param name="box">Коробка</param>
    /// <exception cref="InvalidOperationException">Размеры коробки превышают размеры паллеты</exception>
    public void AddBox(Box box)
    {
        if (box.Width <= Width && box.Depth <= Depth) // Проверка размера коробки перед добавлением
        {
            Boxes.Add(box);
            RecalculateWeightAndExpirationDate(); // Пересчет веса и срока годности
        }
        else
        {
            throw new InvalidOperationException("Размеры коробки превышают размеры паллеты.");
        }
    }

    /// <summary>
    /// Пересчет веса и срока годности паллеты
    /// </summary>
    private void RecalculateWeightAndExpirationDate()
    {
        Weight = PalletBaseWeight + Boxes.Sum(box => box.Weight);
        ExpirationDate = Boxes.Where(box => box.ExpirationDate.HasValue)
            .Min(box => box.ExpirationDate); // Вычисление наименьшего срока годности
    }

    /// <summary>
    /// Метод для расчета объема паллеты
    /// </summary>
    /// <returns>Объем паллеты</returns>
    public override double GetVolume()
    {
        double boxVolume = Boxes.Sum(box => box.GetVolume());
        double palletVolume = Width * Height * Depth;
        return boxVolume + palletVolume;
    }
}
