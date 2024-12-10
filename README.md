# Configuración del Proyecto

## Configuración de `appsettings.json`

```json
{
    "Logging": {
        "LogLevel": {
            "Default": "Information",
            "Microsoft.Hosting.Lifetime": "Information"
        }
    },
    "ConnectionStrings": {
        "DefaultConnection": "Server=localhost;Port=3306;Database=culturaDb_local;User=myuser;Password=mypassword;SslMode=None;AllowPublicKeyRetrieval=true;"  
    },

    "EmailSettings": {
        "Port": 2525,
        "Host": "sandbox.smtp.mailtrap.io",
        "From": "from@example.com",
        "Username": "b46b85a1265f30",
        "Password": "10788cea302c87"
    },
    //Estos settings comentados son los necesarios para el proyecto
    //"AzureAd": {
    //    "Instance": "https://login.microsoftonline.com/",
    //    "TenantId": "84fd0453-7058-46cc-aed5-d12b401aa79c",
    //    "ClientId": "d108dd74-94c6-489f-9763-9f6de05a978e",
    //    "ClientSecret": "FuY8Q~0LmCc~gFPsoEPlw9loNHC2yFK0Bh5Hgdxx",
    //    "CallbackPath": "/signin-oidc",
    //    "Audience": "d108dd74-94c6-489f-9763-9f6de05a978e",
    //    "Scopes": "api://d108dd74-94c6-489f-9763-9f6de05a978e/user.read/user.read",
    //    "Authority": "https://login.microsoftonline.com/84fd0453-7058-46cc-aed5-d12b401aa79c/",
    //    "ServicePrincipalId": "833bcf53-3f28-4c14-9107-b132b1e341b9"
    //}
    //Estos settings comentados son de prueba
    "AzureAd": {
        "Instance": "https://login.microsoftonline.com/",
        "TenantId": "0c15fa0b-2f87-4fc7-a67e-ae3b1b5f53e7",
        "ClientId": "70b1da25-c6ec-417b-81d5-6aec5f23fb07",
        "ClientSecret": "Mr98Q~mq-z8hytNxKQdsrU7F2a4agUeeixwDvcvm",
        "CallbackPath": "/signin-oidc",
        "Audience": "70b1da25-c6ec-417b-81d5-6aec5f23fb07",
        "Scopes": "api://70b1da25-c6ec-417b-81d5-6aec5f23fb07/user.read",
        "Authority": "https://login.microsoftonline.com/0c15fa0b-2f87-4fc7-a67e-ae3b1b5f53e7/",
        "ServicePrincipalId": "96b41073-8601-4827-be54-d52994080767"
    }
}
```
# Configuración de la Base de Datos Local

## Crear la Base de Datos en Docker

Ejecuta el siguiente comando para crear e iniciar un contenedor de MySQL:

```bash
docker run --name mysql-db \
    -e MYSQL_ROOT_PASSWORD=rootpassword \
    -e MYSQL_DATABASE=culturaDb_local \
    -e MYSQL_USER=myuser \
    -e MYSQL_PASSWORD=mypassword \
    -p 3306:3306 \
    -d mysql:latest



Cadena de Conexión
MySQL (JDBC)

jdbc:mysql://localhost:3306/culturaDb_local?allowPublicKeyRetrieval=true&useSSL=false
```

