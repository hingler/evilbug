namespace evilbug.player;

public class SimpleScoreTracker : IScoreTracker {
  public long Score { get; set; } = 0L;

  public void AddScore(long points) {
    Score += points;
  }
}