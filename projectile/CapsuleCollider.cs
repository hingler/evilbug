using System;
using System.Numerics;

namespace evilbug.projectile;

public struct CapsuleCollider : ICollider {
  public Vector2 Start;
  public Vector2 End;
  public float RadiusA;
  public float RadiusB;

  // this is iquilez code i dont have the link sorry
  public float Sample(Vector2 point) {
    Vector2 p = (point - Start);
    Vector2 pb = (End - Start);

    float h = Vector2.Dot(pb, pb);
    Vector2 q = new(
      Vector2.Dot(p, new Vector2(pb.Y, -pb.X)) / h,
      Vector2.Dot(p, pb) / h
    );

    q.X = MathF.Abs(q.X);
    float b = (RadiusA - RadiusB);
    Vector2 c = new(MathF.Sqrt(MathF.Max(h - b * b, 0.00001f)), b);

    float k = c.X * q.Y - c.Y * q.X;
    float m = Vector2.Dot(c, q);
    float n = Vector2.Dot(q, q);

    if (k < 0.0f) {
      return MathF.Sqrt(h * n) - RadiusA;
    } else if (k > c.X) {
      return MathF.Sqrt(h * (n + 1.0f - 2.0f * q.Y)) - RadiusB;
    }

    return m - RadiusA;
  }
}