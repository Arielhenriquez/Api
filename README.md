# Api

Settings:

{
    "Logging": {
        "LogLevel": {
            "Default": "Information",
            "Microsoft.Hosting.Lifetime": "Information"
        }
    },
    "ConnectionStrings": {
        //"DefaultConnection": "Server=localhost;Port=3306;Database=culturaDb_local2;User=myuser;Password=mypassword;SslMode=None;AllowPublicKeyRetrieval=true;",
        "DefaultConnection": "Server=localhost;Port=3306;Database=culturaDb_local2;User=myuser;Password=mypassword;"
    },

    "EmailSettings": {
        "Port": 2525,
        "Host": "sandbox.smtp.mailtrap.io",
        "From": "from@example.com",
        "Username": "4a73f7c04e173c",
        "Password": "5ede0a8d794ca7"
    },
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




BD local docker: docker run --name mysql-db -e MYSQL_ROOT_PASSWORD=rootpassword -e MYSQL_DATABASE=culturaDb_local -e MYSQL_USER=myuser -e MYSQL_PASSWORD=mypassword -p 3306:3306 -d mysql:latest


Connection string: jdbc:mysql://localhost:3306/culturaDb_local?allowPublicKeyRetrieval=true&useSSL=false

Luego correr update database
