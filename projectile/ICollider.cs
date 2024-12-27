using System.Numerics;

namespace evilbug.projectile;

// returns dist to dingus
public interface ICollider {
  public float Sample(Vector2 position);
}