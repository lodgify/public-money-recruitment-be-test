using System;
using System.Collections.Generic;
using System.Linq;

namespace VacationRental.Api.Utilities
{
    public class BookingHandler
    {
        public static int CheckBookings(List<DateTime> arrival, List<DateTime> departure, int units)
        {
            int count = arrival.Count;

            if (count == 1 && units > 0)
            {
                return 1;
            }

            Pair[] temp = new Pair[2 * count];

            int j = 0;
            for (int i = 0; i < count; i++)
            {
                temp[i + j] = new Pair(int.Parse(arrival[i].ToString("yyyyMMdd")), 1);
                temp[i + j + 1] = new Pair(int.Parse(departure[i].ToString("yyyyMMdd")), 0);
                j++;

            }

            Sort.Comparer(temp, 2 * count);

            int currentBookings = 0, maxBookings = 0;
            for (int i = 0; i < 2 * count; i++)
            {
                if (temp[i].y == 1)
                {
                    currentBookings++;
                    maxBookings = Math.Max(maxBookings, currentBookings);
                }
                else
                    currentBookings--;
            }

            return maxBookings;
        }
    }
    public class Pair
    {
        public int x;
        public int y;

        public Pair(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
    class Comparison : Comparer<Pair>
    {
        public override int Compare(Pair p1, Pair p2)
        {
            return p1.x - p2.x;
        }
    }
    public class Sort
    {
        public static void Comparer(Pair[] arr, int n)
        {
            IComparer<Pair> compare = new Comparison();
            Array.Sort(arr, compare);
        }
    }
}
