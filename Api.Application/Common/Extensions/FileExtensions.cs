using System.Globalization;

namespace Api.Application.Common.Extensions;

public static class FileExtensions
{
    public static string ReadEmailTemplate(string fileName, string templpateRouteEmail, string? templateRoute = null)
    {
        templateRoute ??= templpateRouteEmail;
        string textEmailTemplate = Path.Combine(templateRoute, fileName);
        string route = Path.Combine(templateRoute, CultureInfo.CurrentUICulture.Name);

        if (!Directory.Exists(route))
            return File.ReadAllText(textEmailTemplate);

        var fileRoutePlainText = Path.Combine(route, fileName);
        if (File.Exists(fileRoutePlainText))
            textEmailTemplate = fileRoutePlainText;

        return File.ReadAllText(textEmailTemplate);
    }
}
