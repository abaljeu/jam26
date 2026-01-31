using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace mask
{
  internal static class Rand
  {
    static System.Random rand = new System.Random();

    public static T GetRandomEnumValue<T>() where T : Enum
    {
      Array values = Enum.GetValues(typeof(T));
      return (T)values.GetValue(rand.Next(values.Length));
    }
    public static int Value(int min, int max)
    {
      return rand.Next(min,max);
    }

  }
}
