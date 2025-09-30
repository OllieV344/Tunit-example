using DotNetTUnitExample.Models;
using System.Diagnostics;

namespace DotNetTUnitExample.Services;

/// <summary>
/// Service for managing user operations with async database simulation.
/// Demonstrates async patterns and exception handling for TUnit testing.
/// </summary>
public class UserService
{
    private readonly List<User> _users;
    private int _nextId;

    /// <summary>
    /// Initializes a new instance of the UserService class.
    /// </summary>
    public UserService()
    {
        _users = new List<User>();
        _nextId = 1;
    }

    /// <summary>
    /// Creates a new user asynchronously, simulating database operations.
    /// </summary>
    /// <param name="name">The user's name.</param>
    /// <param name="email">The user's email address.</param>
    /// <returns>A ProcessingResult containing the created user or error information.</returns>
    /// <exception cref="ArgumentException">Thrown when name or email is null or empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown when a user with the same email already exists.</exception>
    public async Task<ProcessingResult<User>> CreateUserAsync(string name, string email)
    {
        var stopwatch = Stopwatch.StartNew();
        
        try
        {
            // Validate input parameters
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or empty.", nameof(name));
            
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be null or empty.", nameof(email));

            // Simulate database operation delay
            await Task.Delay(100);

            // Check if user with same email already exists
            if (_users.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException($"User with email '{email}' already exists.");
            }

            // Create new user
            var user = new User(_nextId++, name, email);
            
            // Validate the created user
            if (!user.IsValid())
            {
                stopwatch.Stop();
                return ProcessingResult<User>.CreateFailure(
                    "Created user failed validation.", 
                    stopwatch.Elapsed);
            }

            // Simulate additional database save operation
            await Task.Delay(50);
            
            _users.Add(user);
            
            stopwatch.Stop();
            return ProcessingResult<User>.CreateSuccess(user, stopwatch.Elapsed);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return ProcessingResult<User>.CreateFailure(
                $"Failed to create user: {ex.Message}", 
                stopwatch.Elapsed);
        }
    }

    /// <summary>
    /// Validates a user asynchronously, simulating database lookup operations.
    /// </summary>
    /// <param name="userId">The ID of the user to validate.</param>
    /// <returns>A ProcessingResult indicating whether the user is valid.</returns>
    /// <exception cref="ArgumentException">Thrown when userId is less than or equal to 0.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the user is not found.</exception>
    public async Task<ProcessingResult<bool>> ValidateUserAsync(int userId)
    {
        var stopwatch = Stopwatch.StartNew();
        
        try
        {
            // Validate input parameter
            if (userId <= 0)
                throw new ArgumentException("User ID must be greater than 0.", nameof(userId));

            // Simulate database lookup delay
            await Task.Delay(75);

            // Find the user
            var user = _users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }

            // Simulate validation processing delay
            await Task.Delay(25);

            var isValid = user.IsValid();
            
            stopwatch.Stop();
            return ProcessingResult<bool>.CreateSuccess(isValid, stopwatch.Elapsed);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return ProcessingResult<bool>.CreateFailure(
                $"Failed to validate user: {ex.Message}", 
                stopwatch.Elapsed);
        }
    }

    /// <summary>
    /// Updates an existing user asynchronously, simulating database update operations.
    /// </summary>
    /// <param name="userId">The ID of the user to update.</param>
    /// <param name="name">The new name for the user (optional).</param>
    /// <param name="email">The new email for the user (optional).</param>
    /// <returns>A ProcessingResult containing the updated user or error information.</returns>
    /// <exception cref="ArgumentException">Thrown when userId is invalid or email format is invalid.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the user is not found.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the new email already exists for another user.</exception>
    public async Task<ProcessingResult<User>> UpdateUserAsync(int userId, string? name = null, string? email = null)
    {
        var stopwatch = Stopwatch.StartNew();
        
        try
        {
            // Validate input parameter
            if (userId <= 0)
                throw new ArgumentException("User ID must be greater than 0.", nameof(userId));

            // Simulate database lookup delay
            await Task.Delay(100);

            // Find the user
            var user = _users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }

            // Check if new email already exists for another user
            if (!string.IsNullOrWhiteSpace(email) && 
                _users.Any(u => u.Id != userId && u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException($"Email '{email}' is already in use by another user.");
            }

            // Update user properties if provided
            if (!string.IsNullOrWhiteSpace(name))
            {
                user.Name = name;
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                user.Email = email;
            }

            // Validate the updated user
            if (!user.IsValid())
            {
                stopwatch.Stop();
                return ProcessingResult<User>.CreateFailure(
                    "Updated user failed validation.", 
                    stopwatch.Elapsed);
            }

            // Simulate database update operation
            await Task.Delay(75);
            
            stopwatch.Stop();
            return ProcessingResult<User>.CreateSuccess(user, stopwatch.Elapsed);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return ProcessingResult<User>.CreateFailure(
                $"Failed to update user: {ex.Message}", 
                stopwatch.Elapsed);
        }
    }

    /// <summary>
    /// Gets a user by ID asynchronously, simulating database lookup operations.
    /// This method is useful for testing and demonstration purposes.
    /// </summary>
    /// <param name="userId">The ID of the user to retrieve.</param>
    /// <returns>A ProcessingResult containing the user or error information.</returns>
    /// <exception cref="ArgumentException">Thrown when userId is less than or equal to 0.</exception>
    /// <exception cref="KeyNotFoundException">Thrown when the user is not found.</exception>
    public async Task<ProcessingResult<User>> GetUserAsync(int userId)
    {
        var stopwatch = Stopwatch.StartNew();
        
        try
        {
            // Validate input parameter
            if (userId <= 0)
                throw new ArgumentException("User ID must be greater than 0.", nameof(userId));

            // Simulate database lookup delay
            await Task.Delay(50);

            // Find the user
            var user = _users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }
            
            stopwatch.Stop();
            return ProcessingResult<User>.CreateSuccess(user, stopwatch.Elapsed);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return ProcessingResult<User>.CreateFailure(
                $"Failed to get user: {ex.Message}", 
                stopwatch.Elapsed);
        }
    }

    /// <summary>
    /// Gets all users asynchronously, simulating database query operations.
    /// This method is useful for testing and demonstration purposes.
    /// </summary>
    /// <returns>A ProcessingResult containing the list of all users.</returns>
    public async Task<ProcessingResult<List<User>>> GetAllUsersAsync()
    {
        var stopwatch = Stopwatch.StartNew();
        
        try
        {
            // Simulate database query delay
            await Task.Delay(150);
            
            var usersCopy = new List<User>(_users);
            
            stopwatch.Stop();
            return ProcessingResult<List<User>>.CreateSuccess(usersCopy, stopwatch.Elapsed);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return ProcessingResult<List<User>>.CreateFailure(
                $"Failed to get all users: {ex.Message}", 
                stopwatch.Elapsed);
        }
    }

    /// <summary>
    /// Gets the current count of users in the system.
    /// This method is synchronous and useful for testing purposes.
    /// </summary>
    /// <returns>The number of users currently in the system.</returns>
    public int GetUserCount()
    {
        return _users.Count;
    }

    /// <summary>
    /// Clears all users from the system.
    /// This method is useful for testing setup and cleanup.
    /// </summary>
    public void ClearAllUsers()
    {
        _users.Clear();
        _nextId = 1;
    }
}