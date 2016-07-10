using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/*
 * TODO
 * 
 * shorted the fixedupdate by making more functions.
 * options to lerp the rotation and movement.
 * Abstract Camera holds the target location
 * Abstract Camera holds the rotation.
 * Only key presses and setups happend in CameraController, 
 *      everything else is in the camera object.
 * Lock things properly.
 * 
 * filters that will trigger a delegate when recievign a certian value.
 * 
 * Free Roam stuff should go in a special FreeRoamCamera that contains a spherical/cyl/cart camera.
 * 
 * finish cylindrical cordinates for the new Component system.
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

    private SphericalCamera mainCamera;

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
            /*if (!freeRoamConfig.mouse.doLock)
            {
                UnlockMouse();
            }
            else
            {
                LockMouse();
            }*/
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
            /*if (config.mouseAutoLockConfig.doLock)
            {
                LockMouse();
            }
            else
            {
                UnlockMouse();
            }*/
        }
        else
        {
            //alerady bound.
        }
    }

    void Start()
    {
        config = target.config;
        /* if (config.mouseAutoLockConfig.doLock)
         {
             LockMouse();
         }
         else
         {
             UnlockMouse();
         }*/

        if (config.onScreenDebugText != null)
        {
            onScreenDebugText = config.onScreenDebugText;
        }
        mainCamera = new SphericalCamera(
            config.defaultDistance, 0, 10, 10, 1, 0);

        mainCamera.sphericalVector.theta.AddFilter(
            new Filters.ClampFilter(0.001f, Mathf.PI - 0.001f),
            Filters.Type.Get);

        mainCamera.sphericalVector.phi.AddFilter(
            new Filters.RepeatFilter(2 * Mathf.PI), Filters.Type.Get);

        mainCamera.roll.AddFilter(
            new Filters.RepeatFilter(2 * Mathf.PI), Filters.Type.Get);
        mainCamera.roll.AddFilter(
            new Filters.InversionFilter(), Filters.Type.Set);

        mainCamera.sphericalVector.radius.AddFilter(
            new Filters.LowerBoundFilter(2), Filters.Type.Get);

        if (config.invertDistanceScroll)
        {
            mainCamera.sphericalVector.radius.AddFilter(
                new Filters.InversionFilter(), Filters.Type.Set);
        }
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
                mainCamera.Spin(0,0,config.lockRotation.z ? 0: mouseX);
            }
            else
            {
                /*the adjusted vector is to translate the bare xy click to a
                 tilted vector so that the rotation is properly handled due
                 to the roll applied to the camera.*/
                var adjusted = TiltVector.CreateTiltedVector(
                    new Vector2(mouseX, mouseY),mainCamera.roll.value);
                mainCamera.Spin(config.lockRotation.y ? 0 : adjusted.x,
                    config.lockRotation.x ? 0 : adjusted.y, 0);
            }
        }
        /*zoom the camera outward.*/
        if (Input.mouseScrollDelta.y != 0)
            mainCamera.sphericalVector.radius += Input.mouseScrollDelta.y;
        /*Set the camera to have to correct position.*/
        transform.position = mainCamera.GetLocation();
        transform.position += targetPosition;

        transform.LookAt(targetPosition);
        /*Do a barrel roll.*/
        transform.Rotate(0, 0, mainCamera.roll.value * Mathf.Rad2Deg);
        if (onScreenDebugText != null)
        {
            if (config.onScreenDebugRotation)
            {
                /*Show some debugging information.*/
                onScreenDebugText.text = "theta: " + mainCamera.sphericalVector.theta.value
                               + "\nphi: " + mainCamera.sphericalVector.phi.value
                               + "\nroll: " + mainCamera.roll.value
                               + "\nDistance: " + mainCamera.sphericalVector.radius.value
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
