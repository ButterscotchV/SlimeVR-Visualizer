using System.Collections.Generic;

public class TrackerBodyPositionValues
{
	public enum TrackerBodyPositionEnum
	{
		None,
		Hmd,
		Chest,
		Waist,

		LeftLeg,
		RightLeg,

		LeftAnkle,
		RightAnkle,

		LeftFoot,
		RightFoot,

		LeftController,
		RightController
	}

	public class TrackerBodyPosition
	{
		public readonly TrackerBodyPositionEnum EnumValue;
		public readonly string Designation;

		public TrackerBodyPosition(TrackerBodyPositionEnum enumValue, string designation)
		{
			EnumValue = enumValue;
			Designation = designation;

			_byDesignation[designation.ToLower()] = this;
		}
	}

	private static readonly Dictionary<string, TrackerBodyPosition> _byDesignation = new Dictionary<string, TrackerBodyPosition>();

	public static readonly TrackerBodyPosition None = new TrackerBodyPosition(TrackerBodyPositionEnum.None, "");
	public static readonly TrackerBodyPosition Hmd = new TrackerBodyPosition(TrackerBodyPositionEnum.Hmd, "body:HMD");
	public static readonly TrackerBodyPosition Chest = new TrackerBodyPosition(TrackerBodyPositionEnum.Chest, "body:chest");
	public static readonly TrackerBodyPosition Waist = new TrackerBodyPosition(TrackerBodyPositionEnum.Waist, "body:waist");

	public static readonly TrackerBodyPosition LeftLeg = new TrackerBodyPosition(TrackerBodyPositionEnum.LeftLeg, "body:left_leg");
	public static readonly TrackerBodyPosition RightLeg = new TrackerBodyPosition(TrackerBodyPositionEnum.RightLeg, "body:right_leg");

	public static readonly TrackerBodyPosition LeftAnkle = new TrackerBodyPosition(TrackerBodyPositionEnum.LeftAnkle, "body:left_ankle");
	public static readonly TrackerBodyPosition RightAnkle = new TrackerBodyPosition(TrackerBodyPositionEnum.RightAnkle, "body:right_ankle");

	public static readonly TrackerBodyPosition LeftFoot = new TrackerBodyPosition(TrackerBodyPositionEnum.LeftFoot, "body:left_foot");
	public static readonly TrackerBodyPosition RightFoot = new TrackerBodyPosition(TrackerBodyPositionEnum.RightFoot, "body:right_foot");

	public static readonly TrackerBodyPosition LeftController = new TrackerBodyPosition(TrackerBodyPositionEnum.LeftController, "body:left_controller");
	public static readonly TrackerBodyPosition RightController = new TrackerBodyPosition(TrackerBodyPositionEnum.RightController, "body:right_conroller");

	public static TrackerBodyPosition GetByDesignation(string designation)
	{
		if (designation == null || !_byDesignation.TryGetValue(designation.ToLower(), out var bodyPosition))
		{
			return null;
		}

		return bodyPosition;
	}

	public static TrackerBodyPosition GetByEnumValue(TrackerBodyPositionEnum enumValue)
	{
		foreach (TrackerBodyPosition trackerBodyPosition in _byDesignation.Values)
		{
			if (trackerBodyPosition.EnumValue == enumValue)
			{
				return trackerBodyPosition;
			}
		}

		return null;
	}
}
