using System.Numerics;

namespace evilbug.projectile;

public interface ICollidable {
  public Vector2 CurrentPos { get; }
  public Vector2 LastPos { get; }
  public int LayerMask { get; }
  public float Radius { get; }
}