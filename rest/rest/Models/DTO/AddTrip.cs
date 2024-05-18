using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace rest.Models.DTO;

public class AddTrip
{
    [NotNull]
    public string FirstName { get; set; }
    [NotNull]
    public string LastName { get; set; }
    [NotNull]
    public string Email { get; set; }
    [NotNull]
    public string Telephone { get; set; }
    [StringLength(11)]
    public string Pesel { get; set; }
    [NotNull]
    public int IdTrip { get; set; }
    [NotNull]
    public string TripName { get; set; }
    public DateTime? PaymentDate { get; set; }
}