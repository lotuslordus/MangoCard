using MangoCard.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

public class Customer
{
    public Guid CustomerId { get; set; }
    public string CompanyId { get; set; } // Foreign key
    public Company Company { get; set; }
    public Guid StoreId { get; set; } 
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public int Points { get; set; }
}
