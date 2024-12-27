using System.Numerics;
using evilbug.projectile;

namespace evilbug.node.projectile;

public interface IProjectileFactory<T> where T : IProjectile {
  // handling direction?
  T Create(Vector2 position, Vector2 direction);
}

public interface IProjectileFactory : IProjectileFactory<IProjectile> {}