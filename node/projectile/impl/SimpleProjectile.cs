using System;
using System.Numerics;
using evilbug.context;
using evilbug.projectile;
using evilbug.util;

namespace evilbug.node.projectile.impl;

// projectile impl which travels outward from a given direction

public class SimpleProjectile : IProjectile {

  private Vector2 position_;
  private readonly float velocity_;
  public Vector2 Position { get => position_; }
  public float Rotation { get; }
  public float Radius => 0.5f;
  public uint CollideMask { get; }
  
  private bool active_;
  public bool Active => active_;
  
  public int Identifier => 1;

  public long Damage => 1;
  private readonly Rect screenrect;

  public SimpleProjectile(
    INodeContext context,
    Vector2 init_position, 
    float rotation, 
    float velocity,
    uint CollideMask = 0xFFFFFFFF
  ) {
    position_ = init_position;
    Rotation = rotation;
    velocity_ = velocity;
    active_ = true;

    this.CollideMask = CollideMask;

    screenrect = context.GetScreenRect();
  }

  public virtual void Update(double delta) {
    Vector2 vel_vector = new(MathF.Cos(Rotation), MathF.Sin(Rotation));
    position_ += vel_vector * (float)(velocity_ * delta);

    if (!screenrect.Contains(position_)) {
      active_ = false;
    }
  }

  public void OnHit() {
    active_ = false;
  }

  public Vector2 GetGlobalPosition() => Position;
}