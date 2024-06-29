using System;
using System.Text.RegularExpressions;

namespace ApiLab7;

public class AddressValidator
{
    public static bool ValidateAddressFormat(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
            return false;

        var commaCountRegex = new Regex(",", RegexOptions.Compiled);
        var cantonRegex = new Regex(@"\bCantón\b", RegexOptions.Compiled);
        var provinciaRegex = new Regex(@"\bProvincia\b", RegexOptions.Compiled);
        var zipCodeRegex = new Regex(@"\b\d{5}\b", RegexOptions.Compiled);

        var commaCount = commaCountRegex.Matches(address).Count;

        var addressParts = address.Split(',');

        bool validParts = true;
        foreach (var part in addressParts)
        {
            if (part.Trim().Length < 3)
            {
                validParts = false;
                break;
            }
        }

        var hasEnoughCommas = commaCount >= 4;
        var hasCantonKeyword = cantonRegex.IsMatch(address);
        var hasProvinciaKeyword = provinciaRegex.IsMatch(address);
        var hasZipCodeFormat = zipCodeRegex.IsMatch(address);

        return hasEnoughCommas && hasCantonKeyword && hasProvinciaKeyword && hasZipCodeFormat && validParts;
    }
}