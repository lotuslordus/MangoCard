using MangoCard.Models; 

public class WalletCard
{
    public Guid WalletId { get; set; } // Prim�rschl�ssel

    public string CompanyId { get; set; } // Fremdschl�ssel f�r Company als string
    public Company Company { get; set; } // Navigation property

    public string TemplateData { get; set; }
    public int PointsToRedeemReward { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
