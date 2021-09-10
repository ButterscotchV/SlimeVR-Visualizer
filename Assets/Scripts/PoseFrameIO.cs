using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

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
			// Write root position vector
			outputStream.Write(frame.RootPos.x);
			outputStream.Write(frame.RootPos.y);
			outputStream.Write(frame.RootPos.z);

			if (frame.Rotations != null)
			{
				// Write rotations
				outputStream.Write(frame.Rotations.Count);
				foreach (KeyValuePair<String, Quaternion> entry in frame.Rotations)
				{
					// Write the label string
					outputStream.Write(entry.Key);

					// Write the rotation quaternion
					Quaternion quat = entry.Value;
					outputStream.Write(quat.x);
					outputStream.Write(quat.y);
					outputStream.Write(quat.z);
					outputStream.Write(quat.w);
				}
			}
			else
			{
				outputStream.Write(0);
			}

			if (frame.Positions != null)
			{
				// Write positions
				outputStream.Write(frame.Positions.Count);
				foreach (KeyValuePair<String, Vector3> entry in frame.Positions)
				{
					// Write the label string
					outputStream.Write(entry.Key);

					// Write the rotation quaternion
					Vector3 vec = entry.Value;
					outputStream.Write(vec.x);
					outputStream.Write(vec.y);
					outputStream.Write(vec.z);
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
			float vecX = inputStream.ReadSingle();
			float vecY = inputStream.ReadSingle();
			float vecZ = inputStream.ReadSingle();

			Vector3 vector = new Vector3(vecX, vecY, vecZ);

			int rotationCount = inputStream.ReadInt32();
			Dictionary<String, Quaternion> rotations = null;
			if (rotationCount > 0)
			{
				rotations = new Dictionary<String, Quaternion>(rotationCount);
				for (int j = 0; j < rotationCount; j++)
				{
					String label = inputStream.ReadString();

					float quatX = inputStream.ReadSingle();
					float quatY = inputStream.ReadSingle();
					float quatZ = inputStream.ReadSingle();
					float quatW = inputStream.ReadSingle();
					Quaternion quaternion = new Quaternion(quatX, quatY, quatZ, quatW);

					rotations.Add(label, quaternion);
				}
			}

			int positionCount = inputStream.ReadInt32();
			Dictionary<String, Vector3> positions = null;
			if (positionCount > 0)
			{
				positions = new Dictionary<String, Vector3>(positionCount);
				for (int j = 0; j < positionCount; j++)
				{
					String label = inputStream.ReadString();

					float posX = inputStream.ReadSingle();
					float posY = inputStream.ReadSingle();
					float posZ = inputStream.ReadSingle();
					Vector3 position = new Vector3(posX, posY, posZ);

					positions.Add(label, position);
				}
			}

			return new PoseFrame(vector, rotations, positions);
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
