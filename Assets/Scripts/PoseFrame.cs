using System.Collections.Generic;

public class PoseFrame
{
	private readonly List<PoseFrameTracker> Trackers;

	public PoseFrame(List<PoseFrameTracker> trackers)
	{
		Trackers = trackers;
    }

	public int GetTrackerCount()
	{
		return Trackers.Count;
	}

	public List<PoseFrameTracker> GetTrackers()
	{
		return Trackers;
	}

	public int GetMaxFrameCount()
	{
		int maxFrames = 0;

		for (int i = 0; i < Trackers.Count; i++)
		{
			PoseFrameTracker tracker = Trackers[i];
			if (tracker != null && tracker.GetFrameCount() > maxFrames)
			{
				maxFrames = tracker.GetFrameCount();
			}
		}

		return maxFrames;
	}

	public int GetFrames(int index, TrackerFrame[] buffer)
	{
		for (int i = 0; i < Trackers.Count; i++)
		{
			buffer[i] = Trackers[i]?.SafeGetFrame(index);
		}

		return Trackers.Count;
	}

	public TrackerFrame[] GetFrames(int index)
	{
		TrackerFrame[] buffer = new TrackerFrame[Trackers.Count];
		GetFrames(index, buffer);
		return buffer;
	}
}
