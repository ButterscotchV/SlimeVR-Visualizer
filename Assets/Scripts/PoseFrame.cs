using System.Collections.Generic;
using static TrackerBodyPositionEnum;

public class PoseFrame
{
	public readonly Dictionary<TrackerBodyPosition, TrackerFrame> TrackerFrames;

	public PoseFrame(Dictionary<TrackerBodyPosition, TrackerFrame> trackerFrames)
	{
		TrackerFrames = trackerFrames;
    }

	public PoseFrame(List<TrackerFrame> trackerFrames)
	{
		Dictionary<TrackerBodyPosition, TrackerFrame> trackerFramesDict = new Dictionary<TrackerBodyPosition, TrackerFrame>(trackerFrames.Count);

		foreach (TrackerFrame trackerFrame in trackerFrames)
		{
			trackerFramesDict[trackerFrame.Designation] = trackerFrame;
		}

		TrackerFrames = trackerFramesDict;
	}
}
