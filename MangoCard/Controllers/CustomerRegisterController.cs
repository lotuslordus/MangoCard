using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

public class CustomerRegisterController : Controller
{
    private readonly AppDbContext _context;

    public CustomerRegisterController(AppDbContext context)
    {
        _context = context;
    }

    // Endpunkt für die URL: /CustomerRegister/{storeId}
    [HttpGet("/CustomerRegistration/{storeId}")]
    public async Task<IActionResult> Register(Guid storeId)
    {
        // Finde den Store anhand der StoreId
        var store = await _context.Stores
            .Include(s => s.Company)  // Lade die zugehörige Company
            .FirstOrDefaultAsync(s => s.StoreId == storeId);

        if (store == null)
        {
            return NotFound("Store nicht gefunden.");
        }

        // Den Store-Name und andere relevante Informationen an die View übergeben
        ViewBag.StoreId = storeId;
        ViewBag.StoreName = store.StoreName;  // Speichere den Store-Namen in der ViewBag
        ViewBag.CompanyName = store.Company.CompanyName;

        return View();
    }

    // POST: /CustomerRegister/{storeId}
    [HttpPost("/CustomerRegister/{storeId}")]
    public async Task<IActionResult> Register(Guid storeId, [FromForm] CustomerRegistrationViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // Finde den Store und die zugehörige Company
        var store = await _context.Stores
            .Include(s => s.Company)  // Lade die verknüpfte Company
            .FirstOrDefaultAsync(s => s.StoreId == storeId);

        if (store == null)
        {
            return NotFound("Store nicht gefunden.");
        }

        // Erstelle eine neue Kundenkarte für diesen Store und Company
        var customer = new Customer
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            StoreId = storeId,  // Weise den Kunden dem Store zu
            CompanyId = store.Company.Id // Weise den Kunden der Company zu
        };

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        // Erfolgsseite anzeigen oder auf eine andere Seite umleiten
        return RedirectToAction("Success");
    }

    // Erfolgsseite nach der Registrierung
    public IActionResult Success()
    {
        return View();
    }
}
