using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moduit.Domain.Objects
{
    public enum ErrorMessageConstant
    {
        ModuitAPI = 1,
        Database = 2
    }

    public static class ErrorMessageConstantExtensions
    {
        public static string ToName(this ErrorMessageConstant value)
        {
            switch (value)
            {
                case ErrorMessageConstant.ModuitAPI:
                    return "Moduit API";
                case ErrorMessageConstant.Database:
                    return "Database";
                default:
                    return "";
            }
        }
    }
}
