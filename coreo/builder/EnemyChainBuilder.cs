using System.Numerics;
using evilbug.context;
using evilbug.enemy;
using evilbug.node;

namespace evilbug.coreo.builder;

#nullable enable

public class EnemyChainBuilder : IEnemyBuilder {
  private int count = 0;
  private float spacing = 0.0f;
  private Vector2 direction = Vector2.UnitX;
  private Vector2 init_position = Vector2.Zero;
  private float velocity = 0.0f;

  private readonly INodeContext ctx;

  // how do we want to register "enemies" so that they can run into the player?
  // - turn enemy planes into projectiles, and destroy on hit
  // - detect enemy planes separately
  // - nest projectiles underneath nodes

  public EnemyChainBuilder(INodeContext ctx) { this.ctx = ctx; }

  public EnemyChainBuilder Count(int number) {
    count = number;
    return this;
  }

  public EnemyChainBuilder Spacing(float space) {
    spacing = space;
    return this;
  }

  public EnemyChainBuilder Direction(float x, float y) {
    direction.X = x;
    direction.Y = y;
    direction = Vector2.Normalize(direction);
    return this;
  }

  public IEnemyBuilder Position(float x, float y) {
    init_position.X = x;
    init_position.Y = y;

    return this;
  }

  public EnemyChainBuilder Velocity(float velocity) {
    this.velocity = velocity;

    return this;
  }

  public void Instantiate(BugNode? coreographer = null) {
    // this makes sense to me rn
    Vector2 offset = -coreographer?.Position ?? Vector2.Zero;
    BugNode root = coreographer ?? ctx.GetRoot();
    if (count > 0) {
      for (int i = 0; i < count; i++) {
        Vector2 position = init_position - (spacing * direction * i);
        StraightPathEnemy e = new(ctx, velocity, 5, 100) {
          Position = position + offset,
          RotationVector = direction
        };

        root.AddChild(e);
      }
    }
  }
}