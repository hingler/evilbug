using System;
using System.Numerics;
using evilbug.context;
using evilbug.node;
using evilbug.util;

namespace evilbug.coreo.builder;

#nullable enable

public class SwoopBuilder : IEnemyBuilder {
  private int wing_size = 1;
  private float spacing = 10.0f;

  private float initX = 0.0f;

  private float velocity = 0.0f;

  private float swoop_threshold = 0.0f;

  private readonly INodeContext ctx;

  public SwoopBuilder(INodeContext ctx) { this.ctx = ctx; }

  public SwoopBuilder WingSize(int number) {
    wing_size = number;
    return this;
  }

  public SwoopBuilder Spacing(float space) {
    spacing = space;
    return this;
  }

  public SwoopBuilder InitX(float X) {
    initX = X;
    return this;
  }

  public SwoopBuilder Velocity(float vel) {
    velocity = vel;
    return this;
  }

  public SwoopBuilder SwoopThreshold(float Y) {
    swoop_threshold = Y;
    return this;
  }

  public void Instantiate(BugNode? coreographer = null) {
    BugNode root = coreographer ?? ctx.GetRoot();

    Rect screenrect = ctx.GetScreenRect();
    float y_frontier = screenrect.End.Y;

    Vector2 offset_global = new(initX, y_frontier + 12.0f);

    for (int i = -wing_size; i <= wing_size; i++) {
      Vector2 offset_local = new(i * spacing, Math.Abs(i) * spacing);
      SwoopingEnemy e = new(ctx, velocity, swoop_threshold, -Math.Sign(initX), 5, 100) {
        Position = offset_local + offset_global,
        Rotation = 1.5f * MathF.PI
      };

      root.AddChild(e);
    }
  }
}