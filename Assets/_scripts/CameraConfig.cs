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
public class Control
{
    [Tooltip("True to check a mouse button instead of a key.")]
    public bool useMouse;
    [Tooltip("The button to check if `Use Mouse` is true.")]
    public int button;
    [Tooltip("The key to check if `Use Mouse` is not true.")]
    public KeyCode key;

    /// <summary>
    /// Determine if the control is activated by its button or key being 
    /// pressed.  If `useMouse` is set to true, `button` will be checked as a
    /// mouse button. Otherwise `key` will be check as a keyboard key.
    /// </summary>
    /// <returns>
    /// True if the key or button is pressed. False otherwise.
    /// </returns>
    public bool IsPressed()
    {
        return useMouse ?
            Input.GetMouseButton(button)
            : Input.GetKey(key);
    }
}

[System.Serializable]
public class MouseLockConfig
{
    [Tooltip("True to auto lock the cursor when bound by the camera.")]
    public bool doLock = false;
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
    [Tooltip("True to have the mouse auto lock to this target when bound to.")]
    public MouseLockConfig mouseAutoLockConfig;

}
