using System;
using System.IO;
using System.Reflection.Metadata.Ecma335;

namespace DoomsdayAlgorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime thisDate = new DateTime(2056, 4, 4);
            int month = thisDate.Month;
            int year = thisDate.Year - 2000;
            int day = thisDate.Day;

            double doomsday = 2; // Because 2000 is Tuesday


            double x = 38; // Year
            double y = 4;
            double z = Math.Floor(x / y);

            double w = (x + z) % 7;
            doomsday = w + 2;
            doomsday = doomsday % 7;

            //Console.WriteLine(doomsday);

            string independenceDay = IndependenceDay(doomsday);
            //Console.WriteLine(independenceDay);

            string thanksGiving = ColumbusDay(doomsday);
            Console.WriteLine(thanksGiving);

        }

        static string NewYearsDay(double doomsday, double year)
        {
            bool isLeapYear = false;
            if ((year % 4) == 0)
            {
                isLeapYear = true;
            }
            if (isLeapYear == true)
            {
                if (doomsday == 0 || doomsday == 1 || doomsday == 2)
                {
                    double nyeDOW = doomsday + 4;
                    //Console.WriteLine(nyeDOW);
                    string nyeDay = IntToDay(nyeDOW);
                    return $"New Year's Day falls on {nyeDay}";
                }
                else
                {
                    double nyeDOW = doomsday - 3;
                    //Console.WriteLine(nyeDOW);
                    string nyeDay = IntToDay(nyeDOW);
                    return $"New Year's Day falls on {nyeDay}";
                }
            }
            else
            {
                if (doomsday == 0 || doomsday == 1)
                {
                    double nyeDOW = doomsday + 5;
                    //Console.WriteLine(nyeDOW);
                    string nyeDay = IntToDay(nyeDOW);
                    return $"New Year's  falls on {nyeDay}";
                }
                else
                {
                    double nyeDOW = doomsday - 2;
                    //Console.WriteLine(nyeDOW);
                    string nyeDay = IntToDay(nyeDOW);
                    return $"New Year's  falls on {nyeDay}";
                }

            }
        }

        static string MLKDay(double doomsday, double year)
        {
            bool isLeapYear = false;
            if ((year % 4) == 0)
            {
                isLeapYear = true;
            }
            if (isLeapYear == true)
            {
                double firstDOM = (doomsday + 3) % 7;
                double MLKDate = ((8 - firstDOM) % 7) + 14;
                Console.WriteLine($"MLK Day is on January {MLKDate}");
            }
            else
            {
                double firstDOM = (doomsday + 4) % 7;
                double MLKDate = ((8 - firstDOM) % 7) + 14;
                Console.WriteLine($"MLK Day is on January {MLKDate}");
            }
            return "Done";
        }

        static string WashingtonDay(double doomsday, double year)
        {
            bool isLeapYear = false;
            if ((year % 4) == 0)
            {
                isLeapYear = true;
            }
            if (isLeapYear == true) // Doomsday is the first
            {
                double washingtonDay = ((8 - doomsday) % 7) + 15;
                Console.WriteLine($"President's Day is on February {washingtonDay}");
            }
            else
            {
                double firstDOM = (doomsday + 1) % 7;
                double washingtonDay = ((8 - firstDOM) % 7) + 15;
                Console.WriteLine($"President's Day is on February {washingtonDay}");
            }
            return "Done";
        }

        static string MemorialDay(double doomsday)
        {
            double lastDay = (doomsday + 1) % 7;
            double memDate = 31 - ((lastDay + 6) % 7);
            return $"Memorial Day falls on May {memDate}";
        }

        static string Juneteenth(double doomsday)
        {
            double jtDOW = (doomsday + 6) % 7;
            return $"Juneteenth falls on {IntToDay(jtDOW)}";
        }

        static string IndependenceDay(double doomsday)
        {
            if (doomsday == 0)
            {
                return "7/4 is a Sunday. Market is closed Monday 7/5";
            }
            else if (doomsday == 6)
            {
                return "7/4 is a Saturday. Market is closed Friday 7/3";
            }
            else if (doomsday == 5)
            {
                return "7/4 is a Friday. Market is closed Friday 7/4";
            }
            else if (doomsday == 4)
            {
                return "7/4 is a Thursday. Market is closed Thursday 7/4";
            }
            else if (doomsday == 3)
            {
                return "7/4 is a Wednesday. Market is closed Wednesday 7/4";
            }
            else if (doomsday == 2)
            {
                return "7/4 is a Tuesday. Market is closed Tuesday 7/4";
            }
            else if (doomsday == 1)
            {
                return "7/4 is a Monday. Market is closed Monday 7/4";
            }
            else
            {
                return "Invalid Day";
            }

        }

        static string LaborDay(double doomsday)
        {
            // Doomsday is the 5th
            if (doomsday == 0 || doomsday == 6)
            {
                double laborDayDate = ((8 - doomsday) % 7) + 5;
                return $"Labor Day falls on September {laborDayDate}";
            }
            else
            {
                double laborDayDate = (5 - (doomsday + 6) % 7);
                return $"Labor Day falls on September {laborDayDate}";
            }
        }

        static string ColumbusDay(double doomsday)
        {
            // October 3rd is doomsday
            if (doomsday == 0 || doomsday == 6)
            {
                double dayDistance = (8 - doomsday) % 7;
                double columbusDayDate = dayDistance + 10;
                return $"Columbus Day falls on October {columbusDayDate}";
            }
            else if (doomsday == 1 || doomsday == 2 || doomsday == 3)
            {
                double dayDistance = (doomsday + 6) % 7;
                double columbusDayDate = (7 - dayDistance) + 3;
                return $"Columbus Day falls on October {columbusDayDate}";
            }
            else
            {
                double dayDistance = (8 - doomsday) % 7;
                double columbusDayDate = (7 + dayDistance) + 3;
                return $"Columbus Day falls on October {columbusDayDate}";
            }
            return null;
        }

        static string VeteransDay(double doomsday)
        {
            double vetDay = (doomsday + 4) % 7;
            return $"Veterans Day falls on {IntToDay(vetDay)}";

        }

        static string ThanksGiving(double doomsday)
        {
            double preFirstDay = doomsday + 1;
            double firstDay = preFirstDay % 7;

            if (firstDay == 4 || firstDay == 5 || firstDay == 6)
            {
                double date = 32 - doomsday;
                Console.WriteLine($"Thanksgiving falls on Nov. {date}");
            }
            else if (firstDay == 0 || firstDay == 1 || firstDay == 2 || firstDay == 3)
            {
                double date = 25 - doomsday;
                Console.WriteLine($"Thanksgiving falls on Nov. {date}");
            }

            return "Done";
        }

        static string Christmas(double doomsday)
        {
            double xmas = ((doomsday + 7) - 1) % 7;
            string xmasDOW = IntToDay(xmas);
            return $"Christmas falls on {xmasDOW}";
        }

        static string IntToDay(double day)
        {
            if (day == 0)
            {
                return "Sunday";
            }
            else if (day == 1)
            {
                return "Monday";
            }
            else if (day == 2)
            {
                return "Tuesday";
            }
            else if (day == 3)
            {
                return "Wednesday";
            }
            else if (day == 4)
            {
                return "Thursday";
            }
            else if (day == 5)
            {
                return "Friday";
            }
            else if (day == 6)
            {
                return "Saturday";
            }
            else
            {
                return "Invalid Day";
            }
        }
    }

    
}