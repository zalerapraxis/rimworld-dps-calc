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
        private static int index = 0;

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

            Console.Write("Burst shot count - use the first value (1 for non-burst weapons): ");
            var burstcount = double.Parse(Console.ReadLine());

            // 3600 ticks in a minute of realtime, divide that by rpm to get ticks between burst shots
            Console.Write("RPM (0 for non-burst weapons): ");
            var burstticks = (3600 / double.Parse(Console.ReadLine()));
            if (Double.IsInfinity(burstticks))
                burstticks = 0;

            Console.Write("Ranged cooldown seconds: ");
            var cooldown = double.Parse(Console.ReadLine()) * 60;

            Console.Write("Warmup seconds: ");
            var warmup = double.Parse(Console.ReadLine()) * 60;


            var maxdpsAimed = RoundUp((dmg * burstcount) / ((cooldown + warmup + (burstticks * (burstcount - 1))) / 60), 2);
            var maxdpsSnapshot = RoundUp((dmg * (burstcount * 2)) / ((cooldown + warmup + (burstticks * ((burstcount * 2) - 1))) / 60), 2);

            var dpsSharpPenAimed = sharpPen * maxdpsAimed;
            var dpsBluntPenAimed = bluntPen * maxdpsAimed;

            var dpsSharpPenSnapshot = sharpPen * maxdpsSnapshot;
            var dpsBluntPenSnapshot = bluntPen * maxdpsSnapshot;


            var results = $"{index}) {name} base aim: {maxdpsAimed} snap: {maxdpsSnapshot} | " +
                          $"sharp aim: {dpsSharpPenAimed} snap: {dpsSharpPenSnapshot} | " +
                          $"blunt aim: {dpsBluntPenAimed} snap: {dpsBluntPenSnapshot}";
            PreviousEntries.Add(results);

            
            foreach (var entry in PreviousEntries)
                Console.WriteLine(entry);

            Console.ReadLine();

            Console.Clear();
            index += 1;
            Calculate();
        }

        public static double RoundUp(double input, int places)
        {
            double multiplier = Math.Pow(10, Convert.ToDouble(places));
            return Math.Ceiling(input * multiplier) / multiplier;
        }
    }
}
