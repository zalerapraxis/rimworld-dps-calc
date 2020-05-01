using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rimworld_dps_calc
{
    class Program
    {
        static List<string> PreviousEntries = new List<string>();

        static void Main(string[] args)
        {
            Calculate();
        }

        static void Calculate()
        {
            Console.Write("Name (optional): ");
            var name = Console.ReadLine();

            Console.Write("Bullet damage: ");
            var dmg = double.Parse(Console.ReadLine());

            Console.Write("Sharp penetration: ");
            var sharpPen = double.Parse(Console.ReadLine());

            Console.Write("Blunt penetration: ");
            var bluntPen = double.Parse(Console.ReadLine());

            Console.Write("Burst shot count: ");
            var burstcount = double.Parse(Console.ReadLine());

            Console.Write("RPM: ");
            var burstticks = (3600 / double.Parse(Console.ReadLine()));
            if (Double.IsInfinity(burstticks))
                burstticks = 0;

            Console.Write("Ranged cooldown seconds: ");
            var cooldown = double.Parse(Console.ReadLine()) * 60;

            Console.Write("Warmup seconds: ");
            var warmup = double.Parse(Console.ReadLine()) * 60;

            var maxdps = (dmg * burstcount) / ((cooldown + warmup + (burstticks * (burstcount - 1))) / 60);
            var maxdpsrounded = RoundUp(maxdps, 2);

            var dpsSharpPen = sharpPen * maxdpsrounded;
            var dpsBluntPen = bluntPen * maxdpsrounded;

            var results = $"{name} max DPS: {maxdpsrounded} | sharp PS: {dpsSharpPen} | blunt PS: {dpsBluntPen}";
            PreviousEntries.Add(results);

            // print results and previous results if any
            Console.WriteLine(results);
            foreach (var entry in PreviousEntries)
                Console.WriteLine(entry);

            Console.ReadLine();

            Console.Clear();
            Calculate();
        }

        public static double RoundUp(double input, int places)
        {
            double multiplier = Math.Pow(10, Convert.ToDouble(places));
            return Math.Ceiling(input * multiplier) / multiplier;
        }
    }
}
