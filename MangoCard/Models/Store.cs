public class Store
{
    public Guid StoreId { get; set; } // Prim�rschl�ssel

    public string CompanyId { get; set; } // Fremdschl�ssel f�r Company als string
    public Company Company { get; set; } // Navigation property

    public string StoreName { get; set; }
    public string Location { get; set; }
}
