namespace Api.Application.Common.Extensions;

public static class RequestCodeHelper
{
    public static string GenerateRequestCode(string departmentName)
    {
        if (string.IsNullOrWhiteSpace(departmentName))
            throw new ArgumentException("Department name cannot be null or empty");

        var departmentCode = departmentName.Split(' ')[0].Substring(0, 3).ToUpper();

        var randomCode = new Random().Next(100000, 999999);

        return $"{departmentCode}{randomCode}";
    }
}

