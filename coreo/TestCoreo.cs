using System.Numerics;
using evilbug.context;
using evilbug.coreo.builder;
using evilbug.util;

namespace evilbug.coreo;

public class TestCoreo(INodeContext ctx) : WorldCoreographer {
  public override void Coreograph() {
    AddBuilder(GetChainBuilder(false), 1.5f);
    AddBuilder(GetChainBuilder(true), 3.5f);

    AddBuilder(new WallBuilder(ctx), 4.0f);

    AddBuilder(GetSwoopBuilder(15.0f), 5.5f);
    AddBuilder(GetSwoopBuilder(-15.0f), 8.5f);

  }

  private IEnemyBuilder GetSwoopBuilder(float initX) {
    return new SwoopBuilder(ctx)
      .WingSize(2)
      .InitX(initX)
      .Spacing(12.0f)
      .Velocity(65.0f)
      .SwoopThreshold(22.0f);
  }

  private IEnemyBuilder GetChainBuilder(bool isLeft) {
    Rect screenspace = ctx.GetScreenRect();

    float boundary = (screenspace.Size.X / 2) + 12.0f;
    float far_end = (screenspace.Size.Y / 2) + 12.0f;
    return new EnemyChainBuilder(ctx)
      .Count(4)
      .Spacing(15.0f)
      .Direction(isLeft ? 1.0f : -1.0f, -0.5f)
      .Velocity(80.0f)
      .Position(isLeft ? -boundary : boundary, far_end + 18.0f);
  }
}