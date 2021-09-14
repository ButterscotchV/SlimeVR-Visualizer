using System.Collections.Generic;
using static TrackerBodyPositionValues;

public class PoseFrame
{
	public readonly List<TrackerFrame> TrackerFrames;

	public PoseFrame(List<TrackerFrame> trackerFrames)
	{
		TrackerFrames = trackerFrames;
    }

	public TrackerFrame FindTracker(TrackerBodyPosition designation)
	{
		foreach (TrackerFrame tracker in TrackerFrames)
		{
			if (tracker.Designation == designation)
			{
				return tracker;
			}
		}
		return null;
	}

	public TrackerFrame FindTracker(TrackerBodyPosition designation, TrackerBodyPosition altDesignation)
	{
		return FindTracker(designation) ?? FindTracker(altDesignation);
	}
}
