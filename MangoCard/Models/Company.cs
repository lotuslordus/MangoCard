using Microsoft.AspNetCore.Identity;
using System.Collections.Generic; // Für ICollection

namespace MangoCard.Models 
{
    public class Company : IdentityUser
    {
        public string? CompanyName { get; set; }

        public decimal? MonthlyFee { get; set; }

        // Navigation properties
        public ICollection<Customer>? Customers { get; set; }
        public ICollection<Store>? Stores { get; set; }
        public ICollection<WalletCard>? WalletCards { get; set; }
    }
}
