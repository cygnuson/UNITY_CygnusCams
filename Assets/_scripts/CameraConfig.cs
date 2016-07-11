using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class Bool3
{
    public bool x;
    public bool y;
    public bool z;
}


[System.Serializable]
public class CameraConfig {
    [Tooltip("The onscreen text for debugging.")]
    public Text onScreenDebugText;
    [Tooltip("Include the rotation info for the unscreen debugging.")]
    public bool onScreenDebugRotation = false;

    [Tooltip("Invert the direction when scrolling to change distance.")]
    public bool invertDistanceScroll = true;
    [Tooltip("Start the program with the camera bound to the target.")]
    public bool bindToTarget = true;

}
