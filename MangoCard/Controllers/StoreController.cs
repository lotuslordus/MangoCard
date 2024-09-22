using MangoCard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QRCoder;
using System.Diagnostics;
using System.Drawing;
using System.Security.Claims;

namespace MangoCard.Controllers
{
    public class StoreController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context; // Dein DbContext


        public StoreController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // Die Index-Methode l�dt die Stores des angemeldeten Benutzers und zeigt die View "Index" (Store.cshtml) an
        public async Task<IActionResult> Store()
        {
            // Hole die CompanyId des angemeldeten Benutzers
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _logger.LogInformation("Da ist ein Fehler: " + userId);
            // Finde alle Stores, die zu dieser Company geh�ren
            var stores = await _context.Stores
                .Where(s => s.CompanyId == userId)
                .ToListAsync();

            return View(stores);  // Gibt die Liste der Stores an die View zur�ck
        }

        [HttpPost]
        public async Task<IActionResult> CreateStore([FromBody] Store store)
        {
            // Finde die Benutzer-ID (CompanyId ist die Benutzer-ID des IdentityUsers)
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Json(new { success = false, message = "Fehler: Benutzer nicht gefunden." });
            }

            // Setze die CompanyId vor der Validierung
            store.CompanyId = userId;
            ModelState.Clear(); 
            // �berpr�fe jetzt, ob das Model valide ist
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage);
                return Json(new { success = false, message = $"Fehler beim Speichern des Stores: {string.Join(", ", errors)}" });
            }

            // Erstelle eine neue StoreId und f�ge den Store hinzu
            store.StoreId = Guid.NewGuid();

            _context.Stores.Add(store);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Store wurde erfolgreich hinzugef�gt." });
        }


        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            // Suche den Store basierend auf der StoreId
            var store = await _context.Stores.FindAsync(id);

            if (store == null)
            {
                return Json(new { success = false, message = "Store nicht gefunden." });
            }

            // Entferne den Store aus der Datenbank
            _context.Stores.Remove(store);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Store erfolgreich gel�scht." });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
