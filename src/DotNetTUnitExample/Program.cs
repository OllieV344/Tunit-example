using DotNetTUnitExample.Models;
using DotNetTUnitExample.Services;

namespace DotNetTUnitExample;

/// <summary>
/// Main program demonstrating the TUnit example application functionality.
/// Shows usage of all services with both synchronous and asynchronous operations,
/// including proper error handling and validation.
/// </summary>
class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("=== .NET 9 TUnit Example Application ===");
        Console.WriteLine("This application demonstrates various services that are tested with TUnit.");
        Console.WriteLine();

        // Initialize services
        var calculatorService = new CalculatorService();
        var userService = new UserService();
        var dataProcessingService = new DataProcessingService();

        // Demonstrate Calculator Service (synchronous operations)
        DemonstrateCalculatorService(calculatorService);
        
        // Demonstrate User Service (asynchronous operations)
        await DemonstrateUserService(userService);
        
        // Demonstrate Data Processing Service (both sync and async operations)
        await DemonstrateDataProcessingService(dataProcessingService);

        Console.WriteLine();
        Console.WriteLine("=== Application Complete ===");
        Console.WriteLine("All services have been demonstrated. Check the test project for comprehensive TUnit examples!");
    }

    /// <summary>
    /// Demonstrates the CalculatorService with various mathematical operations and error handling.
    /// </summary>
    /// <param name="calculator">The calculator service instance.</param>
    private static void DemonstrateCalculatorService(CalculatorService calculator)
    {
        Console.WriteLine("--- Calculator Service Demo ---");
        
        try
        {
            // Basic operations
            Console.WriteLine($"Addition: 10 + 5 = {calculator.Add(10, 5)}");
            Console.WriteLine($"Subtraction: 10 - 3 = {calculator.Subtract(10, 3)}");
            Console.WriteLine($"Multiplication: 4 * 7 = {calculator.Multiply(4, 7)}");
            Console.WriteLine($"Division: 15 / 3 = {calculator.Divide(15, 3)}");
            
            // Demonstrate error handling - division by zero
            Console.WriteLine("\nTesting error handling:");
            try
            {
                var result = calculator.Divide(10, 0);
                Console.WriteLine($"Division by zero result: {result}");
            }
            catch (DivideByZeroException ex)
            {
                Console.WriteLine($"✓ Caught expected exception: {ex.Message}");
            }
            
            // Demonstrate validation error handling
            try
            {
                var result = calculator.Add(double.NaN, 5);
                Console.WriteLine($"NaN addition result: {result}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"✓ Caught validation exception: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Unexpected error in calculator demo: {ex.Message}");
        }
        
        Console.WriteLine();
    }

    /// <summary>
    /// Demonstrates the UserService with asynchronous operations and error handling.
    /// </summary>
    /// <param name="userService">The user service instance.</param>
    private static async Task DemonstrateUserService(UserService userService)
    {
        Console.WriteLine("--- User Service Demo (Async Operations) ---");
        
        try
        {
            // Create users successfully
            Console.WriteLine("Creating users...");
            var user1Result = await userService.CreateUserAsync("John Doe", "john.doe@example.com");
            if (user1Result.Success)
            {
                Console.WriteLine($"✓ Created user: {user1Result.Data} (Processing time: {user1Result.ProcessingTime.TotalMilliseconds}ms)");
            }
            else
            {
                Console.WriteLine($"❌ Failed to create user: {user1Result.ErrorMessage}");
            }

            var user2Result = await userService.CreateUserAsync("Jane Smith", "jane.smith@example.com");
            if (user2Result.Success)
            {
                Console.WriteLine($"✓ Created user: {user2Result.Data} (Processing time: {user2Result.ProcessingTime.TotalMilliseconds}ms)");
            }

            // Demonstrate validation of created users
            if (user1Result.Success)
            {
                var validationResult = await userService.ValidateUserAsync(user1Result.Data!.Id);
                Console.WriteLine($"User validation result: {validationResult.Success} (Processing time: {validationResult.ProcessingTime.TotalMilliseconds}ms)");
            }

            // Demonstrate user update
            if (user2Result.Success)
            {
                var updateResult = await userService.UpdateUserAsync(user2Result.Data!.Id, "Jane Johnson", null);
                if (updateResult.Success)
                {
                    Console.WriteLine($"✓ Updated user: {updateResult.Data} (Processing time: {updateResult.ProcessingTime.TotalMilliseconds}ms)");
                }
            }

            // Get all users
            var allUsersResult = await userService.GetAllUsersAsync();
            if (allUsersResult.Success)
            {
                Console.WriteLine($"Total users in system: {allUsersResult.Data!.Count} (Processing time: {allUsersResult.ProcessingTime.TotalMilliseconds}ms)");
            }

            // Demonstrate error handling - duplicate email
            Console.WriteLine("\nTesting error handling:");
            var duplicateResult = await userService.CreateUserAsync("Another User", "john.doe@example.com");
            if (!duplicateResult.Success)
            {
                Console.WriteLine($"✓ Caught expected error: {duplicateResult.ErrorMessage}");
            }

            // Demonstrate error handling - invalid user ID
            var invalidUserResult = await userService.ValidateUserAsync(-1);
            if (!invalidUserResult.Success)
            {
                Console.WriteLine($"✓ Caught validation error: {invalidUserResult.ErrorMessage}");
            }

            // Demonstrate error handling - user not found
            var notFoundResult = await userService.GetUserAsync(999);
            if (!notFoundResult.Success)
            {
                Console.WriteLine($"✓ Caught not found error: {notFoundResult.ErrorMessage}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Unexpected error in user service demo: {ex.Message}");
        }
        
        Console.WriteLine();
    }

    /// <summary>
    /// Demonstrates the DataProcessingService with both synchronous and asynchronous operations.
    /// </summary>
    /// <param name="dataService">The data processing service instance.</param>
    private static async Task DemonstrateDataProcessingService(DataProcessingService dataService)
    {
        Console.WriteLine("--- Data Processing Service Demo (Sync & Async Operations) ---");
        
        try
        {
            // Synchronous operations
            Console.WriteLine("Synchronous operations:");
            
            var numbers = new[] { 1, 2, 3, 4, 5 };
            var processResult = dataService.ProcessNumbers(numbers, 2);
            if (processResult.Success)
            {
                Console.WriteLine($"✓ Processed numbers: [{string.Join(", ", processResult.Data!)}] (Processing time: {processResult.ProcessingTime.TotalMilliseconds}ms)");
            }

            var strings = new[] { "hello", "world", "tunit", "testing" };
            var stringResult = dataService.ProcessStrings(strings, true);
            if (stringResult.Success)
            {
                Console.WriteLine($"✓ Processed strings: [{string.Join(", ", stringResult.Data!)}] (Processing time: {stringResult.ProcessingTime.TotalMilliseconds}ms)");
            }

            var filterResult = dataService.FilterNumbers(numbers, 3);
            if (filterResult.Success)
            {
                Console.WriteLine($"✓ Filtered numbers (>= 3): [{string.Join(", ", filterResult.Data!)}] (Processing time: {filterResult.ProcessingTime.TotalMilliseconds}ms)");
            }

            // Asynchronous operations
            Console.WriteLine("\nAsynchronous operations:");
            
            var asyncNumbers = new[] { 10, 20, 30, 40, 50 };
            var asyncProcessResult = await dataService.ProcessNumbersAsync(asyncNumbers, 3, 50);
            if (asyncProcessResult.Success)
            {
                Console.WriteLine($"✓ Async processed numbers: [{string.Join(", ", asyncProcessResult.Data!)}] (Processing time: {asyncProcessResult.ProcessingTime.TotalMilliseconds}ms)");
            }

            var asyncStrings = new[] { "async", "testing", "with", "tunit", "framework", "example" };
            var batchResult = await dataService.ProcessStringsBatchAsync(asyncStrings, 3, false);
            if (batchResult.Success)
            {
                Console.WriteLine($"✓ Batch processed strings: [{string.Join(", ", batchResult.Data!)}] (Processing time: {batchResult.ProcessingTime.TotalMilliseconds}ms)");
            }

            var aggregateNumbers = new[] { 5, 10, 15, 20, 25 };
            var sumResult = await dataService.AggregateNumbersAsync(aggregateNumbers, "sum");
            if (sumResult.Success)
            {
                Console.WriteLine($"✓ Sum aggregation: {sumResult.Data} (Processing time: {sumResult.ProcessingTime.TotalMilliseconds}ms)");
            }

            var avgResult = await dataService.AggregateNumbersAsync(aggregateNumbers, "average");
            if (avgResult.Success)
            {
                Console.WriteLine($"✓ Average aggregation: {avgResult.Data:F2} (Processing time: {avgResult.ProcessingTime.TotalMilliseconds}ms)");
            }

            // Demonstrate error handling
            Console.WriteLine("\nTesting error handling:");
            
            // Synchronous error handling
            var nullResult = dataService.ProcessNumbers(null!, 2);
            if (!nullResult.Success)
            {
                Console.WriteLine($"✓ Caught sync error: {nullResult.ErrorMessage}");
            }

            // Asynchronous error handling
            var asyncNullResult = await dataService.ProcessNumbersAsync(null!, 2);
            if (!asyncNullResult.Success)
            {
                Console.WriteLine($"✓ Caught async error: {asyncNullResult.ErrorMessage}");
            }

            // Simulated failure
            var failureResult = dataService.ProcessWithPotentialFailure(new[] { 1, 2, 3 }, true, "Simulated processing failure for demo");
            if (!failureResult.Success)
            {
                Console.WriteLine($"✓ Caught simulated error: {failureResult.ErrorMessage}");
            }

            var asyncFailureResult = await dataService.ProcessWithPotentialFailureAsync(new[] { 1, 2, 3 }, true, "Simulated async failure for demo", 25);
            if (!asyncFailureResult.Success)
            {
                Console.WriteLine($"✓ Caught simulated async error: {asyncFailureResult.ErrorMessage}");
            }

            // Invalid operation error
            var invalidOpResult = await dataService.AggregateNumbersAsync(new[] { 1, 2, 3 }, "invalid");
            if (!invalidOpResult.Success)
            {
                Console.WriteLine($"✓ Caught invalid operation error: {invalidOpResult.ErrorMessage}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Unexpected error in data processing demo: {ex.Message}");
        }
        
        Console.WriteLine();
    }
}
