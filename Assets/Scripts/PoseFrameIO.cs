using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static TrackerBodyPositionValues;

public static class PoseFrameIO
{
	public static bool WriteFrames(BinaryWriter outputStream, PoseFrame frames)
	{
		try
		{
			if (frames != null)
			{

				outputStream.Write(frames.GetTrackerCount());
				foreach (var tracker in frames.GetTrackers())
				{

					outputStream.Write(tracker.Name);
					outputStream.Write(tracker.GetFrameCount());
					foreach (TrackerFrame trackerFrame in tracker)
					{
						outputStream.Write(trackerFrame.DataFlags);

						if (trackerFrame.HasData(TrackerFrameDataValues.Designation))
						{
							outputStream.Write(trackerFrame.Designation.Designation);
						}

						if (trackerFrame.HasData(TrackerFrameDataValues.Rotation))
						{
							Quaternion quat = trackerFrame.Rotation.Value;
							outputStream.Write(quat.x);
							outputStream.Write(quat.y);
							outputStream.Write(quat.z);
							outputStream.Write(quat.w);
						}

						if (trackerFrame.HasData(TrackerFrameDataValues.Position))
						{
							Vector3 vec = trackerFrame.Position.Value;
							outputStream.Write(vec.x);
							outputStream.Write(vec.y);
							outputStream.Write(vec.z);
						}
					}
				}
			}
			else
			{
				outputStream.Write(0);
			}
		}
		catch (Exception e)
		{
			Debug.LogError("Error writing frame to stream");
			Debug.LogException(e);
			return false;
		}

		return true;
	}

	public static bool WriteToFile(string file, PoseFrame frame)
	{
		try
		{
			using (BinaryWriter outputStream = new EndiannessAwareBinaryWriter(new BufferedStream(File.OpenRead(file)), Endianness.Big))
			{
				WriteFrames(outputStream, frame);
			}
		}
		catch (Exception e)
		{
			Debug.LogError("Error writing frame to file");
			Debug.LogException(e);
			return false;
		}

		return true;
	}

	public static PoseFrame ReadFrames(BinaryReader inputStream)
	{
		try
		{

			int trackerCount = inputStream.ReadInt32();
			List<PoseFrameTracker> trackers = new List<PoseFrameTracker>(trackerCount);
			for (int i = 0; i < trackerCount; i++)
			{

				string name = inputStream.ReadString();
				int trackerFrameCount = inputStream.ReadInt32();
				List<TrackerFrame> trackerFrames = new List<TrackerFrame>(trackerFrameCount);
				for (int j = 0; j < trackerFrameCount; j++)
				{
					int dataFlags = inputStream.ReadInt32();

					TrackerBodyPosition designation = null;
					if (TrackerFrameDataValues.Designation.Check(dataFlags))
					{
						designation = TrackerBodyPositionValues.GetByDesignation(inputStream.ReadString());
					}

					Quaternion? rotation = null;
					if (TrackerFrameDataValues.Rotation.Check(dataFlags))
					{
						float quatX = inputStream.ReadSingle();
						float quatY = inputStream.ReadSingle();
						float quatZ = inputStream.ReadSingle();
						float quatW = inputStream.ReadSingle();
						rotation = new Quaternion(quatX, quatY, quatZ, quatW);
					}

					Vector3? position = null;
					if (TrackerFrameDataValues.Position.Check(dataFlags))
					{
						float posX = inputStream.ReadSingle();
						float posY = inputStream.ReadSingle();
						float posZ = inputStream.ReadSingle();
						position = new Vector3(posX, posY, posZ);
					}

					trackerFrames.Add(new TrackerFrame(designation, rotation, position));
				}

				trackers.Add(new PoseFrameTracker(name, trackerFrames));
			}

			return new PoseFrame(trackers);
		}
		catch (Exception e)
		{
			Debug.LogError("Error reading frame from stream");
			Debug.LogException(e);
		}

		return null;
	}

	public static PoseFrame ReadFromFile(string file)
	{
		try
		{
			return ReadFrames(new EndiannessAwareBinaryReader(new BufferedStream(File.OpenRead(file)), Endianness.Big));
		}
		catch (Exception e)
		{
			Debug.LogError("Error reading frame from file");
			Debug.LogException(e);
		}

		return null;
	}
}
