using System;
using System.Collections.Generic;
using System.Text;

namespace SmashUltimateEditor.Helpers
{
    public class RandomizerHelper
    {

        public static int GetRandomInt()
        {
            return new Random().Next();
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
