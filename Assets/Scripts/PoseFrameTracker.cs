using System;
using System.Collections;
using System.Collections.Generic;

public class PoseFrameTracker : IEnumerable<TrackerFrame>
{
	public readonly string Name;

	private readonly List<TrackerFrame> Frames;

	public PoseFrameTracker(string name, List<TrackerFrame> frames)
	{
		if (frames == null)
		{
			throw new ArgumentException($"{nameof(frames)} must not be null", nameof(frames));
		}

		Name = name ?? "";
		Frames = frames;
	}

	public int GetFrameCount()
	{
		return Frames.Count;
	}

	public TrackerFrame GetFrame(int index)
	{
		return Frames[index];
	}

	public TrackerFrame SafeGetFrame(int index)
	{
		try
		{
			return Frames[index];
		}
		catch (Exception)
		{
			return null;
		}
	}

	public override string ToString()
	{
		return Name;
	}

	public IEnumerator<TrackerFrame> GetEnumerator()
	{
		return Frames.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
