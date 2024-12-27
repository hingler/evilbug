using evilbug.context;
using evilbug.node;
using evilbug.node.projectile;
using evilbug.node.ship;
using evilbug.projectile.helper;

namespace evilbug.enemy;

public class TurretEnemy : BugNode, IDemuxComponent {
  private readonly EnemyTarget target;
  private readonly Emitter emitter;
  private readonly INodeContext context;


  private TickDemuxer fire_rate;

  public TurretEnemy(
    INodeContext context,
    long health,
    long score
  ) {
    target = new(context, health, score);
    emitter = new(context, new SimpleProjectileFactory(context, 110.0f) {
      CollideMask = (uint)CollideMaskEnum.ALLY
    });

    AddChild(emitter);
    AddChild(target);

    this.context = context;

    tex = NodeTex.TURRET;

    Size = new(8.0f);
    Anchor = new(0.5f);

    fire_rate = new(this, 2.0);

    target.AddDestroyHandler(Pop);
  }

  public override void Update(double delta) {
    base.Update(delta);
    fire_rate.Update(delta);
  }

  public void UpdateDemux(double delta, double secs_behind, bool is_interval) {
    PointAt(context.GetPlayer());
    if (is_interval) {
      emitter.Emit(secs_behind);
    }
  }
}