using System.Collections.Generic;

public class TrackerFrameDataValues
{
	public enum TrackerFrameDataEnum
	{
		Designation = 1 << 0,
		Rotation = 1 << 1,
		Position = 1 << 2
	}

	public class TrackerFrameData
	{
		public readonly TrackerFrameDataEnum EnumValue;

		public int Flag => (int)EnumValue;

		public TrackerFrameData(TrackerFrameDataEnum enumValue)
		{
			EnumValue = enumValue;

			// Set up list
			Values.Add(this);
		}

		public bool Check(int dataFlags)
		{
			return (dataFlags & Flag) != 0;
		}
	}

	public static readonly List<TrackerFrameData> Values = new List<TrackerFrameData>();

	public static readonly TrackerFrameData Designation = new TrackerFrameData(TrackerFrameDataEnum.Designation);
	public static readonly TrackerFrameData Rotation = new TrackerFrameData(TrackerFrameDataEnum.Rotation);
	public static readonly TrackerFrameData Position = new TrackerFrameData(TrackerFrameDataEnum.Position);
}
