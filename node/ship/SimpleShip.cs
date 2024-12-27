using System;
using System.Collections.Generic;
using evilbug.context;
using evilbug.input;
using evilbug.node.projectile;
using evilbug.projectile;
using evilbug.projectile.collision;
using evilbug.projectile.helper;
using evilbug.util;
using Vector2 = System.Numerics.Vector2;

namespace evilbug.node.ship;

#nullable enable

// ships are effectively just emitters on a node
// what more does a ship need?

// - I think we want "spawners" that will put ships in global space

public class SimpleShip : BugNode, IDemuxComponent {
  private readonly List<Emitter> emitters;
  private readonly SimpleProjectileFactory factory;
  private readonly IInputManager manager;
  private readonly TickDemuxer demux;
  private readonly CapsuleBuilder builder;

  private readonly IProjectileManager projectiles;

  private readonly INodeContext context;

  public bool Hit = false;

  // lets go with a size of "5"
  public SimpleShip(INodeContext context) {
    Anchor = new(0.5f, 0.5f);
    tex = NodeTex.SHIP_PLAYER;
    this.context = context;
    emitters = [];
    factory = new(context, 165.0f);
    factory.CollideMask = (uint)CollideMaskEnum.ENEMY;
    manager = context.GetInputManager();

    ZIndex = 5;

    // one emitter at the front, two emitters at the sides

    for (int i = 0; i < 3; i++) {
      Emitter e = new(context, factory);
      AddChild(e);
      emitters.Add(e);
    }

    emitters[0].Position = new(-1.0f, -3.0f);
    emitters[1].Position = new(1.0f, 0.0f);
    emitters[2].Position = new(-1.0f, 3.0f);

    demux = new(this, 0.045);

    Rotation = (float)(Math.PI / 2.0);

    builder = new(Position, 4.0f);

    projectiles = context.GetProjectileManager();

    Size = new(12.0f);
    Anchor = new(0.5f);
  }

  public override void Update(double delta) {
    base.Update(delta);
    demux.Update(delta);

    builder.Update(Position);

    IProjectile? proj = projectiles.Test(GetGlobalPosition(), 4.0f, (uint)CollideMaskEnum.ALLY);
    Hit = (proj != null);
  }

  public void UpdateDemux(
    double delta, 
    double secs_behind,
    bool is_interval
  ) {
    Vector2 dir = manager.GetDirectionalInput();

    Vector2 padding = new(4.0f);
    Rect r = context.GetScreenRect();
    Position = Vector2.Clamp(
      Position + dir * (float)(75.0 * delta),
      r.Start + padding, 
      r.End - padding
    );

    if (is_interval && manager.IsActionPressed(ActionEnum.Primary)) {
      Emit(secs_behind);
    }
  }

  public void Emit(double pre_delta) {
    for (int i = 0; i < 3; i++) {
      emitters[i].Emit(pre_delta);
    }
  }
}