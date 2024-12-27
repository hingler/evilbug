using System;
using System.Numerics;
using evilbug.context;
using evilbug.node;
using evilbug.node.projectile;
using evilbug.node.projectile.impl;
using evilbug.node.ship;
using evilbug.projectile;

namespace evilbug.demo;

public class SpinningEmitter : BugNode, IProjectileFactory {
  private readonly Emitter emitter;

  private readonly INodeContext context;

  private double delta_acc = 0.0;

  public SpinningEmitter(INodeContext context) {
    this.context = context;
    emitter = new(context, this);
    AddChild(emitter);

    emitter.Position = new(0.0f);
    emitter.Rotation = 0.0f;
  }

  public override void Update(double delta) {
    base.Update(delta);

    double delta_fract = delta / 2.0;

    for (int i = 0; i < 2; i++) {
      emitter.Rotation = (float)(-2.0 * delta_acc);

      emitter.Position = new(
        (float)(Math.Cos(delta_acc * 0.8) * 16.5),
        (float)(Math.Sin(delta_acc * 0.8) * 16.5)
      );


      emitter.Emit();
      delta_acc += delta_fract;
    }
    
  }

  public IProjectile Create(Vector2 pos, Vector2 rot) {
    SimpleProjectile p = new(
      context,
      pos, 
      MathF.Atan2(rot.Y, rot.X), 
      35.0f, 
      (uint)CollideMaskEnum.ALLY
    );
    return p;
  }
}