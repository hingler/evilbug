using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using evilbug.util;
using Vector2 = System.Numerics.Vector2;

namespace evilbug.projectile.impl;

#nullable enable

public class SimpleProjectileManager : IProjectileManager {
  private readonly Dictionary<IProjectile, PositionHistory> position_data = [];

  public bool RegisterProjectile(IProjectile projectile) {
    PositionHistory ps = new();
    Vector2 pos = projectile.Position;

    ps.CurrentPos.X = pos.X;
    ps.CurrentPos.Y = pos.Y;
    ps.LastPos.X = pos.X;
    ps.LastPos.Y = pos.Y;

    return position_data.TryAdd(projectile, ps);
  }

  public bool UnregisterProjectile(IProjectile projectile) {
    return position_data.Remove(projectile);
  }

  public IEnumerable<IProjectile> GetProjectiles() {
    return position_data.Keys;
  }

  public void Update(double delta) {
    position_data.RemoveAll((p) => !p.Active);

    foreach (KeyValuePair<IProjectile, PositionHistory> pair in position_data) {
      PositionHistory ps = pair.Value;
      IProjectile proj = pair.Key;

      Vector2 pos_last = proj.Position;

      proj.Update(delta);

      Vector2 pos_current = proj.Position;

      ps.LastPos = pos_last;
      ps.CurrentPos = pos_current;  
    }

    // gut any disposables
  }

  public IProjectile? Test(ICollider collider, uint mask, int time_steps = 4) {
    return TestAll(collider, mask, time_steps, 1).ElementAtOrDefault(0);
  }

  public IProjectile? Test(Vector2 point, float radius, uint mask) {
    // tba: do some sort of "grid based" simplification of collision testing
    return TestAll(point, radius, mask, 1).ElementAtOrDefault(0);
  }

  public IList<IProjectile> TestAll(
    ICollider collider,
    uint mask,
    int time_steps = 4,
    int count_max = 0
  ) {
    List<IProjectile> res = [];

    foreach (KeyValuePair<IProjectile, PositionHistory> pair in position_data) {
      PositionHistory ps = pair.Value;
      IProjectile proj = pair.Key;

      if (TestProjectile(collider, mask, proj, ps, time_steps)) {
        res.Add(proj);
        if (count_max > 0 && res.Count >= count_max) {
          return res;
        }
      }
    }

    return res;
  }

  private bool TestProjectile(
    ICollider collider,
    uint mask,
    IProjectile proj,
    PositionHistory ps,
    int time_steps
  ) {
    if ((mask & proj.CollideMask) == 0) {
      return false;
    }

    Vector2 pos_start = ps.LastPos;
    Vector2 pos_end = ps.CurrentPos;
    
    int step_net = Math.Max(time_steps - 1, 1);

    for (int i = 0; i <= step_net; i++) {
      float mix_fract = i / (float)step_net;
      Vector2 test_pos = pos_start * (1.0f - mix_fract) + pos_end * mix_fract;

      float dist = collider.Sample(test_pos);
      if (dist < proj.Radius) {
        return true;
      }
    }

    return false;
  }

  public IList<IProjectile> TestAll(
    Vector2 point, 
    float radius, 
    uint mask,
    int count_max = 0
    ) {
    CapsuleCollider workspace;
    List<IProjectile> res = [];

    foreach (KeyValuePair<IProjectile, PositionHistory> pair in position_data) {
      PositionHistory ps = pair.Value;
      IProjectile proj = pair.Key;

      // not stellar
      if ((mask & proj.CollideMask) == 0) {
        continue;
      }

      workspace.Start = ps.LastPos;
      workspace.End = ps.CurrentPos;

      workspace.RadiusA = proj.Radius;
      workspace.RadiusB = proj.Radius;

      float dist = workspace.Sample(point);
      if (dist < radius) {
        res.Add(proj);

        if (count_max > 0 && res.Count >= count_max) {
          break;
        }
      }
    }

    return res;
  }
}