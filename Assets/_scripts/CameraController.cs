using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/*
 * TODO
 * 
 * shorted the fixedupdate by making more functions.
 * options to lerp the rotation and movement.
 * Abstract Camera holds the target location
 * Only key presses and setups happend in CameraController, 
 *      everything else is in the camera object.
 * filters that will trigger a delegate when recievign a certian value.
 * mouse locking.
 * Free Roam stuff should go in a special FreeRoamCamera that contains a spherical/cyl/cart camera.
 * 
 * finish cylindrical cordinates for the new Component system.
 * 
 * custom screen Text panel set for debugging stuff.
 * 
 * use key combos instead of single keys.  Change the Control. If it has only one key
 *      or button set to the combo, then it will have no change in ehavior compared to right now.
 *      make the roll use a combo with first click, then right click. then zoom
 *      to work with first click then scroll.
 *      
 *      
 * */


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
    //public CameraControl freeRoamConfig;

    /*The config will referece the config in the target.*/
    private CameraConfig config;
    /*Keep track of wether the mouse is locked or not.*/
    private bool mouseLocked;

    private SphericalCamera mainCamera;

    private Vector3 freeRoamTarget;

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
        mainCamera = new SphericalCamera(
            15, 0, 10, 10, 1, 0);

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
        mainCamera.sphericalVector.radius.AddFilter(
            new Filters.MultiplyFilter(10), Filters.Type.Set);

        if (config.invertDistanceScroll)
        {
            mainCamera.sphericalVector.radius.AddFilter(
                new Filters.InversionFilter(), Filters.Type.Set);
        }

        mainCamera.EnableControl(CameraControl.KeyType.Rotate, 0);
        mainCamera.EnableControl(CameraControl.KeyType.Roll, 1);
        mainCamera.
            EnableAxis(CameraControl.AxisType.ZoomAxis, "Mouse ScrollWheel");
        mainCamera.
            EnableAxis(CameraControl.AxisType.CameraLeftRight, "Mouse X");
        mainCamera.
            EnableAxis(CameraControl.AxisType.RollCamera, "Mouse X");
        mainCamera.
            EnableAxis(CameraControl.AxisType.CameraUpDown, "Mouse Y");

        mainCamera.SetDebugScreen(config.onScreenDebugText);
        mainCamera.doDebug = true;
        mainCamera.enabled = true;
        //mainCamera.EnableControl(CameraControl.KeyType.ZoomOut, KeyCode.PageDown);
        //mainCamera.EnableControl(CameraControl.KeyType.ZoomIn, KeyCode.PageUp);


        Debug.Log("Target is at: " + target.transform.position.ToString());
    }

    void FixedUpdate()
    {
        mainCamera.FixedUpdate(GetComponent<GameObject>());
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
        /*Free roam movement actions.*/
        if (!config.bindToTarget && Input.anyKey)
        {
            /*var frKeys = freeRoamConfig;
            if (frKeys.IsPressed(CameraControl.KeyType.Forward))
            {
                freeRoamTarget += transform.forward * delta;
            }
            if (frKeys.IsPressed(CameraControl.KeyType.Backward))
            {
                freeRoamTarget -= transform.forward * delta;
            }
            if (frKeys.IsPressed(CameraControl.KeyType.Left))
            {
                freeRoamTarget -= transform.right * delta;
            }
            if (frKeys.IsPressed(CameraControl.KeyType.Right))
            {
                freeRoamTarget += transform.right * delta;
            }
            if (frKeys.IsPressed(CameraControl.KeyType.Up))
            {
                freeRoamTarget += transform.up * delta;
            }
            if (frKeys.IsPressed(CameraControl.KeyType.Down))
            {
                freeRoamTarget -= transform.up * delta;
            }*/
        }



        /*Set the camera to have to correct position.*/
        transform.position = mainCamera.GetLocation();
        transform.position += targetPosition;

        transform.LookAt(targetPosition);
        /*Do a barrel roll.*/
        transform.Rotate(0, 0, mainCamera.roll.value * Mathf.Rad2Deg);

        DebugInfo info1 = new DebugInfo(); ;
        info1.name = "X-Rotation";
        info1.info = transform.rotation.eulerAngles.x.ToString();
        DebugInfo info2 = new DebugInfo(); ;
        info2.name = "Y-Rotation";
        info2.info = transform.rotation.eulerAngles.y.ToString();
        DebugInfo info3 = new DebugInfo(); ;
        info3.name = "Z-Rotation";
        info3.info = transform.rotation.eulerAngles.z.ToString();

        mainCamera.ProcessDebugScreen(true,info1, info2, info3);

    }
}
