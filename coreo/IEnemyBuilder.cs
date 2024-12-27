using evilbug.node;

namespace evilbug.coreo;

#nullable enable

public interface IEnemyBuilder {
  public void Instantiate(BugNode? coreographer = null);
}