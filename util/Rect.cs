using System.Numerics;

namespace evilbug.util;

public class Rect {
  public Vector2 Start;
  public Vector2 End;

  public Rect(Vector2 Start, Vector2 End) {
    this.Start = Start;
    this.End = End;
  }

  public Vector2 Size {
    get => End - Start;
    set => End = (Start + value);
  }

  public Vector2 Center {
    get => Start + Size / 2;
    set {
      Vector2 half_size = Vector2.Abs(Size) / 2;
      Start = value - half_size;
      End = value + half_size;
    }
  }

  public bool Contains(Vector2 test) {
    return (
      test.X > Start.X && test.X < End.X
      && test.Y > Start.Y && test.Y < End.Y
    );
  }
}