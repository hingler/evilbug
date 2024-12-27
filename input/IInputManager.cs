using System.Numerics;

namespace evilbug.input;

public interface IInputManager {
  public Vector2 GetDirectionalInput();
  public bool IsActionPressed(ActionEnum e);

  // tba: event listener, simple shit
}