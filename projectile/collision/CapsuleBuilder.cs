using System.Numerics;

namespace evilbug.projectile.collision;

public class CapsuleBuilder {
  private Vector2 prev_pos = Vector2.Zero;
  private Vector2 curr_pos = Vector2.Zero;

  private readonly float radius;

  public CapsuleBuilder(Vector2 init_pos, float radius) {
    prev_pos = curr_pos = init_pos;
    this.radius = radius;
  }
  public void Update(Vector2 new_pos) {
    prev_pos = curr_pos;
    curr_pos = new_pos;
  }

  public CapsuleCollider GetCollider() {
    CapsuleCollider res = new() {
      Start = prev_pos,
      End = curr_pos,
      RadiusA = radius,
      RadiusB = radius
    };

    return res;
  }
}