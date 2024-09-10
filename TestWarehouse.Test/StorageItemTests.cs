using TestWarehouse.Models;

namespace TestWarehouse.Tests;

/// <summary>
/// Тесты для моделей
/// </summary>
public class StorageItemTests
{
    /// <summary>
    /// Тест проверяет, что правильно рассчитывает срок годности на основе даты производства
    /// </summary>
    [Fact]
    public void Box_CalculateExpirationDate_Should_SetExpirationDate_When_ManufactureDate_IsGiven()
    {
        // Arrange
        var pallet = new Pallet(Guid.NewGuid(), 3, 2, 2);
        var box = new Box(Guid.NewGuid(), 2, 1, 1, 5, pallet.Id, new DateTime(2023, 1, 1));

        // Act
        box.CalculateExpirationDate();

        // Assert
        Assert.Equal(new DateTime(2023, 4, 11), box.ExpirationDate);
    }

    /// <summary>
    /// Тест проверяет корректность расчета объема для коробки
    /// </summary>
    [Fact]
    public void Box_GetVolume_Should_Calculate_CorrectVolume()
    {
        // Arrange
        var pallet = new Pallet(Guid.NewGuid(), 3, 2, 2);
        var box = new Box(Guid.NewGuid(), 2, 1, 1, 5, pallet.Id);

        // Act
        double volume = box.GetVolume();

        // Assert
        Assert.Equal(2, volume);
    }

    /// <summary>
    /// Тест проверяет, что добавление коробки, которая превышает размеры паллеты, вызывает исключение
    /// </summary>
    [Fact]
    public void Pallet_AddBox_Should_ThrowException_When_BoxExceedsPalletSize()
    {
        // Arrange
        var pallet = new Pallet(Guid.NewGuid(), 3, 2, 2);
        var largeBox = new Box(Guid.NewGuid(), 4, 1, 1, 5, pallet.Id); // ширина больше чем паллета

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => pallet.AddBox(largeBox));
    }

    /// <summary>
    /// Тест проверяет, что срок годности паллеты правильно устанавливается как минимальный срок годности среди всех коробок
    /// </summary>
    [Fact]
    public void Pallet_CalculateExpirationDate_Should_SetExpirationDate_As_Minimum_Of_Boxes()
    {
        // Arrange
        var pallet = new Pallet(Guid.NewGuid(), 3, 2, 2);
        var box1 = new Box(Guid.NewGuid(), 2, 1, 1, 5, pallet.Id, new DateTime(2023, 1, 1));
        var box2 = new Box(Guid.NewGuid(), 1, 1, 1, 3, pallet.Id, expirationDate: new DateTime(2023, 4, 1));

        pallet.AddBox(box1);
        pallet.AddBox(box2);

        // Act
        var expirationDate = pallet.ExpirationDate;

        // Assert
        Assert.Equal(new DateTime(2023, 4, 1), expirationDate);
    }

    /// <summary>
    /// Тест проверяет корректность расчета объема паллеты с учетом вложенных коробок
    /// </summary>
    [Fact]
    public void Pallet_GetVolume_Should_Calculate_CorrectVolume_With_Boxes()
    {
        // Arrange
        var pallet = new Pallet(Guid.NewGuid(), 3, 2, 2); // объем 12
        var box1 = new Box(Guid.NewGuid(), 2, 1, 1, 5, pallet.Id); // объем 2
        var box2 = new Box(Guid.NewGuid(), 1, 1, 1, 3, pallet.Id); // объем 1

        pallet.AddBox(box1);
        pallet.AddBox(box2);

        // Act
        double volume = pallet.GetVolume();

        // Assert
        Assert.Equal(15, volume); // 2 (Box1) + 1 (Box2) + 12 (this Pallet) = 15
    }
}