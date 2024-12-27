using System;
using System.Collections.Generic;
using System.Numerics;
using static evilbug.util.DigiMath;

namespace evilbug.node;

#nullable enable

// think "absolute world space" - worry about bounds later
// no content atm
public class BugNode : IBugComponent {
  private Vector2 offset_ = Vector2.Zero;
  private Vector2 anchor_ = Vector2.Zero;
  private float rotation_ = 0.0f;
  private float scale_ = 1.0f;
  private readonly HashSet<BugNode> children = new();
  private BugNode? parent_ = null;
  public NodeTex tex = NodeTex.NONE;
  public BugNode? Parent {
    get => parent_;
  }

  public Vector2 Position {
    get => offset_;
    set => offset_ = value;
  }

  public Vector2 Anchor {
    get => anchor_;
    set => anchor_ = value;
  }

  public float Rotation {
    get => rotation_;
    set => rotation_ = PositiveModulo(value, 2.0f * MathF.PI); 
  }

  public Vector2 Size { get; set; } = Vector2.Zero;

  public int ZIndex;

  public int ZIndexAbsolute {
    get {
      return Parent?.ZIndexAbsolute ?? 0 + ZIndex;
    }
  }



  public Vector2 RotationVector {
    get => new(MathF.Cos(Rotation), MathF.Sin(Rotation));
    set {
      rotation_ = MathF.Atan2(value.Y, value.X);
    }
  }

  public float Scale {
    get => scale_;
    set => scale_ = value;
  }

  public Matrix3x2 GetLocalTransform() {
    Matrix3x2 res = Matrix3x2.Identity;
    res *= Matrix3x2.CreateScale(Scale, Anchor);
    res *= Matrix3x2.CreateRotation(Rotation);
    res *= Matrix3x2.CreateTranslation(Position);

    return res;
  }

  public float GetNetRotation() {
    float ParentRotation = Parent?.GetNetRotation() ?? 0.0f;
    return ParentRotation + Rotation;
  }

  public void PointAt(BugNode node) {
    Vector2 node_global = node.GetGlobalPosition();
    Vector2 self_global = GetGlobalPosition();

    Vector2 net_dir = Vector2.Normalize(node_global - self_global);

    // desired rotation in global space
    double global_angle = Math.Atan2(net_dir.Y, net_dir.X);

    // current rotation in global space
    double current_angle = GetNetRotation();

    // figure out how much we need to append to current angle to get desired rotation
    Rotation += (float)(global_angle - current_angle);


  }

  public void PreUpdate(double delta) {
    Update(delta);
    foreach (BugNode child in GetChildren()) {
      child.PreUpdate(delta);
    }
  }

  // virtual in this case
  public virtual void Update(double delta) {}

  public Matrix3x2 GetGlobalTransform() {
    Matrix3x2 parent_transform = Parent?.GetGlobalTransform() ?? Matrix3x2.Identity;
    return GetLocalTransform() * parent_transform;
  }

  public Vector2 GetGlobalPosition() {
    return Vector2.Transform(Vector2.Zero, GetGlobalTransform());
  }

  public Vector2 GetFacingDirection() {
    return Vector2.Normalize(NormalToGlobal(Vector2.UnitX));
  }

  public Vector2 LocalToGlobal(Vector2 point) {
    return Vector2.Transform(point, GetGlobalTransform());
  }

  public Vector2 NormalToGlobal(Vector2 normal) {
    return Vector2.TransformNormal(normal, GetGlobalTransform());
  }

  public void AddChild(BugNode node) {
    node.Parent?.RemoveChild(node);
    children.Add(node);
    node.parent_ = this;
  }

  public void RemoveChild(BugNode node) {
    if (children.Remove(node)) {
      node.parent_ = null;
    }
  }

  public void Pop() {
    Parent?.RemoveChild(this);
  }

  public IReadOnlyCollection<BugNode> GetChildren() => children;
}