using System;

namespace VacationRental.Core
{
    public static class Guard
    {
        public static void NotNull(object arg, string argName)
        {
            if (arg == null)
                throw new ArgumentNullException(argName);
        }
    }
}
