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
    [Tooltip("The default distance of the camera to the target.")]
    public float defaultDistance;
    [Tooltip("the default setting of rotation on loading.")]
    public Vector3 defaultRotation;
    [Tooltip("Lock the rotation so that it cannot move on that axis.")]
    public Bool3 lockRotation;
    [Tooltip("Invert the direction when scrolling to change distance.")]
    public bool invertDistanceScroll = true;
    [Tooltip("Start the program with the camera bound to the target.")]
    public bool bindToTarget = true;
    [Tooltip("The speed that the rotation happens.")]
    public Vector3 rotationSpeed = new Vector3(10, 10, 10);
    [Tooltip("The button that will switch over to the rolling mode.")]
    public Control zAxisControl;
    [Tooltip("The button that will engage the rotation of the camera.")]
    public Control rotateCameraControl;

}
