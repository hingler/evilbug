namespace evilbug.player;

public interface IScoreTracker {
  public void AddScore(long points);
  public long Score { get; }

  // tba: combos? multipliers?
}