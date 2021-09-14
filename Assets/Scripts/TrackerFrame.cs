using System.Collections.Generic;
using UnityEngine;
using static TrackerBodyPositionValues;
using static TrackerFrameDataValues;

public class TrackerFrame
{
	public int DataFlags { get; private set; } = 0;

	public readonly string Name = "TrackerFrame";
	public readonly TrackerBodyPosition Designation;
	public readonly Quaternion? Rotation;
	public readonly Vector3? Position;

	public TrackerFrame(string designationString, TrackerBodyPosition designation, Quaternion? rotation, Vector3? position)
	{
		if (designationString != null)
		{
			Name = $"TrackerFrame:/{designationString}";
		}

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

	public TrackerFrame(TrackerBodyPosition designation, Quaternion? rotation, Vector3? position) : this(designation?.Designation, designation, rotation, position)
	{
	}

	public bool HasData(TrackerFrameData flag)
	{
		return flag.Check(DataFlags);
	}

	public override string ToString()
	{
		return Name;
	}
}
