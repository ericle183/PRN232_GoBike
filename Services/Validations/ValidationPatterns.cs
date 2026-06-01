using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Services;

public static partial class ValidationPatterns
{
    public static readonly Regex LicensePlateRegex = new(@"^[0-9]{2}[A-Z]{1,2}-[0-9]{4,5}$", RegexOptions.Compiled);
    public static readonly Regex CCCDRegex = new(@"^[0-9]{12}$", RegexOptions.Compiled);
    public static readonly Regex PhoneRegex = new(@"^0[0-9]{9,10}$", RegexOptions.Compiled);

    public static ValidationResult? ValidateAge(DateTime dateOfBirth, ValidationContext context)
    {
        var age = DateTime.Today.Year - dateOfBirth.Year;
        if (dateOfBirth.Date > DateTime.Today.AddYears(-age)) age--;
        if (age < 18)
            return new ValidationResult("Customer must be at least 18 years old to rent");
        return ValidationResult.Success;
    }

    public static bool IsValidLicensePlate(string licensePlate) => LicensePlateRegex.IsMatch(licensePlate);
    public static bool IsValidCCCD(string cccd) => CCCDRegex.IsMatch(cccd);
    public static bool IsValidPhone(string phone) => PhoneRegex.IsMatch(phone);
}
