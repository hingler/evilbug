using System;
using System.Collections.Generic;
using evilbug.context;
using evilbug.node;
using evilbug.node.ship;
using evilbug.player;
using evilbug.projectile;
using Godot;

namespace evilbug.enemy;


public class EnemyTarget : BugNode {
  private long hit_points;
  private long score_bonus;



  public float Radius = 4.0f;
  public long HitPoints => hit_points;
  
  private IProjectileManager manager;
  private IScoreTracker score;


  private readonly ISet<Action> destroy_listeners = new HashSet<Action>();

  public EnemyTarget(
    INodeContext node_context,
    long hit_points,
    long score_bonus
  ) {
    manager = node_context.GetProjectileManager();
    score = node_context.GetScore();

    this.hit_points = hit_points;
    this.score_bonus = score_bonus;
  }

  public void AddDestroyHandler(Action action) {
    destroy_listeners.Add(action);
  }

  public override void Update(double delta) {
    if (hit_points > 0) {
      IList<IProjectile> proj_list = manager.TestAll(GetGlobalPosition(), Radius, (uint)CollideMaskEnum.ENEMY);
      foreach (IProjectile proj in proj_list) {
        long new_points = Math.Max(hit_points - proj.Damage, 0L);
        long points_scored = hit_points - new_points;

        proj.OnHit();
        GD.Print("hit: ", proj.Position);

        // thinking: destroy projectiles after hit
        score.AddScore(points_scored);

        if (hit_points != 0 && new_points == 0) {
          // register as a kill
          GD.Print("kill scored");
          score.AddScore(score_bonus);
          foreach (Action a in destroy_listeners) {
            a();
          }

          // up next
          // - doodle sprites
          // - come up with more enemy chains
          // - add some gameplay, projectiles to avoid
          // - add scrolling
          // - send :)
          GD.Print("current score: ", score.Score);
          break;
        }

        hit_points = new_points;
      }
    }
  }
}