using System;
using System.Numerics;
using evilbug.context;
using evilbug.node.projectile.impl;
using evilbug.projectile;

namespace evilbug.node.projectile;

public class SimpleProjectileFactory : IProjectileFactory {

  private readonly INodeContext context;
  private readonly float velocity;

  public uint CollideMask = 0xFFFFFFFF;

  public SimpleProjectileFactory(INodeContext context, float velocity) {
    this.context = context;
    this.velocity = velocity;
  }

  public IProjectile Create(Vector2 pos, Vector2 rot) {
    return new SimpleProjectile(context, pos, MathF.Atan2(rot.Y, rot.X), velocity, CollideMask);
  }
}