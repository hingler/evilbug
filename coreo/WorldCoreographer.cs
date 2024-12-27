using System;
using System.Collections.Generic;
using System.Numerics;
using evilbug.node;

namespace evilbug.coreo;

using BuilderPair = Tuple<IEnemyBuilder, double>;
public abstract class WorldCoreographer : BugNode {
  private HashSet<BuilderPair> builders = [];
  private double delta_acc = 0.0;

  bool initialized_ = false;

  public abstract void Coreograph();
  public override void Update(double delta) {
    if (!initialized_) {
      initialized_ = true;
      Coreograph();
    }

    delta_acc += delta;
    Position -= (float)(delta * 14.0) * Vector2.UnitY;

    List<BuilderPair> builders_run = [];
    foreach (BuilderPair bp in builders) {
      if (bp.Item2 < delta_acc) {
        builders_run.Add(bp);
      }
    }

    foreach (BuilderPair bp in builders_run) {
      BugNode node = new() {
        Position = -Position
      };

      AddChild(node);
      bp.Item1.Instantiate(node);

      builders.Remove(bp);
    }
  }

  protected void AddBuilder(IEnemyBuilder builder, double delay) {
    builders.Add(new(builder, delay));
  }
}