namespace Api.Domain.Settings;

public class EmailSettings
{
    public required string Port {  get; set; }
    public required string Host { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }

    //Sendgrid cuando llegue xd
    public required string Email { get; set; }
   // public required string Password { get; set; }
    public required string Client { get; set; }
    public required string Name { get; set; }
}
