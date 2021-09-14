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
		}

		public bool Check(int dataFlags)
		{
			return (dataFlags & Flag) != 0;
		}
	}

	public static readonly TrackerFrameData Designation = new TrackerFrameData(TrackerFrameDataEnum.Designation);
	public static readonly TrackerFrameData Rotation = new TrackerFrameData(TrackerFrameDataEnum.Rotation);
	public static readonly TrackerFrameData Position = new TrackerFrameData(TrackerFrameDataEnum.Position);
}
