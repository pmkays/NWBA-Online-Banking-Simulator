using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NWBA_Web_Application.Utilities
{
    public static class UtilityMethods
    {
        // Extracted from Matt's Tute Week 7
        public static bool HasMoreThanNDecimalPlaces(this decimal value, int n) => decimal.Round(value, n) != value;
        public static bool HasMoreThanTwoDecimalPlaces(this decimal value) => value.HasMoreThanNDecimalPlaces(2);

    }
}
