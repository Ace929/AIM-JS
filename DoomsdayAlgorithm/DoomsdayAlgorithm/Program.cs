using System;
using System.ComponentModel;
using System.IO;
using System.Reflection.Metadata.Ecma335;

namespace DoomsdayAlgorithm
{
    class Program
    {
        static void Main(string[] args)
        { // Markets dont close on Veterans day and Columbus day
            //But, they do close on good friday
            int year = 2022;

            var holidays = GetAllMarketHolidays(year);
            
            foreach (var item in holidays)
            {
                Console.WriteLine(item);
            }
            
        }

        static (int date, int dow) NewYearsDay(int doomsday, int year)
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
                    int nyeDOW = doomsday + 4;
                    //Console.WriteLine(nyeDOW);
                    string nyeDay = IntToDay(nyeDOW);
                    return (1, nyeDOW);
                }
                else
                {
                    int nyeDOW = doomsday - 3;
                    //Console.WriteLine(nyeDOW);
                    string nyeDay = IntToDay(nyeDOW);
                    return (1, nyeDOW);
                }
            }
            else
            {
                if (doomsday == 0 || doomsday == 1)
                {
                    int nyeDOW = doomsday + 5;
                    //Console.WriteLine(nyeDOW);
                    string nyeDay = IntToDay(nyeDOW);
                    return (1, nyeDOW);
                }
                else
                {
                    int nyeDOW = doomsday - 2;
                    //Console.WriteLine(nyeDOW);
                    string nyeDay = IntToDay(nyeDOW);
                    return (1, nyeDOW);
                }

            }
        }

        static (int date, int dow) MLKDay(int doomsday, int year)
        {
            bool isLeapYear = false;
            if ((year % 4) == 0)
            {
                isLeapYear = true;
            }
            if (isLeapYear == true)
            {
                int firstDOM = (doomsday + 3) % 7;
                int MLKDate = ((8 - firstDOM) % 7) + 14;
                return (MLKDate, 1);
            }
            else
            {
                int firstDOM = (doomsday + 4) % 7;
                int MLKDate = ((8 - firstDOM) % 7) + 14;
                return (MLKDate, 1);
            }
        }

        static (int date, int dow) PresidentsDay(int doomsday, int year)
        {
            bool isLeapYear = false;
            if ((year % 4) == 0)
            {
                isLeapYear = true;
            }
            if (isLeapYear == true) // Doomsday is the first
            {
                int washingtonDay = ((8 - doomsday) % 7) + 15;
                return (washingtonDay, 1);
            }
            else
            {
                int firstDOM = (doomsday + 1) % 7;
                int washingtonDay = ((8 - firstDOM) % 7) + 15;
                return (washingtonDay, 1);
            }
        }

        static (DateTime date, int dow) GoodFriday(int year)
        {
            int y = year;
            int c = y / 100;
            int n = y - 19 * (y / 19);
            int k = (c - 17) / 25;
            int i = c - c / 4 - (c - k) / 3 + 19 * n + 15;
            i = i - 30 * (i / 30);
            i = i - (i / 28) * (1 - (i / 28) * (29 / (i + 1)) * ((21 - n) / 11));
            int j = y + y / 4 + i + 2 - c + c / 4;
            j = j - 7 * (j / 7);
            int l = i - j;
            int month = 3 + (l + 40) / 44;
            int day = l + 28 - 31 * (month / 4);
            DateTime goodFriday = new DateTime(year, month, day - 2);
            return (goodFriday,5);
        }

        static (int date, int dow) MemorialDay(int doomsday)
        {
            int lastDay = (doomsday + 1) % 7;
            int memDate = 31 - ((lastDay + 6) % 7);
            return (memDate, 1);
        }

        static (int date, int dow) Juneteenth(int doomsday)
        {
            int jtDOW = (doomsday + 6) % 7;
            return (19, jtDOW);
        }

        static (int date, int dow) IndependenceDay(int doomsday)
        {
            return (4, doomsday);
        }

        static (int date, int dow) LaborDay(int doomsday)
        {
            // Doomsday is the 5th
            if (doomsday == 0 || doomsday == 6)
            {
                int laborDayDate = ((8 - doomsday) % 7) + 5;
                return (laborDayDate, 1);
            }
            else
            {
                int laborDayDate = (5 - (doomsday + 6) % 7);
                return (laborDayDate, 1);
            }
        }

        static (int date, int dow) ColumbusDay(int doomsday)
        {
            // October 3rd is doomsday
            if (doomsday == 0 || doomsday == 6)
            {
                int dayDistance = (8 - doomsday) % 7;
                int columbusDayDate = dayDistance + 10;
                return (columbusDayDate, 1);
            }
            else if (doomsday == 1 || doomsday == 2 || doomsday == 3)
            {
                int dayDistance = (doomsday + 6) % 7;
                int columbusDayDate = (7 - dayDistance) + 3;
                return (columbusDayDate, 1);
            }
            else
            {
                int dayDistance = (8 - doomsday) % 7;
                int columbusDayDate = (7 + dayDistance) + 3;
                return (columbusDayDate, 1);
            }
        }

        static (int date, int dow) VeteransDay(int doomsday)
        {
            int vetDay = (doomsday + 4) % 7;
            return (11, vetDay);

        }

        static (int date, int dow) ThanksGiving(int doomsday)
        {
            int preFirstDay = doomsday + 1;
            int firstDay = preFirstDay % 7;

            if (firstDay == 4 || firstDay == 5 || firstDay == 6)
            {
                int date = 32 - doomsday;
                return (date, 4);
            }
            else
            {
                int date = 25 - doomsday;
                return (date, 4);
            }
        }

        static (int date, int dow) Christmas(int doomsday)
        {
            int xmas = ((doomsday + 7) - 1) % 7;
            //string xmasDOW = IntToDay(xmas);
            return (25, xmas);
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

        static List<(DateTime date, int dow)> GetAllMarketHolidays(int year)
        {
            year = year - 2000;
            int doomsday = 2; // Because 2000 is Tuesday (Only initial doomsday [For the century])
            int y = 4;
            int z = (int)Math.Floor((double)year / y);
            int w = (year + z) % 7;
            doomsday = w + 2;
            doomsday = doomsday % 7;

            // This is for next year, which we will use for next year's New Years Day
            int _year = year + 1;
            int _doomsday = 2;
            int _z = (int)Math.Floor((double)_year / y);
            int _w = (int)(_year + _z) % 7;
            _doomsday = _w + 2;
            _doomsday = _doomsday % 7;

            int formattedYear = year + 2000;

            var nye = NewYearsDay(doomsday, year); // can be weekend (SOLVED)
            var nyePlusOne = NewYearsDay(_doomsday, _year);
            var mlk = MLKDay(doomsday, year);
            var pres = PresidentsDay(doomsday, year);
            var good = GoodFriday(formattedYear);
            var mem = MemorialDay(doomsday);
            var jun = Juneteenth(doomsday); // can be weekend (SOLVED)
            var ind = IndependenceDay(doomsday); // can be weekend (SOLVED)
            var lab = LaborDay(doomsday);
            var col = ColumbusDay(doomsday);
            var vet = VeteransDay(doomsday); // can be weekend (SOLVED)
            var thank = ThanksGiving(doomsday);
            var xmas = Christmas(doomsday); // can be weekend (SOLVED)
            //Console.WriteLine(xmas.Item1);

            List<(DateTime date, int dow)> finalClosures = new List<(DateTime date, int dow)>();

            // Check new years day
            if (nyePlusOne.dow == 6)
            {
                DateTime newYearsPlusOne = new DateTime(formattedYear, 12, 31);
                finalClosures.Add((newYearsPlusOne, 5));
            }
            else if (nye.dow == 0)
            {
                DateTime newYearsDay = new DateTime(formattedYear, 1, 2);
                finalClosures.Add((newYearsDay, 1));
            }
            else if (nye.dow != 6)
            {
                DateTime newYearsDay = new DateTime(formattedYear, 1, 1);
                finalClosures.Add((newYearsDay, nye.dow));
            }

            finalClosures.Add((new DateTime(formattedYear, 1, mlk.date), 1)); // Add MLK Day
            finalClosures.Add((new DateTime(formattedYear, 2, pres.date), 1)); // Add Presidents Day
            finalClosures.Add((good.date, good.dow)); // Add Good Friday
            finalClosures.Add((new DateTime(formattedYear, 5, mem.date), 1)); // Add Memorial Day

            if (jun.dow == 0)
            {
                DateTime juneteenth = new DateTime(formattedYear, 6, 20);
                finalClosures.Add((juneteenth, 1));
            }
            else if (jun.dow == 6)
            {
                DateTime juneteenth = new DateTime(formattedYear, 6, 18);
                finalClosures.Add((juneteenth, 5));
            }
            else
            {
                DateTime juneteenth = new DateTime(formattedYear, 6, 19);
                finalClosures.Add((juneteenth, jun.dow));
            }
            if (ind.dow == 0)
            {
                DateTime independenceDay = new DateTime(formattedYear, 7, 5);
                finalClosures.Add((independenceDay, 1));
            }
            else if (ind.dow == 6)
            {
                DateTime independenceDay = new DateTime(formattedYear, 7, 3);
                finalClosures.Add((independenceDay, 5));
            }
            else
            {
                DateTime independenceDay = new DateTime(formattedYear, 7, 4);
                finalClosures.Add((independenceDay, ind.dow));
            }

            finalClosures.Add((new DateTime(formattedYear, 9, lab.date), 1)); // Add Labor Day
            //finalClosures.Add((new DateTime(formattedYear, 10, col.date), 1)); // Add Columbus Day

            // This is Veteran's Day
            /*
            if (vet.dow == 0)
            {
                DateTime veteransDay = new DateTime(formattedYear, 11, 12);
                finalClosures.Add((veteransDay, 1));
            }
            else if (vet.dow == 6)
            {
                DateTime veteransDay = new DateTime(formattedYear, 11, 10);
                finalClosures.Add((veteransDay, 5));
            }
            else
            {
                DateTime veteransDay = new DateTime(formattedYear, 11, 11);
                finalClosures.Add((veteransDay, vet.dow));
            }
            */

            finalClosures.Add((new DateTime(formattedYear, 11, thank.date), 4)); // Add Thanksgiving

            if (xmas.dow == 0)
            {
                DateTime christmas = new DateTime(formattedYear, 12, 26);
                finalClosures.Add((christmas, 1));
            }
            else if (xmas.dow == 6)
            {
                DateTime christmas = new DateTime(formattedYear, 12, 24);
                finalClosures.Add((christmas, 5));
            }
            else
            {
                DateTime christmas = new DateTime(formattedYear, 12, 25);
                finalClosures.Add((christmas, xmas.dow));
            }

            return finalClosures; // This is a list of market closures because of holidays
        }
    }
}