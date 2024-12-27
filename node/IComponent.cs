using System.Numerics;

namespace evilbug.node;

public interface IBugComponent {

  public Vector2 GetGlobalPosition();
  void Update(double delta);
}