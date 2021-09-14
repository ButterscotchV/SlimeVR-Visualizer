using UnityEngine;
using static TrackerBodyPositionValues;
using static TrackerFrameDataValues;

public class TrackerFrame
{
	public readonly int DataFlags = 0;

	public readonly TrackerBodyPosition Designation;
	public readonly Quaternion? Rotation;
	public readonly Vector3? Position;

	public TrackerFrame(TrackerBodyPosition designation, Quaternion? rotation, Vector3? position)
	{
		Designation = designation;
		if (designation != null)
		{
			DataFlags |= TrackerFrameDataValues.Designation.Flag;
		}

		Rotation = rotation;
		if (rotation != null)
		{
			DataFlags |= TrackerFrameDataValues.Rotation.Flag;
		}

		Position = position;
		if (position != null)
		{
			DataFlags |= TrackerFrameDataValues.Position.Flag;
		}
	}

	public bool HasData(TrackerFrameData flag)
	{
		return flag.Check(DataFlags);
	}

	public override string ToString()
	{
		return $"TrackerFrame:/{Designation?.Designation}";
	}
}
