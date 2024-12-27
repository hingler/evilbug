using System;
using evilbug.context;
using evilbug.enemy;
using evilbug.node;
using evilbug.node.projectile;
using evilbug.node.ship;
using Godot;

public class SwoopingEnemy : BugNode {
  private readonly EnemyTarget target;
  private readonly Emitter emitter;
  private readonly INodeContext context;

  private readonly float velocity;
  private readonly float swoop_threshold;
  private readonly int swoop_sign;

  private bool has_shot = false;

  public SwoopingEnemy(
    INodeContext context,
    float velocity,
    float swoop_threshold,
    int swoop_sign,
    long health,
    long score
  ) {
    this.velocity = velocity;
    this.swoop_threshold = swoop_threshold;
    this.context = context;

    target = new(context, health, score);
    emitter = new(context, new SimpleProjectileFactory(context, 125.0f) {
      CollideMask = (uint)CollideMaskEnum.ALLY
    });

    tex = NodeTex.SHIP_ENEMY;

    this.swoop_sign = Math.Sign(swoop_sign);

    AddChild(target);
    AddChild(emitter);
    target.AddDestroyHandler(Pop);
    target.Radius = 6.0f;

    Size = new(12.0f);
    Anchor = new(0.5f);
  }

  public override void Update(double delta) {
    base.Update(delta);

    
    if (GetGlobalPosition().Y < swoop_threshold) {
      Rotation += (float)(0.85 * delta * swoop_sign);

      if (!has_shot) {
        emitter.PointAt(context.GetPlayer());

        emitter.Emit();
        has_shot = true;
      }
    }

    // should be facing downwards
    Rotation = Math.Clamp(Rotation, 1.3f * MathF.PI, 1.7f * MathF.PI);

    Position += RotationVector * (float)(velocity * delta);

  }


}