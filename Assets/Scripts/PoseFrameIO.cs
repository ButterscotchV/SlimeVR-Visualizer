using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static TrackerBodyPositionEnum;

public static class PoseFrameIO
{
	public static bool WriteToFile(string file, PoseFrame[] frames)
	{
		try
		{
			using (BinaryWriter outputStream = new EndiannessAwareBinaryWriter(new BufferedStream(File.OpenRead(file)), Endianness.Big))
			{
				// Write every frame
				outputStream.Write(frames.Length);
				foreach (PoseFrame frame in frames)
				{
					WriteFrame(outputStream, frame);
				}
			}
		}
		catch (Exception e)
		{
			Debug.LogError("Error writing frames to file");
			Debug.LogException(e);
			return false;
		}

		return true;
	}

	public static bool WriteFrame(BinaryWriter outputStream, PoseFrame frame)
	{
		try
		{
			if (frame != null && frame.TrackerFrames != null)
			{
				outputStream.Write(frame.TrackerFrames.Count);

				foreach (TrackerFrame trackerFrame in frame.TrackerFrames.Values)
				{
					outputStream.Write(trackerFrame.DataFlags);

					if (trackerFrame.HasData(TrackerFrameDataEnum.Designation))
					{
						outputStream.Write(trackerFrame.Designation.Designation);
					}

					if (trackerFrame.HasData(TrackerFrameDataEnum.Rotation))
					{
						Quaternion quat = trackerFrame.Rotation.Value;
						outputStream.Write(quat.x);
						outputStream.Write(quat.y);
						outputStream.Write(quat.z);
						outputStream.Write(quat.w);
					}

					if (trackerFrame.HasData(TrackerFrameDataEnum.Position))
					{
						Vector3 vec = trackerFrame.Position.Value;
						outputStream.Write(vec.x);
						outputStream.Write(vec.y);
						outputStream.Write(vec.z);
					}
				}
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

	public static bool WriteFrame(string file, PoseFrame frame)
	{
		try
		{
			using (BinaryWriter outputStream = new EndiannessAwareBinaryWriter(new BufferedStream(File.OpenRead(file)), Endianness.Big))
			{
				WriteFrame(outputStream, frame);
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

	public static PoseFrame[] ReadFromFile(string file)
	{
		try
		{
			using (BinaryReader inputStream = new EndiannessAwareBinaryReader(new BufferedStream(File.OpenRead(file)), Endianness.Big))
			{
				int frameCount = inputStream.ReadInt32();

				PoseFrame[] frames = new PoseFrame[frameCount];
				for (int i = 0; i < frameCount; i++)
				{
					frames[i] = ReadFrame(inputStream);
				}

				return frames;
			}
		}
		catch (Exception e)
		{
			Debug.LogError("Error reading frames from file");
			Debug.LogException(e);
		}

		return null;
	}

	public static PoseFrame ReadFrame(BinaryReader inputStream)
	{
		try
		{
			int trackerFrameCount = inputStream.ReadInt32();

			Dictionary<TrackerBodyPosition, TrackerFrame> trackerFrames = new Dictionary<TrackerBodyPosition, TrackerFrame>(trackerFrameCount);
			for (int i = 0; i < trackerFrameCount; i++)
			{
				int dataFlags = inputStream.ReadInt32();

				TrackerBodyPosition designation = null;
				if (TrackerFrameDataEnum.Designation.Check(dataFlags))
				{
					designation = TrackerBodyPositionEnum.GetByDesignation(inputStream.ReadString());
				}

				Quaternion? rotation = null;
				if (TrackerFrameDataEnum.Rotation.Check(dataFlags))
				{
					float quatX = inputStream.ReadSingle();
					float quatY = inputStream.ReadSingle();
					float quatZ = inputStream.ReadSingle();
					float quatW = inputStream.ReadSingle();
					rotation = new Quaternion(quatX, quatY, quatZ, quatW);
				}

				Vector3? position = null;
				if (TrackerFrameDataEnum.Position.Check(dataFlags))
				{
					float posX = inputStream.ReadSingle();
					float posY = inputStream.ReadSingle();
					float posZ = inputStream.ReadSingle();
					position = new Vector3(posX, posY, posZ);
				}

				if (designation != null)
				{
					trackerFrames[designation] = new TrackerFrame(designation, rotation, position);
				}
			}

			return new PoseFrame(trackerFrames);
		}
		catch (Exception e)
		{
			Debug.LogError("Error reading frame from stream");
			Debug.LogException(e);
		}

		return null;
	}

	public static PoseFrame ReadFrame(string file)
	{
		try
		{
			return ReadFrame(new EndiannessAwareBinaryReader(new BufferedStream(File.OpenRead(file)), Endianness.Big));
		}
		catch (Exception e)
		{
			Debug.LogError("Error reading frame from file");
			Debug.LogException(e);
		}

		return null;
	}
}
