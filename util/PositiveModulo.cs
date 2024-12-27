namespace evilbug.util;

using System.Numerics;

public static class DigiMath {
  public static T PositiveModulo<T>(T val, T divisor) where T : INumber<T> {
    return ((val % divisor) + divisor) % divisor;
  }
}