using DotNetTUnitExample.Models;
using System.Diagnostics;

namespace DotNetTUnitExample.Services;

/// <summary>
/// Service for processing collections of data with both synchronous and asynchronous operations.
/// Provides various processing scenarios for testing purposes.
/// </summary>
public class DataProcessingService
{
    /// <summary>
    /// Processes a collection of integers synchronously by applying a transformation.
    /// </summary>
    /// <param name="numbers">The collection of numbers to process.</param>
    /// <param name="multiplier">The multiplier to apply to each number.</param>
    /// <returns>A ProcessingResult containing the processed numbers.</returns>
    public ProcessingResult<List<int>> ProcessNumbers(IEnumerable<int> numbers, int multiplier)
    {
        var stopwatch = Stopwatch.StartNew();
        
        try
        {
            if (numbers == null)
            {
                return ProcessingResult<List<int>>.CreateFailure("Input numbers cannot be null", stopwatch.Elapsed);
            }

            var numbersList = numbers.ToList();
            if (!numbersList.Any())
            {
                return ProcessingResult<List<int>>.CreateSuccess(new List<int>(), stopwatch.Elapsed);
            }

            var processedNumbers = numbersList.Select(n => n * multiplier).ToList();
            stopwatch.Stop();
            
            return ProcessingResult<List<int>>.CreateSuccess(processedNumbers, stopwatch.Elapsed);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return ProcessingResult<List<int>>.CreateFailure($"Error processing numbers: {ex.Message}", stopwatch.Elapsed);
        }
    }

    /// <summary>
    /// Processes a collection of strings synchronously by applying transformations.
    /// </summary>
    /// <param name="strings">The collection of strings to process.</param>
    /// <param name="toUpperCase">Whether to convert strings to uppercase.</param>
    /// <returns>A ProcessingResult containing the processed strings.</returns>
    public ProcessingResult<List<string>> ProcessStrings(IEnumerable<string> strings, bool toUpperCase = true)
    {
        var stopwatch = Stopwatch.StartNew();
        
        try
        {
            if (strings == null)
            {
                return ProcessingResult<List<string>>.CreateFailure("Input strings cannot be null", stopwatch.Elapsed);
            }

            var stringsList = strings.ToList();
            var processedStrings = new List<string>();

            foreach (var str in stringsList)
            {
                if (string.IsNullOrEmpty(str))
                {
                    processedStrings.Add(string.Empty);
                }
                else
                {
                    processedStrings.Add(toUpperCase ? str.ToUpper() : str.ToLower());
                }
            }

            stopwatch.Stop();
            return ProcessingResult<List<string>>.CreateSuccess(processedStrings, stopwatch.Elapsed);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return ProcessingResult<List<string>>.CreateFailure($"Error processing strings: {ex.Message}", stopwatch.Elapsed);
        }
    }

    /// <summary>
    /// Filters a collection of numbers synchronously based on a predicate.
    /// </summary>
    /// <param name="numbers">The collection of numbers to filter.</param>
    /// <param name="minValue">The minimum value to include in the result.</param>
    /// <returns>A ProcessingResult containing the filtered numbers.</returns>
    public ProcessingResult<List<int>> FilterNumbers(IEnumerable<int> numbers, int minValue)
    {
        var stopwatch = Stopwatch.StartNew();
        
        try
        {
            if (numbers == null)
            {
                return ProcessingResult<List<int>>.CreateFailure("Input numbers cannot be null", stopwatch.Elapsed);
            }

            var filteredNumbers = numbers.Where(n => n >= minValue).ToList();
            stopwatch.Stop();
            
            return ProcessingResult<List<int>>.CreateSuccess(filteredNumbers, stopwatch.Elapsed);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return ProcessingResult<List<int>>.CreateFailure($"Error filtering numbers: {ex.Message}", stopwatch.Elapsed);
        }
    }

    /// <summary>
    /// Processes a collection of integers asynchronously by applying a transformation.
    /// Simulates database or external service operations with delays.
    /// </summary>
    /// <param name="numbers">The collection of numbers to process.</param>
    /// <param name="multiplier">The multiplier to apply to each number.</param>
    /// <param name="delayMs">The delay in milliseconds to simulate processing time.</param>
    /// <returns>A Task containing a ProcessingResult with the processed numbers.</returns>
    public async Task<ProcessingResult<List<int>>> ProcessNumbersAsync(IEnumerable<int> numbers, int multiplier, int delayMs = 100)
    {
        var stopwatch = Stopwatch.StartNew();
        
        try
        {
            if (numbers == null)
            {
                return ProcessingResult<List<int>>.CreateFailure("Input numbers cannot be null", stopwatch.Elapsed);
            }

            var numbersList = numbers.ToList();
            if (!numbersList.Any())
            {
                return ProcessingResult<List<int>>.CreateSuccess(new List<int>(), stopwatch.Elapsed);
            }

            // Simulate async processing delay
            await Task.Delay(delayMs);

            var processedNumbers = numbersList.Select(n => n * multiplier).ToList();
            stopwatch.Stop();
            
            return ProcessingResult<List<int>>.CreateSuccess(processedNumbers, stopwatch.Elapsed);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return ProcessingResult<List<int>>.CreateFailure($"Error processing numbers asynchronously: {ex.Message}", stopwatch.Elapsed);
        }
    }

    /// <summary>
    /// Processes a collection of strings asynchronously with batch processing simulation.
    /// </summary>
    /// <param name="strings">The collection of strings to process.</param>
    /// <param name="batchSize">The size of each processing batch.</param>
    /// <param name="toUpperCase">Whether to convert strings to uppercase.</param>
    /// <returns>A Task containing a ProcessingResult with the processed strings.</returns>
    public async Task<ProcessingResult<List<string>>> ProcessStringsBatchAsync(IEnumerable<string> strings, int batchSize = 5, bool toUpperCase = true)
    {
        var stopwatch = Stopwatch.StartNew();
        
        try
        {
            if (strings == null)
            {
                return ProcessingResult<List<string>>.CreateFailure("Input strings cannot be null", stopwatch.Elapsed);
            }

            if (batchSize <= 0)
            {
                return ProcessingResult<List<string>>.CreateFailure("Batch size must be greater than zero", stopwatch.Elapsed);
            }

            var stringsList = strings.ToList();
            var processedStrings = new List<string>();

            // Process in batches
            for (int i = 0; i < stringsList.Count; i += batchSize)
            {
                var batch = stringsList.Skip(i).Take(batchSize);
                
                // Simulate async batch processing
                await Task.Delay(50);
                
                foreach (var str in batch)
                {
                    if (string.IsNullOrEmpty(str))
                    {
                        processedStrings.Add(string.Empty);
                    }
                    else
                    {
                        processedStrings.Add(toUpperCase ? str.ToUpper() : str.ToLower());
                    }
                }
            }

            stopwatch.Stop();
            return ProcessingResult<List<string>>.CreateSuccess(processedStrings, stopwatch.Elapsed);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return ProcessingResult<List<string>>.CreateFailure($"Error processing strings in batches: {ex.Message}", stopwatch.Elapsed);
        }
    }

    /// <summary>
    /// Aggregates a collection of numbers asynchronously with various statistical operations.
    /// </summary>
    /// <param name="numbers">The collection of numbers to aggregate.</param>
    /// <param name="operation">The aggregation operation to perform (sum, average, max, min).</param>
    /// <returns>A Task containing a ProcessingResult with the aggregated result.</returns>
    public async Task<ProcessingResult<double>> AggregateNumbersAsync(IEnumerable<int> numbers, string operation)
    {
        var stopwatch = Stopwatch.StartNew();
        
        try
        {
            if (numbers == null)
            {
                return ProcessingResult<double>.CreateFailure("Input numbers cannot be null", stopwatch.Elapsed);
            }

            var numbersList = numbers.ToList();
            if (!numbersList.Any())
            {
                return ProcessingResult<double>.CreateFailure("Cannot aggregate empty collection", stopwatch.Elapsed);
            }

            // Simulate async processing
            await Task.Delay(75);

            double result = operation?.ToLower() switch
            {
                "sum" => numbersList.Sum(),
                "average" => numbersList.Average(),
                "max" => numbersList.Max(),
                "min" => numbersList.Min(),
                _ => throw new ArgumentException($"Unknown operation: {operation}")
            };

            stopwatch.Stop();
            return ProcessingResult<double>.CreateSuccess(result, stopwatch.Elapsed);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return ProcessingResult<double>.CreateFailure($"Error aggregating numbers: {ex.Message}", stopwatch.Elapsed);
        }
    }

    /// <summary>
    /// Processes data with potential failure scenarios for testing error handling.
    /// </summary>
    /// <param name="data">The collection of data to process.</param>
    /// <param name="shouldFail">Whether the operation should simulate a failure.</param>
    /// <param name="failureMessage">The failure message to use if shouldFail is true.</param>
    /// <returns>A ProcessingResult indicating success or failure.</returns>
    public ProcessingResult<List<T>> ProcessWithPotentialFailure<T>(IEnumerable<T> data, bool shouldFail = false, string failureMessage = "Simulated processing failure")
    {
        var stopwatch = Stopwatch.StartNew();
        
        try
        {
            if (shouldFail)
            {
                return ProcessingResult<List<T>>.CreateFailure(failureMessage, stopwatch.Elapsed);
            }

            if (data == null)
            {
                return ProcessingResult<List<T>>.CreateFailure("Input data cannot be null", stopwatch.Elapsed);
            }

            var result = data.ToList();
            stopwatch.Stop();
            
            return ProcessingResult<List<T>>.CreateSuccess(result, stopwatch.Elapsed);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return ProcessingResult<List<T>>.CreateFailure($"Unexpected error: {ex.Message}", stopwatch.Elapsed);
        }
    }

    /// <summary>
    /// Processes data asynchronously with potential failure scenarios for testing error handling.
    /// </summary>
    /// <param name="data">The collection of data to process.</param>
    /// <param name="shouldFail">Whether the operation should simulate a failure.</param>
    /// <param name="failureMessage">The failure message to use if shouldFail is true.</param>
    /// <param name="delayMs">The delay in milliseconds before processing.</param>
    /// <returns>A Task containing a ProcessingResult indicating success or failure.</returns>
    public async Task<ProcessingResult<List<T>>> ProcessWithPotentialFailureAsync<T>(IEnumerable<T> data, bool shouldFail = false, string failureMessage = "Simulated async processing failure", int delayMs = 100)
    {
        var stopwatch = Stopwatch.StartNew();
        
        try
        {
            await Task.Delay(delayMs);

            if (shouldFail)
            {
                return ProcessingResult<List<T>>.CreateFailure(failureMessage, stopwatch.Elapsed);
            }

            if (data == null)
            {
                return ProcessingResult<List<T>>.CreateFailure("Input data cannot be null", stopwatch.Elapsed);
            }

            var result = data.ToList();
            stopwatch.Stop();
            
            return ProcessingResult<List<T>>.CreateSuccess(result, stopwatch.Elapsed);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return ProcessingResult<List<T>>.CreateFailure($"Unexpected async error: {ex.Message}", stopwatch.Elapsed);
        }
    }
}