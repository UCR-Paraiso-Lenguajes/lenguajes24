export const validateAddressFormat = (address : string) => {
    const commaCountRegex = /,/g;
    const cantonRegex = /\bCantón\b/;
    const provinciaRegex = /\bProvincia\b/;
    const zipCodeRegex = /\b\d{5}\b/;

    const commaCount = (address.match(commaCountRegex) || []).length;

    const hasEnoughCommas = commaCount >= 4;
    const hasCantonKeyword = cantonRegex.test(address);
    const hasProvinciaKeyword = provinciaRegex.test(address);
    const hasZipCodeFormat = zipCodeRegex.test(address);

    if (hasEnoughCommas && hasCantonKeyword && hasProvinciaKeyword && hasZipCodeFormat) {
        return true;
    } else {
        return false;
    }
}