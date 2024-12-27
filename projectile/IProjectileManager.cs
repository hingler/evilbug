using System.Collections.Generic;
using System.Numerics;

namespace evilbug.projectile;

#nullable enable

public interface IProjectileManager {
  public bool RegisterProjectile(IProjectile projectile);
  public bool UnregisterProjectile(IProjectile projectile);

  public IEnumerable<IProjectile> GetProjectiles();


  // tests passed collider
  public IProjectile? Test(Vector2 point, float radius, uint mask);
  public IList<IProjectile> TestAll(Vector2 point, float radius, uint mask, int count_max = 0);
}