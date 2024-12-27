namespace evilbug.projectile.helper;

// demuxes delta events to produce a smoother time flow
// callback with two args:
// - delta
// - true if interval fired, else false
public class TickDemuxer {
  private readonly IDemuxComponent action;


  // measures "simulation" time since last interval
  private double delta_interval = 0.0;
  private readonly double interval;

  private double delta_acc_debug = 0.0;
  private double delta_net_debug = 0.0;


  public TickDemuxer(
    IDemuxComponent demux,
    double interval
  ) {
    action = demux;
    this.interval = interval;
  }

  public void Update(double delta) {
    // time progressed since last tick
    delta_acc_debug += delta;

    double delta_next = delta_interval + delta;

    // net delta
    double delta_net = delta;

    while (delta_next > interval) {
      double delta_init = interval - delta_interval;

      delta_next -= interval;
      delta_net -= delta_init;

      action.UpdateDemux(delta_init, delta_net, true);
      delta_net_debug += delta_init;


      delta_interval = 0.0;
    }

    // 0 < delta_next < interval

    // idea being: we get a precise tick rate, AND we remain caught up to our frame counter
    delta_interval = delta_next;

    if (delta_net > 0.0001) {
      // draining delta - at this call, we're caught up
      action.UpdateDemux(delta_net, 0.0, false);
      delta_net_debug += delta_net;
    }
  }


}