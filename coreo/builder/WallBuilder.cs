using System.Security.Cryptography.X509Certificates;
using evilbug.context;
using evilbug.enemy;
using evilbug.node;
using evilbug.util;

namespace evilbug.coreo.builder;

#nullable enable

public class WallBuilder(INodeContext ctx) : IEnemyBuilder {
  public void Instantiate(BugNode? coreographer = null) {
    BugNode root = coreographer ?? ctx.GetRoot();

    BugNode wall = new();

    Rect screen = ctx.GetScreenRect();
    wall.Size = new(screen.Size.X + 16.0f, 8.0f);
    wall.Anchor = new(0.5f);
    wall.Position = new(0.0f, screen.End.Y + 16.0f);

    float size_offset = screen.Size.X / 3;

    root.AddChild(wall);

    wall.ZIndex = -2;

    wall.tex = NodeTex.WALL_WIRE;

    for (int i = -1; i <= 1; i++) {
      BugNode tower = new();

      tower.Position = new(size_offset * i, screen.End.Y + 16.0f);
      tower.Anchor = new(0.5f);
      tower.Size = new(12.0f);

      tower.tex = NodeTex.WALL_TOWER;

      tower.ZIndex = -1;
      root.AddChild(tower);


      TurretEnemy turret = new(ctx, 25, 50);
      tower.AddChild(turret);
    }
  }
}