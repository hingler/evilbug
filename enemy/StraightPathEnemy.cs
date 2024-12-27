using System.Numerics;
using evilbug.context;
using evilbug.node;

namespace evilbug.enemy;

public class StraightPathEnemy : BugNode {
  private readonly EnemyTarget target;

  private readonly float velocity;

  public StraightPathEnemy(
    INodeContext context,
    float velocity,
    long health,
    long score
  ) {
    this.velocity = velocity;
    target = new(context, health, score);

    AddChild(target);

    target.AddDestroyHandler(OnDestroy);
    target.Radius = 6.0f;

    tex = NodeTex.SHIP_ENEMY;

    Size = new(12.0f);
    Anchor = new(0.5f);
  }

  public override void Update(double delta) {
    base.Update(delta);
    Position += RotationVector * (float)(velocity * delta);
  }

  private void OnDestroy() {
    Pop();
  }
}