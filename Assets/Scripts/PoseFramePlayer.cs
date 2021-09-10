using UnityEngine;

[RequireComponent(typeof(Skeleton))]
public class PoseFramePlayer : MonoBehaviour
{
	public string File = "C:/Users/Dankrushen/Documents/SlimeVR AutoBone/Butterscotch! - Copy/ABRecording1.abf";
	private string _loadedFile;

	public PoseFrame[] Frames;

	public bool Play = true;
	public bool Loop = true;

	[Min(1)]
	public float Interval = 20f / 1000f; // 20 ms
	private float NextFrameTime = 0f;

	[Min(0)]
	public int Cursor = 0;

	private Skeleton _skeleton;

	private void LoadFile(string file)
	{
		PoseFrame[] frames = PoseFrameIO.ReadFromFile(file);
		_loadedFile = File;

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
