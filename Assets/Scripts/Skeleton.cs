using System;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour
{
	[SerializeField]
	private float WaistDistance = 0.85f;
	[SerializeField]
	private float ChestDistance = 0.42f;
	[SerializeField]
	private float LegsLength = 0.84f;
	[SerializeField]
	private float KneeHeight = 0.42f;

	public GameObject Hmd;
	public GameObject Head;
	public GameObject Neck;
	public GameObject Waist;
	public GameObject Chest;

	public GameObject LeftHip;
	public GameObject LeftKnee;
	public GameObject LeftAnkle;
	public GameObject RightHip;
	public GameObject RightKnee;
	public GameObject RightAnkle;

	public Dictionary<string, Transform> Nodes = new Dictionary<string, Transform>(11);
	public Dictionary<string, float> Configs = new Dictionary<string, float>()
	{
		{ "Head", 0.1f },
		{ "Neck", 0.1f },
		{ "Waist", 0.85f },
		{ "Chest", 0.42f },
		{ "Hips width", 0.3f },
		{ "Knee height", 0.42f },
		{ "Legs length", 0.84f }
	};

	// Start is called before the first frame update
	private void Start()
    {
		Hmd = new GameObject("HMD");
		Head = new GameObject("Head");
		Neck = new GameObject("Neck");
		Waist = new GameObject("Waist");
		Chest = new GameObject("Chest");

		LeftHip = new GameObject("Left-Hip");
		LeftKnee = new GameObject("Left-Knee");
		LeftAnkle = new GameObject("Left-Ankle");
		RightHip = new GameObject("Right-Hip");
		RightKnee = new GameObject("Right-Knee");
		RightAnkle = new GameObject("Right-Ankle");

		Hmd.transform.parent = transform;

		Head.transform.parent = Hmd.transform;
		Neck.transform.parent = Head.transform;
		Chest.transform.parent = Neck.transform;
		Waist.transform.parent = Chest.transform;

		LeftHip.transform.parent = Waist.transform;
		RightHip.transform.parent = Waist.transform;

		LeftKnee.transform.parent = LeftHip.transform;
		RightKnee.transform.parent = RightHip.transform;

		LeftAnkle.transform.parent = LeftKnee.transform;
		RightAnkle.transform.parent = RightKnee.transform;

		ForEachChildRecursive(Hmd.transform, (node) => { Nodes.Add(node.name, node); });

		foreach (var config in Configs)
		{
			SetSkeletonConfig(config.Key, config.Value);
		}
    }

	private void ForEachChildRecursive(Transform transform, Action<Transform> action)
	{
		action.Invoke(transform);

		foreach (Transform node in transform)
		{
			ForEachChildRecursive(node, action);
		}
	}

	public void SetSkeletonConfig(string bone, float length)
	{
		switch (bone.ToLower())
		{
			case "head":
				Head.transform.localPosition = new Vector3(0, 0, length);
				break;
			case "neck":
				Neck.transform.localPosition = new Vector3(0, -length, 0);
				break;
			case "waist":
				WaistDistance = length;
				Waist.transform.localPosition = new Vector3(0, -(length - ChestDistance), 0);
				break;
			case "chest":
				ChestDistance = length;
				Chest.transform.localPosition = new Vector3(0, -length, 0);
				Waist.transform.localPosition = new Vector3(0, -(WaistDistance - length), 0);
				break;
			case "hips width":
				LeftHip.transform.localPosition = new Vector3(-length / 2f, 0, 0);
				RightHip.transform.localPosition = new Vector3(length / 2f, 0, 0);
				break;
			case "knee height":
				KneeHeight = length;
				LeftAnkle.transform.localPosition = new Vector3(0, -length, 0);
				RightAnkle.transform.localPosition = new Vector3(0, -length, 0);
				LeftKnee.transform.localPosition = new Vector3(0, -(LegsLength - length), 0);
				RightKnee.transform.localPosition = new Vector3(0, -(LegsLength - length), 0);
				break;
			case "legs length":
				LegsLength = length;
				LeftKnee.transform.localPosition = new Vector3(0, -(length - KneeHeight), 0);
				RightKnee.transform.localPosition = new Vector3(0, -(length - KneeHeight), 0);
				break;
		}
	}

	public void SetPoseFromFrame(PoseFrame frame)
	{
		Hmd.transform.localPosition = frame.RootPos;

		foreach (var rot in frame.Rotations)
		{
			if (Nodes.TryGetValue(rot.Key, out var transform))
			{
				transform.rotation = rot.Value;
			}
		}
	}

	public void DrawDebug()
	{
		Debug.DrawLine(Hmd.transform.position, Head.transform.position);
		Debug.DrawLine(Head.transform.position, Neck.transform.position);
		Debug.DrawLine(Neck.transform.position, Chest.transform.position);
		Debug.DrawLine(Chest.transform.position, Waist.transform.position);

		Debug.DrawLine(Waist.transform.position, LeftHip.transform.position);
		Debug.DrawLine(Waist.transform.position, RightHip.transform.position);

		Debug.DrawLine(LeftHip.transform.position, LeftKnee.transform.position);
		Debug.DrawLine(RightHip.transform.position, RightKnee.transform.position);

		Debug.DrawLine(LeftKnee.transform.position, LeftAnkle.transform.position);
		Debug.DrawLine(RightKnee.transform.position, RightAnkle.transform.position);
	}
}
