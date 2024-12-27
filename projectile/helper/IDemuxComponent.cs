namespace evilbug.projectile.helper;

public interface IDemuxComponent {
  // updates the demux
  // - delta: delta time for this demux call
  // - secs_behind: interval between time this is "handled", and current delta frontier
  // - is_interval: true if this call falls on an interval boundary, else false
  public void UpdateDemux(
    double delta, 
    double secs_behind,
    bool is_interval
  );
}