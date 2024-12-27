using System.Numerics;
using evilbug.node;

namespace evilbug.projectile;

public interface IProjectile : IBugComponent {
  public Vector2 Position { get; }
  public float Rotation { get; }
  public float Radius { get; }
  public uint CollideMask { get; }


  public long Damage { get; }

  // true if still active, false if can be disposed
  public bool Active { get; }
  
  // identifies this particle (ex. when drawing)
  public int Identifier { get; }

  // called when this projectile hits a target
  public void OnHit();
}