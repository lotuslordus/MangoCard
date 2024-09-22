using MangoCard.Models;
using System.ComponentModel.DataAnnotations;


namespace MangoCard.Models
{
    public class Store
    {
        public Guid StoreId { get; set; } // Prim�rschl�ssel
        [Required]
        public string CompanyId { get; set; } // Fremdschl�ssel f�r Company als string
        public Company? Company { get; set; } // Navigation property

        public string StoreName { get; set; }
        public string Address { get; set; }
        public string PLZ { get; set; }
        public string City { get; set; }
        public string? ContactPerson { get; set; }
    }

}
