using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace rimworld_dps_calc
{
    class Program
    {
        static List<string> PreviousEntries = new List<string>();
        private static int entryIndex = 0;

        private const int distanceTouch = 3;
        private const int distanceShort = 12;
        private const int distanceMedium = 25;
        private const int distanceLong = 40;

        private const int aimTicksMin = 30;
        private const int aimTicksMax = 240;

        // represents the accuracy of the shooter, which can be pulled from Numbers under 'Aiming Accuracy'
        // this value is a percent
        // 0.60 is the average aiming accuracy of my current colonists
        private const double pawnAccuracy = 0.60;

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

            Console.Write("Magazine size: ");
            var magazineSize = int.Parse(Console.ReadLine());

            Console.Write("Reload time seconds: ");
            var reloadTime = double.Parse(Console.ReadLine());

            Console.Write("Burst shot count - use the second value (1 for non-burst weapons): ");
            var burstCount = int.Parse(Console.ReadLine());

            Console.Write("RPM (0 for non-burst weapons): ");
            var burstTicks = (3600 / double.Parse(Console.ReadLine())); // rounds per second
            if (Double.IsInfinity(burstTicks))
                burstTicks = 0;

            Console.Write("Ranged cooldown seconds: ");
            var cooldownTime = double.Parse(Console.ReadLine());

            Console.Write("Warmup seconds: ");
            var warmupTime = double.Parse(Console.ReadLine());

            Console.WriteLine("Sights Efficiency: ");
            var sightsEfficiency = double.Parse(Console.ReadLine());

            Console.WriteLine("Sway Factor: ");
            var swayFactor = double.Parse(Console.ReadLine());


            double parseTimer = 30;
            int bulletsFired = 0;
            int bulletsInMagazine = magazineSize;

            while (parseTimer > 0)
            {
                if (bulletsInMagazine > 0)
                {
                    // subtract warmup - is this part of the game calcs?
                    parseTimer -= warmupTime;

                    // subtract aim time
                    var aimTime = Lerp(aimTicksMin, aimTicksMax, (distanceLong / 100));
                    parseTimer -= aimTime / 60;

                    // subtract fire time
                    var fireTime = (burstTicks * (burstCount - 1)) / 60;
                    parseTimer -= fireTime; //convert to seconds

                    // count total bullets fired
                    bulletsFired += burstCount;

                    // subtract cooldown
                    parseTimer -= cooldownTime;
                }
                else
                {
                    // subtract reload
                    parseTimer -= reloadTime;
                }
            }

            var dps = RoundUp(((dmg * bulletsFired) / 60), 2);
            var dpsSharp = RoundUp(((sharpPen * bulletsFired) / 60), 2);
            var dpsBlunt = RoundUp(((bluntPen * bulletsFired) / 60), 2);

            // this accuracy formula is made pulling some details from the game
            // pawnaccuracy is a constant representing the average shooting accuracy of my colonists
            // swayfactor would have extra calculations based on pawn shooting accuracy but calcualted a different way that I can't figure out
            var accuracyFactor = (1.5f - pawnAccuracy) / sightsEfficiency;
            var accuracy = RoundUp((accuracyFactor * swayFactor * ((bulletsFired + 1) * 0.75f)), 2);

            var results = $"{entryIndex}) {name} | base: {dps} | sharp: {dpsSharp} | blunt: {dpsBlunt} | accuracy variance: {accuracy}";

            PreviousEntries.Add(results);

            foreach (var entry in PreviousEntries)
                Console.WriteLine(entry);

            Console.ReadLine();

            Console.Clear();
            entryIndex += 1;
            Calculate();
        }

        public static double RoundUp(double input, int places)
        {
            double multiplier = Math.Pow(10, Convert.ToDouble(places));
            return Math.Ceiling(input * multiplier) / multiplier;
        }

        // mathf implementation of linear interpolation
        public static float Lerp(int a, int b, float t)
        {
            return a + (b - a) * Clamp01(t);
        }

        // Clamps value between 0 and 1 and returns value
        public static float Clamp01(float value)
        {
            if (value < 0F)
                return 0F;
            else if (value > 1F)
                return 1F;
            else
                return value;
        }

        // mathf min
        public static float Min(float a, float b) { return a < b ? a : b; }

        //mathf max
        public static float Max(float a, float b) { return a > b ? a : b; }
    }
}
