public class TrackerFrameData
{
	public static readonly TrackerFrameData Designation = new TrackerFrameData(0);
	public static readonly TrackerFrameData Rotation = new TrackerFrameData(1);
	public static readonly TrackerFrameData Position = new TrackerFrameData(2);

	public readonly int Flag;

	private TrackerFrameData(int id)
	{
		Flag = 1 << id;
	}

	public bool Check(int dataFlags)
	{
		return (dataFlags & Flag) != 0;
	}
}
