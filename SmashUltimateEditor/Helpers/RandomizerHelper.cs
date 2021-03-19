using System;
using System.Collections.Generic;

namespace YesweDo.Helpers
{
    public class RandomizerHelper
    {
        public static List<int> fighterDistribution = RandomizerHelper.BuildMinDistributionList(1, 8, 3);
        public static List<int> fighterLoseEscortDistribution = RandomizerHelper.BuildMinDistributionList(2, 8, 3, 0);
        public static List<int> eventDistribution = RandomizerHelper.BuildMinDistributionList(1, 3, 1, 0);

        public static int GetRandomInt(Random rnd = null)
        {
            if(rnd is null)
                rnd = new Random();
            return rnd.Next();
        }

        public static int GetRandomBoolean(Random rnd = null)
        {
            if (rnd is null)
                rnd = new Random();
            return new Random().Next();
        }

        public static float GetRandomFloatInRange(float min, float max, Random rnd = null)
        {
            if (rnd is null)
                rnd = new Random();
            return Single.Parse((rnd.Next((int)(min * 100), (int)(max * 100))).ToString()) / 100;
        }

        public static bool ChancePass(int chance, ref Random rnd)
        {
            if (rnd is null)
                rnd = new Random();
            return rnd.NextDouble() <= ((float)chance / 100);
        }
        public static List<int> BuildMinDistributionList(int preferred, int max, double mod1 = 1.0, double mod2 = 0.5)
        {
            var lint = new List<int>();

            for (int i = preferred; i <= max; i++)
            {
                for (int j = preferred; j <= i; j++)
                {
                    for(int k = 0; k < mod1; k++)
                    {
                        lint.Add(j);
                    }
                }
            }
            // Make one fighter more likely significantly.  
            for (int i = (int)(lint.Count * mod2); i > 0; i--)
            {
                lint.Add(preferred);
            }

            return lint;
        }

        public static List<int> BuildMaxDistributionList(int preferred, int min, double mod1 = 1.0, double mod2 = 0.5)
        {
            var lint = new List<int>();

            for (int i = preferred; i > min; i--)
            {
                for (int j = preferred * (int)mod1; j > i ; j--)
                {
                    lint.Add(j);
                }
            }
            // Make one fighter more likely significantly.  
            for (int i = (int)(lint.Count * mod2); i > 0; i--)
            {
                lint.Add(preferred);
            }

            return lint;
        }
    }
}
