using evilbug.input;
using evilbug.node;
using evilbug.player;
using evilbug.projectile;
using evilbug.util;

namespace evilbug.context;

public interface INodeContext {
  public BugNode GetRoot();

  // fetch the projectile manager

  // tba: view bounds (despawn entities)

  IProjectileManager GetProjectileManager();

  IInputManager GetInputManager();

  IScoreTracker GetScore();

  Rect GetScreenRect();

  BugNode GetPlayer();
}