using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

public class CardService
{
    private readonly AppDbContext _context;
    private readonly string certificatePath = "D:\\Projects\\CardLaboratory\\Certificates\\passcertificate.pem";
    private readonly string privateKeyPath = "D:\\Projects\\CardLaboratory\\Certificates\\passkey.pem";
    private readonly string wwdrCertificatePath = "D:\\Projects\\CardLaboratory\\Certificates\\WWDR.pem";
    private readonly string baseCardPath = "D:\\Projects\\CardLaboratory\\Cards";

    public CardService(AppDbContext context)
    {
        _context = context;
    }

    public async Task CreateWalletCard(string firstName, string lastName, string email, Guid customerId, string companyId)
    {
        // Holen des WalletCard Templates basierend auf der CompanyId
        var walletCard = await _context.WalletCards
            .FirstOrDefaultAsync(w => w.CompanyId == companyId);

        if (walletCard == null)
        {
            throw new Exception("Template für diese Company wurde nicht gefunden.");
        }

        // Laden und Parsen der Template-Daten aus dem WalletCard Template (pass.json)
        var passData = JObject.Parse(walletCard.TemplateData);

        // Anpassung der Felder im pass.json
        passData["description"] = $"Staff Pass for {firstName} {lastName}";
        passData["generic"]["primaryFields"][0]["value"] = $"{firstName} {lastName}";
        passData["barcode"]["message"] = customerId.ToString();  // QR-Code mit der CustomerId

        // Erstelle den Pfad für die Passdateien
        string cardDirectory = Path.Combine(baseCardPath, customerId.ToString());
        if (!Directory.Exists(cardDirectory))
        {
            Directory.CreateDirectory(cardDirectory);
        }

        // Speichern der angepassten pass.json in die Datei
        string passJsonPath = Path.Combine(cardDirectory, "pass.json");
        System.IO.File.WriteAllText(passJsonPath, passData.ToString());

        // Hier kannst du das Signieren und Erstellen der .pkpass Datei durchführen
        await SignPass(passJsonPath, customerId, cardDirectory);

        // TODO: Implementiere das Versenden der Wallet Card per E-Mail

        // TODO: Implementiere das Löschen der Wallet Card nach erfolgreichem Versand
    }

    private async Task SignPass(string passJsonPath, Guid customerId, string cardDirectory)
    {
        // Pfade für Manifest und Signatur
        string manifestPath = Path.Combine(cardDirectory, "manifest.json");
        string signaturePath = Path.Combine(cardDirectory, "signature");

        // Manifest generieren (enthält SHA-1-Hashes der Dateien, z.B. pass.json)
        string passJsonHash = GenerateFileHash(passJsonPath);
        var manifestData = new JObject
    {
        { "pass.json", passJsonHash }
    };
        System.IO.File.WriteAllText(manifestPath, manifestData.ToString());

        // Signiere das Manifest mit OpenSSL
        string opensslCommand = $"openssl smime -sign -signer {certificatePath} -inkey {privateKeyPath} -certfile {wwdrCertificatePath} -in {manifestPath} -out {signaturePath} -outform DER -binary";
        var processInfo = new System.Diagnostics.ProcessStartInfo("cmd.exe", "/c " + opensslCommand)
        {
            CreateNoWindow = true,
            UseShellExecute = false
        };
        var process = System.Diagnostics.Process.Start(processInfo);
        await process.WaitForExitAsync();

        // Erstelle die .pkpass Datei
        CreatePkpass(customerId, passJsonPath, manifestPath, signaturePath, cardDirectory);
    }


    private string GenerateFileHash(string filePath)
    {
        using (var sha1 = System.Security.Cryptography.SHA1.Create())
        {
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                byte[] hashBytes = sha1.ComputeHash(stream);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }

    private void CreatePkpass(Guid customerId, string passJsonPath, string manifestPath, string signaturePath, string cardDirectory)
    {
        string pkpassPath = Path.Combine(cardDirectory, $"{customerId}.pkpass");

        // ZIP die .pkpass Datei
        using (var zip = ZipFile.Open(pkpassPath, ZipArchiveMode.Create))
        {
            zip.CreateEntryFromFile(passJsonPath, "pass.json");
            zip.CreateEntryFromFile(manifestPath, "manifest.json");
            zip.CreateEntryFromFile(signaturePath, "signature");
        }
    }
}
