namespace Api.Application.Common.Extensions;

public static class RequestCodeHelper
{
    public static string GenerateRequestCode(string requestType, Func<string, int?> getLastCodeNumber)
    {
        if (string.IsNullOrWhiteSpace(requestType))
            throw new ArgumentException("Request type cannot be null or empty");

        string prefix = requestType.ToUpper() switch
        {
            "TRANSPORTE" => "T",
            "ALMACEN" => "A",
            _ => throw new ArgumentException("Invalid request type. Allowed values are 'Transporte' or 'Almacen'")
        };

        int lastNumber = getLastCodeNumber(prefix) ?? 0;

        int nextNumber = lastNumber + 1;

        return $"{prefix}{nextNumber:D6}";
    }
}
