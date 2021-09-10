using System.Collections.Generic;
using UnityEngine;

public class PoseFrame
{
	public readonly Vector3 RootPos;
	public readonly Dictionary<string, Quaternion> Rotations;
	public readonly Dictionary<string, Vector3> Positions;

	public PoseFrame(Vector3 rootPos, Dictionary<string, Quaternion> rotations, Dictionary<string, Vector3> positions)
	{
		this.RootPos = rootPos;
		this.Rotations = rotations;
        this.Positions = positions;
    }

	public PoseFrame(GameObject root)
	{
		// Copy headset position
		this.RootPos = root.transform.position;

		// Copy all rotations
		this.Rotations = new Dictionary<string, Quaternion>();
		foreach (Transform node in root.transform)
		{
			Rotations.Add(node.name, node.transform.rotation);
		}

		this.Positions = null;
	}
}
