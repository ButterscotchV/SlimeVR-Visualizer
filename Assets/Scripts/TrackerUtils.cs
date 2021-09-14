using System.Collections.Generic;
using static TrackerBodyPositionValues;

public static class TrackerUtils
{
	public static TrackerFrame FindTracker(this IEnumerable<TrackerFrame> trackers, TrackerBodyPosition designation)
	{
		foreach (TrackerFrame tracker in trackers)
		{
			if (tracker != null && tracker.Designation == designation)
			{
				return tracker;
			}
		}
		return null;
	}

	public static TrackerFrame FindTracker(this TrackerFrame[] trackers, TrackerBodyPosition designation)
	{
		foreach (TrackerFrame tracker in trackers)
		{
			if (tracker != null && tracker.Designation == designation)
			{
				return tracker;
			}
		}
		return null;
	}

	public static TrackerFrame FindTracker(this IEnumerable<TrackerFrame> trackers, TrackerBodyPosition designation, TrackerBodyPosition altDesignation)
	{
		return FindTracker(trackers, designation) ?? FindTracker(trackers, altDesignation);
	}

	public static TrackerFrame FindTracker(this TrackerFrame[] trackers, TrackerBodyPosition designation, TrackerBodyPosition altDesignation)
	{
		return FindTracker(trackers, designation) ?? FindTracker(trackers, altDesignation);
	}
}
