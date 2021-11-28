using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moduit.Utility.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Appends a url path with an additional url path.
        /// </summary>
        /// <param name="value">Path to be appended.</param>
        /// <param name="additionalPath">Additional path that will be appended to value.</param>
        /// <returns>a string representation of all path.</returns>
        /// <exception cref="System.ArgumentNullException">value, or additionalPath is null.</exception>
        public static string AppendUrlPath(this string value, string additionalPath)
        {
            return value.AppendPath("/", additionalPath);
        }


        /// <summary>
        /// Appends a path with an additional path.
        /// </summary>
        /// <param name="value">Path to be appended.</param>
        /// <param name="pathSeparator">Separator between path elements</param>
        /// <param name="additionalPath">Additional path that will be appended to value.</param>
        /// <returns>a string representation of all path.</returns>
        /// <exception cref="System.ArgumentNullException">value, or pathSeparator, or additionalPath is null.</exception>
        public static string AppendPath(this string value, string pathSeparator, string additionalPath)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            if (pathSeparator == null)
            {
                throw new ArgumentNullException(nameof(pathSeparator));
            }
            if (additionalPath == null)
            {
                throw new ArgumentNullException(nameof(additionalPath));
            }

            string firstPart = value;
            string lastPart = additionalPath;

            if (firstPart.Right(pathSeparator.Length) == pathSeparator)
            {
                firstPart = firstPart.Left(firstPart.Length - pathSeparator.Length);
            }

            if (lastPart.Left(pathSeparator.Length) == pathSeparator)
            {
                lastPart = lastPart.Right(lastPart.Length - pathSeparator.Length);
            }

            return firstPart + pathSeparator + lastPart;
        }

        /// <summary>
        /// Returns a string containing a specified number of characters from the left side of a string.
        /// </summary>
        /// <param name="value">String expression from which the left-most characters are returned.</param>
        /// <param name="length">The number of characters in the substring.</param>
        /// <returns>A string containing a specified number of characters from the left side of a string.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">length is negative.</exception>
        public static string Left(this string value, int length)
        {
            string result = value;
            if (value == null)
            {
                return null;
            }
            else
            {
                if (length < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(length));
                }
                if (result.Length > length)
                {
                    result = result.Substring(0, length);
                }
            }
            return result;
        }

        /// <summary>
        /// Returns a string containing a specified number of characters from the right side of a string.
        /// </summary>
        /// <param name="value">String expression from which the right-most characters are returned.</param>
        /// <param name="length">The number of characters in the substring.</param>
        /// <returns>>A string containing a specified number of characters from the right side of a string.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">length is negative.</exception>
        public static string Right(this string value, int length)
        {
            string result = value;
            if (value == null)
            {
                return null;
            }
            else
            {
                if (length < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(length));
                }
                if (result.Length > length)
                {
                    result = result.Substring(result.Length - length, length);
                }
            }
            return result;
        }


    }
}
