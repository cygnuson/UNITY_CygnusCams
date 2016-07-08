using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/*
 * TODO
 * 
 * test the spherical vector.
 * shorted the fixedupdate by making more functions.
 * 
 * 
 * */


/// <summary>
/// Configuration setting for a change to free roam mode.
/// </summary>
[System.Serializable]
public class FreeRoamConfig
{
    [Tooltip("The button to move forward.")]
    public Control forward;
    [Tooltip("The button to move backward.")]
    public Control backward;
    [Tooltip("The button to move left.")]
    public Control left;
    [Tooltip("The button to move right.")]
    public Control right;
    [Tooltip("The button to move up.")]
    public Control up;
    [Tooltip("The button to move down.")]
    public Control down;
    [Tooltip("The speed to move when free roaming.")]
    public float freeRoamSpeed = 10;
    [Tooltip("The mouse setting for when in free roam mode.")]
    public MouseLockConfig mouse;
}

/// <summary>
/// The CameraController is intended to be attached to a camera. It can be
/// bound or unbound from its target. The target must have an object with 
/// `CameraTarget` attached to it.  The config setting will all come from the
/// attached target except the free roam setting.
/// 
/// </summary>
public class CameraController : MonoBehaviour
{
    [Tooltip("The target that the camera will be bound to or unbound from.")]
    public CameraTarget target;
    [Tooltip("The configureation for the camera when the free roam option is "
        + "enabled. That will happen when the camera is unbound from its"
        + "target.")]
    public FreeRoamConfig freeRoamConfig;
    
    /*The config will referece the config in the target.*/
    private CameraConfig config;
    /*Keep track of wether the mouse is locked or not.*/
    private bool mouseLocked;

    private float rad;
    private float roll = 0;
    private float theta = 1;
    private float phi = 10;
    private float speedMod = 0.1f;
    private Vector3 freeRoamTarget;
    private Text onScreenDebugText;

    /// <summary>
    /// Toggel the lock state of the mouse. If its locked, it will become 
    /// unlocked and vice versa.
    /// </summary>
    public void ToggleMouse()
    {
        if (mouseLocked)
        {
            UnlockMouse();
        }
        else
        {
            LockMouse();
        }
    }

    /// <summary>
    /// Lock the mouse to the center of the screen and hide it.
    /// </summary>
    public void LockMouse()
    {
        if (!mouseLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            mouseLocked = true;
        }
        else
        {

        }
    }

    /// <summary>
    /// Force the mouse to be unlocked from the center of the screen and shown.
    /// </summary>
    public void UnlockMouse()
    {
        if (mouseLocked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            mouseLocked = false;
        }
        else
        {

        }

    }

    /// <summary>
    /// Unbind the camera to its target.
    /// </summary>
    public void UnbindFromTarget()
    {
        if (config.bindToTarget)
        {
            freeRoamTarget = target.transform.position;
            config.bindToTarget = false;
            if (!freeRoamConfig.mouse.doLock)
            {
                UnlockMouse();
            }
            else
            {
                LockMouse();
            }
        }
        else
        {
            //alerady unbound.
        }
    }

    /// <summary>
    /// Bind the camera to its target.
    /// </summary>
    public void BindToTarget()
    {
        if (!config.bindToTarget)
        {
            config.bindToTarget = true;
            if (config.mouseAutoLockConfig.doLock)
            {
                LockMouse();
            }
            else
            {
                UnlockMouse();
            }
        }
        else
        {
            //alerady bound.
        }
    }

    void Start()
    {
        config = target.config;
        if (config.mouseAutoLockConfig.doLock)
        {
            LockMouse();
        }
        else
        {
            UnlockMouse();
        }

        if (config.onScreenDebugText != null)
        {
            onScreenDebugText = config.onScreenDebugText;
        }
        rad = config.defaultDistance;
        Debug.Log("Target is at: " + target.transform.position.ToString());
    }

    void FixedUpdate()
    {
        //////////////////////DEBUGGING KEYS///////////////////////////////////
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            ToggleMouse();
        }
        if (Input.GetKeyUp(KeyCode.BackQuote))
        {
            if (config.bindToTarget)
            {
                UnbindFromTarget();
            }
            else
            {
                BindToTarget();
            }
        }
        //////////////////////////////////////////////////////////////////////


        float delta = Time.deltaTime;
        /*The target position changes base on wether or not the camera is bound
         to its target.*/
        Vector3 targetPosition = config.bindToTarget ?
            target.transform.position
            : freeRoamTarget;
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        /*Free roam movement actions.*/
        if (!config.bindToTarget && Input.anyKey)
        {
            float speed = freeRoamConfig.freeRoamSpeed;
            var frKeys = freeRoamConfig;
            if (frKeys.forward.IsPressed())
            {
                freeRoamTarget += transform.forward * delta * speed;
            }
            if (frKeys.backward.IsPressed())
            {
                freeRoamTarget -= transform.forward * delta * speed;
            }
            if (frKeys.left.IsPressed())
            {
                freeRoamTarget -= transform.right * delta * speed;
            }
            if (frKeys.right.IsPressed())
            {
                freeRoamTarget += transform.right * delta * speed;
            }
            if (frKeys.up.IsPressed())
            {
                freeRoamTarget += transform.up * delta * speed;
            }
            if (frKeys.down.IsPressed())
            {
                freeRoamTarget -= transform.up * delta * speed;
            }
        }
        /*choose wether a rotate should happen.*/
        bool doRotate = config.rotateCameraControl.useMouse ?
            Input.GetMouseButton(config.rotateCameraControl.button)
            : Input.GetKey(config.rotateCameraControl.key);

        if (doRotate)
        {
            bool doRoll = config.zAxisControl.IsPressed();
            if (!config.lockRotation.z && doRoll)
            {
                roll += config.lockRotation.z ? 0
                    : ((-(mouseX) * config.rotationSpeed.z * speedMod));
                /*Make sure the roll value does not go to high by cycling it.*/
                while (roll > 2 * Mathf.PI)
                {
                    roll -= 2 * Mathf.PI;
                }
                while (roll < 0)
                {
                    roll += 2 * Mathf.PI;
                }
            }
            else
            {
                /*The next two statements convert out regular XY mouse 
                 * cordinates to a a rotated axis that will allow proper 
                 * rotation.*/
                float x = mouseX * Mathf.Cos(roll)
                    - mouseY * Mathf.Sin(roll);
                float y = mouseX * Mathf.Sin(roll)
                    + mouseY * Mathf.Cos(roll);
                theta += config.lockRotation.x ? 0
                    : ((y) * config.rotationSpeed.x * speedMod);
                phi += config.lockRotation.y ? 0
                    : ((x) * config.rotationSpeed.y * speedMod);
                /*keep theta between 0 and pi.*/
                theta = Mathf.Clamp(theta, 0.001f, Mathf.PI - 0.001f);
            }
        }
        /*zoom the camera outward.*/
        rad += (config.invertDistanceScroll ? -1 : 1)
            * Input.mouseScrollDelta.y;
        /*Keep phi from getting out of control with large numbers.*/
        while (phi > 2 * Mathf.PI)
        {
            phi -= 2 * Mathf.PI;
        }
        while (phi < 0)
        {
            phi += 2 * Mathf.PI;
        }
        /*Calculate the x y and z spherical cordinates so that there is a 
         * smooth rotation*/
        float localZ = rad * Mathf.Sin(theta) * Mathf.Cos(phi);
        float localX = rad * Mathf.Sin(theta) * Mathf.Sin(phi);
        float localY = rad * Mathf.Cos(theta);
        /*Set the camera to have to correct position.*/
        transform.position = new Vector3(localX, localY, localZ);
        transform.position += targetPosition;

        transform.LookAt(targetPosition);
        /*Do a barrel roll.*/
        transform.Rotate(0, 0, roll * Mathf.Rad2Deg);
        if (onScreenDebugText != null)
        {
            if (config.onScreenDebugRotation)
            {
                /*Show some debugging information.*/
                onScreenDebugText.text = "theta: " + theta
                               + "\nphi: " + phi
                               + "\nroll: " + roll
                               + "\nDistance: " + rad
                               + "\nX-Rotation: "
                               + transform.rotation.eulerAngles.x
                               + "\nY-Rotation: "
                               + transform.rotation.eulerAngles.y
                               + "\nZ-Rotation: "
                               + transform.rotation.eulerAngles.z;
            }

        }
    }
}
