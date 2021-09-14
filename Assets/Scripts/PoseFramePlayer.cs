using UnityEngine;

[RequireComponent(typeof(Skeleton))]
public class PoseFramePlayer : MonoBehaviour
{
	public string File = "C:/Users/Dankrushen/Documents/SlimeVR AutoBone/Butterscotch!/ABRecording1.abf";
	private string _loadedFile;

	public PoseFrame Frames;
	private TrackerFrame[] FrameBuffer;

	public bool Play = true;
	public bool Loop = true;

	[Min(float.Epsilon)]
	public float Interval = 20f / 1000f; // 20 ms
	private float NextFrameTime = 0f;

	[Min(0)]
	public int Cursor = 0;
	private int curCursor = 0;

	private Skeleton _skeleton;

	private void LoadFile(string file)
	{
		PoseFrame frames = PoseFrameIO.ReadFromFile(file);
		_loadedFile = File;

		if (frames != null && frames.GetTrackerCount() > 0)
		{
			Frames = frames;
			FrameBuffer = new TrackerFrame[frames.GetTrackerCount()];
			Debug.Log($"{frames.GetMaxFrameCount()} frames loaded!");
			Debug.Log($"Loaded trackers: {string.Join(", ", frames.GetTrackers())}");
		}
		else
		{
			Debug.LogError("Unable to load frames...");
		}
	}

	private void Start()
	{
		_skeleton = GetComponent<Skeleton>();

		LoadFile(File);
	}

	private void Update()
	{
		_skeleton.DrawDebug();

		if (!Play || Time.realtimeSinceStartup >= NextFrameTime)
		{
			if (File != _loadedFile)
			{
				LoadFile(File);
				Cursor = 0;
			}

			PoseFrame frames = Frames;

			if (frames != null && frames.GetTrackerCount() > 0)
			{
				if (Cursor >= 0 && Cursor < frames.GetMaxFrameCount())
				{
					NextFrameTime = Time.realtimeSinceStartup + Interval;
					if (Play || curCursor != Cursor)
					{
						curCursor = Play ? Cursor++ : Cursor;
						frames.GetFrames(curCursor, FrameBuffer);
						_skeleton.SetPoseFromFrame(FrameBuffer);
						_skeleton.ApplyModifications();

						Vector3 left = _skeleton.LeftAnkle.transform.position;
						Vector3 right = _skeleton.RightAnkle.transform.position;
						Debug.Log($"Foot positions:\nOffset {(right - left).ToString("F4")}    Left {left.ToString("F4")}    Right {right.ToString("F4")}");
					}
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
