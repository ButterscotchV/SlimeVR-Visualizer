using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using static TrackerBodyPositionEnum;

public class Skeleton : MonoBehaviour
{
	public string VrConfig = "vrconfig.yml";

	private float WaistDistance;
	private float ChestDistance;
	private float LegsLength;
	private float KneeHeight;

	public float LeftScale;
	public float RightScale;

	public Material LineMaterial;

	public Gradient SpineGradient = new Gradient();
	public Gradient LegGradient = new Gradient();

	public float LineWidth = 1f;

	private LineRenderer SpineLine;
	private LineRenderer LeftLegLine;
	private LineRenderer RightLegLine;

	public float HipMix = 1f / 3f;

	[NonSerialized] public GameObject Hmd;
	[NonSerialized] public GameObject Head;
	[NonSerialized] public GameObject Neck;
	[NonSerialized] public GameObject Waist;
	[NonSerialized] public GameObject Chest;

	[NonSerialized] public GameObject LeftHip;
	[NonSerialized] public GameObject LeftKnee;
	[NonSerialized] public GameObject LeftAnkle;
	[NonSerialized] public GameObject RightHip;
	[NonSerialized] public GameObject RightKnee;
	[NonSerialized] public GameObject RightAnkle;

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

	private bool LoadConfig(string file)
	{
		try
		{
			var deserializer = new DeserializerBuilder().WithNamingConvention(new CamelCaseNamingConvention()).Build();

			using (var fileStream = File.OpenText(file))
			{
				var config = deserializer.Deserialize<dynamic>(fileStream);
				var bodyConfig = config["body"];

				Configs["Head"] = float.Parse(bodyConfig["headShift"]);
				Configs["Neck"] = float.Parse(bodyConfig["neckLength"]);
				Configs["Waist"] = float.Parse(bodyConfig["waistDistance"]);
				Configs["Chest"] = float.Parse(bodyConfig["chestDistance"]);
				Configs["Hips width"] = float.Parse(bodyConfig["hipsWidth"]);
				Configs["Knee height"] = float.Parse(bodyConfig["kneeHeight"]);
				Configs["Legs length"] = float.Parse(bodyConfig["legsLength"]);
			}
		}
		catch (Exception e)
		{
			Debug.LogException(e);
			return false;
		}

		return true;
	}

	// Start is called before the first frame update
	private void Start()
	{
		// Load config values
		if (LoadConfig(VrConfig))
		{
			Debug.Log("Loaded config bone lengths!");

			foreach (var boneLength in Configs)
			{
				Debug.Log($"{boneLength.Key}: {boneLength.Value}");
			}
		}

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

		SpineLine = Hmd.AddComponent<LineRenderer>();
		int i = 0;
		ForEachChildRecursive(Hmd.transform, (transform) =>
		{
			i++;
			return transform.childCount == 1;
		});
		SpineLine.positionCount = i;

		LeftLegLine = LeftHip.AddComponent<LineRenderer>();
		i = 0;
		ForEachChildRecursive(LeftHip.transform, (transform) =>
		{
			i++;
			return transform.childCount <= 1;
		});
		LeftLegLine.positionCount = i + 1;

		RightLegLine = RightHip.AddComponent<LineRenderer>();
		i = 0;
		ForEachChildRecursive(RightHip.transform, (transform) =>
		{
			i++;
			return transform.childCount <= 1;
		});
		RightLegLine.positionCount = i + 1;
	}

	private void ForEachChildRecursive(Transform transform, Action<Transform> action)
	{
		ForEachChildRecursive(transform, (child) => { action.Invoke(child); return true; });
	}

	private void ForEachChildRecursive(Transform transform, Func<Transform, bool> func)
	{
		if (!func.Invoke(transform))
		{
			return;
		}

		foreach (Transform child in transform)
		{
			ForEachChildRecursive(child, func);
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
		foreach (var trackerFrame in frame.TrackerFrames.Values)
		{
			if (trackerFrame.Designation == TrackerBodyPositionEnum.Hmd && trackerFrame.HasData(TrackerFrameDataEnum.Position))
			{
				Hmd.transform.position = trackerFrame.Position.Value;
			}

			if (trackerFrame.HasData(TrackerFrameDataEnum.Rotation))
			{
				GameObject node = GetNode(trackerFrame.Designation, true);
				node.transform.rotation = trackerFrame.Rotation.Value;
			}
		}
	}

	public void ApplyModifications()
	{
		AveragePelvis();
	}

	public void AveragePelvis()
	{
		// Average the pelvis with the waist rotation
		Waist.transform.rotation = Quaternion.Lerp(Quaternion.Lerp(LeftHip.transform.rotation, RightHip.transform.rotation, 0.5f), Chest.transform.rotation, HipMix);
	}

	public void ApplyFootOffset()
	{
		float leftAngle = Quaternion.Angle(LeftHip.transform.rotation, LeftKnee.transform.rotation);
		LeftKnee.transform.Rotate(0, 0, leftAngle * LeftScale);

		float rightAngle = Quaternion.Angle(RightHip.transform.rotation, RightKnee.transform.rotation);
		RightKnee.transform.Rotate(0, 0, rightAngle * RightScale);
	}

	public GameObject GetNode(TrackerBodyPosition bodyPosition, bool rotationNode = false)
	{
		switch (bodyPosition.Designation.ToLower())
		{
			case "body:hmd":
				return Hmd;
			case "body:chest":
				return rotationNode ? Neck : Chest;
			case "body:waist":
				return rotationNode ? Chest : Waist;

			case "body:left_leg":
				return rotationNode ? LeftHip : LeftKnee;
			case "body:right_leg":
				return rotationNode ? RightHip : RightKnee;

			case "body:left_ankle":
				return rotationNode ? LeftKnee : LeftAnkle;
			case "body:right_ankle":
				return rotationNode ? RightKnee : RightAnkle;
		}

		return null;
	}

	public void DrawDebug()
	{
		SpineLine.colorGradient = SpineGradient;
		SpineLine.widthMultiplier = LineWidth;
		SpineLine.sharedMaterial = LineMaterial;

		LeftLegLine.colorGradient = LegGradient;
		LeftLegLine.widthMultiplier = LineWidth;
		LeftLegLine.sharedMaterial = LineMaterial;

		RightLegLine.colorGradient = LegGradient;
		RightLegLine.widthMultiplier = LineWidth;
		RightLegLine.sharedMaterial = LineMaterial;

		int i = 0;
		ForEachChildRecursive(Hmd.transform, (transform) =>
		{
			SpineLine.SetPosition(i++, transform.position);
			return transform.childCount == 1;
		});

		i = 0;
		LeftLegLine.SetPosition(i++, Waist.transform.position);
		ForEachChildRecursive(LeftHip.transform, (transform) =>
		{
			LeftLegLine.SetPosition(i++, transform.position);
			return transform.childCount <= 1;
		});

		i = 0;
		RightLegLine.SetPosition(i++, Waist.transform.position);
		ForEachChildRecursive(RightHip.transform, (transform) =>
		{
			RightLegLine.SetPosition(i++, transform.position);
			return transform.childCount <= 1;
		});
	}
}
