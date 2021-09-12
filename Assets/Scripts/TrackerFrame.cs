using UnityEngine;

public class TrackerFrame
{
	public int DataFlags { get; private set; } = 0;

	public readonly TrackerBodyPosition Designation;
	public readonly Quaternion? Rotation;
	public readonly Vector3? Position;

	public TrackerFrame(TrackerBodyPosition designation, Quaternion? rotation, Vector3? position)
	{
		Designation = designation;
		if (designation != null)
		{
			DataFlags |= TrackerFrameData.Designation.Flag;
		}

		Rotation = rotation;
		if (rotation != null)
		{
			DataFlags |= TrackerFrameData.Rotation.Flag;
		}

		Position = position;
		if (position != null)
		{
			DataFlags |= TrackerFrameData.Position.Flag;
		}
	}

	public bool HasData(TrackerFrameData flag)
	{
		return flag.Check(DataFlags);
	}
}
