namespace DotNetTUnitExample.Models;

/// <summary>
/// Represents a user in the system with basic properties and validation.
/// </summary>
public class User
{
    /// <summary>
    /// Gets or sets the unique identifier for the user.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the user's name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user's email address.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date and time when the user was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Initializes a new instance of the User class.
    /// </summary>
    public User()
    {
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Initializes a new instance of the User class with specified values.
    /// </summary>
    /// <param name="id">The user's unique identifier.</param>
    /// <param name="name">The user's name.</param>
    /// <param name="email">The user's email address.</param>
    public User(int id, string name, string email)
    {
        Id = id;
        Name = name ?? string.Empty;
        Email = email ?? string.Empty;
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Validates whether the user has valid data.
    /// </summary>
    /// <returns>True if the user is valid; otherwise, false.</returns>
    public bool IsValid()
    {
        return !string.IsNullOrEmpty(Name) && 
               !string.IsNullOrEmpty(Email) &&
               IsValidEmail(Email);
    }

    /// <summary>
    /// Validates the email format using a simple check.
    /// </summary>
    /// <param name="email">The email to validate.</param>
    /// <returns>True if the email format is valid; otherwise, false.</returns>
    private static bool IsValidEmail(string email)
    {
        return email.Contains('@') && email.Contains('.');
    }

    /// <summary>
    /// Returns a string representation of the user.
    /// </summary>
    /// <returns>A string containing the user's information.</returns>
    public override string ToString()
    {
        return $"User {{ Id: {Id}, Name: {Name}, Email: {Email}, CreatedAt: {CreatedAt:yyyy-MM-dd HH:mm:ss} }}";
    }
}