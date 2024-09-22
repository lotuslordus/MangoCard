using System.ComponentModel.DataAnnotations;

public class CustomerRegistrationViewModel
{
    [Required(ErrorMessage = "Vorname ist erforderlich.")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Nachname ist erforderlich.")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "E-Mail ist erforderlich.")]
    [EmailAddress(ErrorMessage = "Ungültige E-Mail-Adresse.")]
    public string Email { get; set; }
}
