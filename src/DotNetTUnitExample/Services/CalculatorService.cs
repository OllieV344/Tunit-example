using System;

namespace DotNetTUnitExample.Services
{
    /// <summary>
    /// Service providing basic mathematical operations with proper error handling and validation.
    /// Demonstrates testable business logic for TUnit testing examples.
    /// </summary>
    public class CalculatorService
    {
        /// <summary>
        /// Adds two numbers together.
        /// </summary>
        /// <param name="a">First number</param>
        /// <param name="b">Second number</param>
        /// <returns>Sum of the two numbers</returns>
        /// <exception cref="ArgumentException">Thrown when inputs are invalid (NaN or Infinity)</exception>
        public double Add(double a, double b)
        {
            ValidateInputs(a, b);
            
            var result = a + b;
            ValidateResult(result);
            
            return result;
        }

        /// <summary>
        /// Subtracts the second number from the first number.
        /// </summary>
        /// <param name="a">Number to subtract from</param>
        /// <param name="b">Number to subtract</param>
        /// <returns>Difference of the two numbers</returns>
        /// <exception cref="ArgumentException">Thrown when inputs are invalid (NaN or Infinity)</exception>
        public double Subtract(double a, double b)
        {
            ValidateInputs(a, b);
            
            var result = a - b;
            ValidateResult(result);
            
            return result;
        }

        /// <summary>
        /// Multiplies two numbers together.
        /// </summary>
        /// <param name="a">First number</param>
        /// <param name="b">Second number</param>
        /// <returns>Product of the two numbers</returns>
        /// <exception cref="ArgumentException">Thrown when inputs are invalid (NaN or Infinity)</exception>
        public double Multiply(double a, double b)
        {
            ValidateInputs(a, b);
            
            var result = a * b;
            ValidateResult(result);
            
            return result;
        }

        /// <summary>
        /// Divides the first number by the second number.
        /// </summary>
        /// <param name="a">Dividend</param>
        /// <param name="b">Divisor</param>
        /// <returns>Quotient of the division</returns>
        /// <exception cref="ArgumentException">Thrown when inputs are invalid (NaN or Infinity)</exception>
        /// <exception cref="DivideByZeroException">Thrown when attempting to divide by zero</exception>
        public double Divide(double a, double b)
        {
            ValidateInputs(a, b);
            
            if (b == 0)
            {
                throw new DivideByZeroException("Cannot divide by zero");
            }
            
            var result = a / b;
            ValidateResult(result);
            
            return result;
        }

        /// <summary>
        /// Validates that input numbers are valid (not NaN or Infinity).
        /// </summary>
        /// <param name="a">First number to validate</param>
        /// <param name="b">Second number to validate</param>
        /// <exception cref="ArgumentException">Thrown when any input is NaN or Infinity</exception>
        private void ValidateInputs(double a, double b)
        {
            if (double.IsNaN(a) || double.IsInfinity(a))
            {
                throw new ArgumentException($"First parameter is invalid: {a}", nameof(a));
            }
            
            if (double.IsNaN(b) || double.IsInfinity(b))
            {
                throw new ArgumentException($"Second parameter is invalid: {b}", nameof(b));
            }
        }

        /// <summary>
        /// Validates that the result of an operation is valid (not NaN or Infinity).
        /// </summary>
        /// <param name="result">Result to validate</param>
        /// <exception cref="InvalidOperationException">Thrown when result is NaN or Infinity</exception>
        private void ValidateResult(double result)
        {
            if (double.IsNaN(result))
            {
                throw new InvalidOperationException("Operation resulted in NaN (Not a Number)");
            }
            
            if (double.IsInfinity(result))
            {
                throw new InvalidOperationException("Operation resulted in Infinity");
            }
        }
    }
}