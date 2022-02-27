using System.Collections.Generic;

namespace VacationRental.Api.Helpers
{
    public static class UnitHelper
    {
        public static List<int> GetUnits(int unitCount)
        {
            List<int> units = new List<int>();

            if (unitCount <= 0)
                return units;

            for (int i = 1; i <= unitCount; i++)
            {
                units.Add(i);
            }

            return units;
        }
    }
}
