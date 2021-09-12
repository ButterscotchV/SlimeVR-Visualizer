using System.Collections.Generic;

public class TrackerBodyPositionEnum
{
	public class TrackerBodyPosition
	{
		public readonly string Designation;

		public TrackerBodyPosition(string designation)
		{
			Designation = designation;
			_byDesignation[designation.ToLower()] = this;
		}
	}

	private static readonly Dictionary<string, TrackerBodyPosition> _byDesignation = new Dictionary<string, TrackerBodyPosition>();

	public static readonly TrackerBodyPosition None = new TrackerBodyPosition("");
	public static readonly TrackerBodyPosition Hmd = new TrackerBodyPosition("body:HMD");
	public static readonly TrackerBodyPosition Chest = new TrackerBodyPosition("body:chest");
	public static readonly TrackerBodyPosition Waist = new TrackerBodyPosition("body:waist");

	public static readonly TrackerBodyPosition LeftLeg = new TrackerBodyPosition("body:left_leg");
	public static readonly TrackerBodyPosition RightLeg = new TrackerBodyPosition("body:right_leg");

	public static readonly TrackerBodyPosition LeftAnkle = new TrackerBodyPosition("body:left_ankle");
	public static readonly TrackerBodyPosition RightAnkle = new TrackerBodyPosition("body:right_ankle");

	public static readonly TrackerBodyPosition LeftFoot = new TrackerBodyPosition("body:left_foot");
	public static readonly TrackerBodyPosition RightFoot = new TrackerBodyPosition("body:right_foot");

	public static readonly TrackerBodyPosition LeftController = new TrackerBodyPosition("body:left_controller");
	public static readonly TrackerBodyPosition RightController = new TrackerBodyPosition("body:right_conroller");

	public static TrackerBodyPosition GetByDesignation(string designation)
	{
		if (designation == null || !_byDesignation.TryGetValue(designation.ToLower(), out var bodyPosition))
		{
			return null;
		}

		return bodyPosition;
	}
}
