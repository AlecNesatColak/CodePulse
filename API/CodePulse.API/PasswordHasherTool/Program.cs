using Microsoft.AspNetCore.Identity;

class Program
{
    static void Main(string[] args)
    {
        var password = "Admin@123";
        var hasher = new PasswordHasher<IdentityUser>();
        var user = new IdentityUser(); // user instance required but not used in result
        var hash = hasher.HashPassword(user, password);

        Console.WriteLine("Hashed Password:");
        Console.WriteLine(hash);
    }
}
