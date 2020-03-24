using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IthWeb.Services
{
    /// <summary>
    /// Created to demonstrate unit testing
    /// </summary>
    public class DummyService
    {
        /// <summary>
        /// Adds two number types and returns the sum
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="number1"></param>
        /// <param name="number2"></param>
        /// <returns>Sum of number1 + number2</returns>
        public static T Add<T>(T number1, T number2)
        {
            dynamic a = number1;
            dynamic b = number2;

            return a + b;
        }

        /// <summary>
        /// Generates a new string based on first- and lastname.
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <returns>"firstName lastName"</returns>
        public static string CalculateFullName(string firstName, string lastName)
        {
            return $"{firstName} {lastName}"; 
        }
    }
}
