public class Store
{
    public Guid StoreId { get; set; } // Primärschlüssel

    public string CompanyId { get; set; } // Fremdschlüssel für Company als string
    public Company Company { get; set; } // Navigation property

    public string StoreName { get; set; }
    public string Location { get; set; }
}
