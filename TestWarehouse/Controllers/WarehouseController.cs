using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestWarehouse.Models;

namespace TestWarehouse.Controllers;

public class WarehouseController : Controller
{
    private readonly DatabaseContext _context;

    public WarehouseController(DatabaseContext context)
    {
        _context = context;
    }

    // Метод для отображения списка паллет и коробок
    public async Task<IActionResult> Index()
    {
        var pallets = await _context.Pallets.Include(p => p.Boxes).ToListAsync();
        var groupedPallets = pallets
            .Where(p => p.ExpirationDate.HasValue)
            .OrderBy(p => p.ExpirationDate)
            .ThenBy(p => p.Weight)
            .GroupBy(p => p.ExpirationDate.Value.Date)
            .ToList();

        var top3Pallets = pallets
            .Where(p => p.Boxes.Count > 0)
            .OrderByDescending(p => p.Boxes.Max(box => box.ExpirationDate))
            .ThenBy(p => p.GetVolume())
            .Take(3)
            .ToList();

        ViewData["Pallets"] = pallets;
        ViewData["GroupedPallets"] = groupedPallets;
        ViewData["Top3Pallets"] = top3Pallets;

        return View();
    }

    // Метод для отображения формы добавления паллеты
    public IActionResult CreatePallet()
    {
        return View();
    }

    // Метод для обработки формы добавления паллеты
    [HttpPost]
    public async Task<IActionResult> CreatePallet(Pallet pallet)
    {
        if (ModelState.IsValid)
        {
            Pallet newPallet = new Pallet(Guid.NewGuid(), pallet.Width, pallet.Height, pallet.Depth);
            
            _context.Pallets.Add(newPallet);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(pallet);
    }

    // Метод для удаления паллеты
    public async Task<IActionResult> DeletePallet(Guid id)
    {
        var pallet = await _context.Pallets.Include(p => p.Boxes).FirstOrDefaultAsync(p => p.Id == id);
        if (pallet != null)
        {
            _context.Pallets.Remove(pallet);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    // Метод для отображения формы добавления коробки
    public IActionResult CreateBox(Guid palletId)
    {
        ViewData["PalletId"] = palletId; // Используем ViewData для передачи идентификатора паллеты в представление
        return View();
    }


    // Метод для обработки формы добавления коробки
    [HttpPost]
    public async Task<IActionResult> CreateBox(Box box, Guid palletId)
    {
        if (ModelState.IsValid)
        {
            var pallet = _context.Pallets.Include(p => p.Boxes).FirstOrDefault(p => p.Id == palletId);

            if (pallet == null)
            {
                return NotFound();
            }

            box.PalletId = palletId;
            
            Box newBox = new Box(Guid.NewGuid(), box.Width, box.Height, box.Depth, box.Weight, box.PalletId, box.ManufactureDate, box.ExpirationDate);
            _context.Boxes.Add(newBox);
            
            pallet.AddBox(newBox);
            _context.Pallets.Update(pallet);
            await _context.SaveChangesAsync();
        
            return RedirectToAction("Index");
        }

        return View(box);
    }

    // Метод для удаления коробки
    public async Task<IActionResult> DeleteBox(Guid id)
    {
        var box = await _context.Boxes.FindAsync(id);
        if (box != null)
        {
            _context.Boxes.Remove(box);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}