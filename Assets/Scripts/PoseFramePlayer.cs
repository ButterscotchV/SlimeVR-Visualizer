using UnityEngine;

[RequireComponent(typeof(Skeleton))]
public class PoseFramePlayer : MonoBehaviour
{
	public PoseFrame[] Frames;

	public bool Play = true;
	public bool Loop = true;

	[Min(1)]
	public float Interval = 20f / 1000f; // 20 ms
	private float NextFrameTime = 0f;

	[Min(0)]
	public int Cursor = 0;

	private Skeleton _skeleton;

	private void Start()
	{
		_skeleton = GetComponent<Skeleton>();

		PoseFrame[] frames = PoseFrameIO.ReadFromFile("C:/Users/Dankrushen/Documents/SlimeVR AutoBone/Butterscotch! - Copy/ABRecording1.abf");

		if (frames != null && frames.Length > 0)
		{
			Frames = frames;
			Debug.Log($"{frames.Length} frames loaded!");
		}
		else
		{
			Debug.LogError("Unable to load frames...");
		}
	}

	private void Update()
	{
		_skeleton.DrawDebug();

		if (!Play || Time.realtimeSinceStartup >= NextFrameTime)
		{
			PoseFrame[] frames = Frames;

			if (frames != null && frames.Length > 0)
			{
				if (Cursor >= 0 && Cursor < frames.Length)
				{
					NextFrameTime = Time.realtimeSinceStartup + Interval;
					_skeleton.SetPoseFromFrame(frames[Play ? Cursor++ : Cursor]);
				}
				else if (Loop)
				{
					Cursor = 0;
					NextFrameTime = 0;
				}
			}
		}
	}
}
