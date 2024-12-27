using System.Numerics;
using evilbug.context;
using evilbug.projectile;

namespace evilbug.node.projectile;

public class Emitter : BugNode {

  private readonly INodeContext context;
  private readonly IProjectileFactory factory;

  public Emitter(INodeContext context, IProjectileFactory factory) : base() {
    this.context = context;
    this.factory = factory;
  }

  // thinking: provide a "delta" to pre-simulate by
  public void Emit(double pre_delta = 0.0) {
    IProjectile proj = factory.Create(
      GetGlobalPosition(),
      GetFacingDirection()
    );

    if (pre_delta > 0.0001) {
      proj.Update(pre_delta);
    }

    context.GetProjectileManager().RegisterProjectile(proj);
  }
}