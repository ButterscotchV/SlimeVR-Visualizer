using UnityEngine;

public class Test : MonoBehaviour
{
	public PoseFrame[] frames;

    // Start is called before the first frame update
    public void Start()
    {
		frames = PoseFrameIO.ReadFromFile("C:/Users/Dankrushen/Documents/SlimeVR AutoBone/Butterscotch! - Copy/ABRecording1.abf");
		Debug.Log($"{frames.Length} frames loaded!");
    }
}
