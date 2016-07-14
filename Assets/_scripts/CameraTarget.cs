using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

/*
 * TODO
 * 
 * options to lerp the rotation and movement.
 * filters that will trigger a delegate when recievign a certian value.
 * mouse locking.
 * Free Roam stuff should go in a special FreeRoamCamera that contains a spherical/cyl/cart camera.
 * 
 * finish cylindrical cordinates for the new Component system.
 * 
 * 
 * use key combos instead of single keys.  Change the Control. If it has only one key
 *      or button set to the combo, then it will have no change in ehavior compared to right now.
 *      make the roll use a combo with first click, then right click. then zoom
 *      to work with first click then scroll.
 *      
 *better lerps (new project)
 *      
 * */

public class KeySet
{
    public delegate void PressedCaller();
    PressedCaller pressCallback;
    public List<Control> keys;
    public KeySet()
    {
        keys = new List<Control>();
    }
    public void SetCaller(PressedCaller caller)
    {
        pressCallback = caller;
    }
    public void Add(KeyCode keyCode)
    {
        keys.Add(new Control(keyCode));
    }
    public void Add(int button)
    {
        keys.Add(new Control(button));
    }
    private bool IsPressed(Control con)
    {
        return con.IsPressed() && !con.IsLocked();
    }
    public bool IsPressed()
    {
        int amt = keys.Count;
        for (int i = 0; i < amt; ++i)
        {
            if (!IsPressed(keys[i]))
            {
                return false;
            }
        }
        return true;
    }
    public bool Try()
    {
        if (IsPressed())
        {
            pressCallback();
            Debug.Log("Try: Key pressed.");
            return true;
        }
        else
        {

        }
        return false;
    }
}


public class CameraTarget : MonoBehaviour
{
    /// <summary>
    /// The type of camera to use.
    /// </summary>
    public enum CameraType
    {
        Spherical,
        Cylindrical,
        Fixed,
        FreeRoam,

        Total,
    }

    [Tooltip("The Text object that will recieve the debugging info.")]
    public Text debugScreen;
    [Tooltip("The camera that acts on this target.")]
    public AbstractCamera camController;
    /// <summary>
    /// The type of camera to use.
    /// </summary>
    public CameraType cameraType;

    public Camera unityCamera;

    KeySet _DEBUG_CHANGE_TO_SPHERICAL;
    KeySet _DEBUG_CHANGE_TO_CYLINDRICAL;

    public void SwitchToSpherical()
    {
        if (camController is SphericalCamera)
            return;
        Debug.Log("Attempting to switch to Spherical Camera mode.");
        var old = SphericalVector.
            CreateFromCartesian(camController.GetLocation());
        camController = new SphericalCamera(old.radius.value, 
            camController.getRoll,SphericalVector.Create(50, 1, 1), 
            5, old.theta.value, old.phi.value, debugScreen);
    }

    public void SwitchToCylindrical()
    {
        if (camController is CylindricalCamera)
            return;
        var old = CylindricalVector.
           CreateFromCartesian(camController.GetLocation());
        Debug.Log("Attempting to switch to Cylindrical Camera mode.");
        camController = new CylindricalCamera(old.rho.value, 
            camController.getRoll,CylindricalVector.Create(50, 1, 20), 5, 
            old.elevation.value, old.phi.value, debugScreen);
    }

    void ChangeCamera(CameraType type)
    {
        switch (type)
        {
            case CameraType.Cylindrical:
                SwitchToCylindrical();
                break;
            case CameraType.Spherical:
                SwitchToSpherical();
                break;
            case CameraType.Fixed:

                break;
            case CameraType.FreeRoam:

                break;
            default:
                Debug.Log("Invalid Camera type.");
                break;
        }
        SetupCamera();
    }

    void SetupCamera()
    {
        camController.AddFilter(
           new Filters.InversionFilter(), Filters.Type.Set,
           AbstractCamera.PropertyType.Roll);

        camController.AddFilter(
            new Filters.LowerBoundFilter(2), Filters.Type.Get,
            AbstractCamera.PropertyType.Zoom);

        camController.AddFilter(
             new Filters.InversionFilter(), Filters.Type.Set,
             AbstractCamera.PropertyType.Zoom);

        camController.EnableControl(CameraControl.KeyType.Rotate, 0);
        camController.EnableControl(CameraControl.KeyType.Roll, 1);
        camController.
            EnableAxis(CameraControl.AxisType.ZoomAxis, "Mouse ScrollWheel");
        camController.
            EnableAxis(CameraControl.AxisType.CameraLeftRight, "Mouse X");
        camController.
            EnableAxis(CameraControl.AxisType.RollCamera, "Mouse X");
        camController.
            EnableAxis(CameraControl.AxisType.CameraUpDown, "Mouse Y");

        camController.doDebug = true;
        camController.enabled = true;
        

    }

    void Start()
    {
        switch (cameraType)
        {
            case CameraType.Cylindrical:
                camController = new CylindricalCamera(
                    15, 0, CylindricalVector.Create(50, 1, 20), 5, 1, 0,
                    debugScreen);
                break;
            case CameraType.Spherical:
                camController = new SphericalCamera(
                    15, 0, SphericalVector.Create(50, 1, 1), 5, 1, 0,
                    debugScreen);
                break;
            case CameraType.Fixed:

                break;
            case CameraType.FreeRoam:

                break;
            default:
                Debug.Log("Invalid Camera type.");
                break;
        }


        SetupCamera();
        //mainCamera.EnableControl(CameraControl.KeyType.ZoomOut, KeyCode.PageDown);
        //mainCamera.EnableControl(CameraControl.KeyType.ZoomIn, KeyCode.PageUp);
        Debug.Log("Target is at: " + transform.position.ToString());

        ///////////////START////Debug Stuff///////////////////////////////////
        _DEBUG_CHANGE_TO_SPHERICAL = new KeySet();
        _DEBUG_CHANGE_TO_SPHERICAL.Add(KeyCode.Alpha1);
        _DEBUG_CHANGE_TO_SPHERICAL.SetCaller(()=>
        {
            ChangeCamera(CameraType.Spherical);
        });

        _DEBUG_CHANGE_TO_CYLINDRICAL = new KeySet();
        _DEBUG_CHANGE_TO_CYLINDRICAL.Add(KeyCode.Alpha2);
        _DEBUG_CHANGE_TO_CYLINDRICAL.SetCaller(() =>
        {
            ChangeCamera(CameraType.Cylindrical);
        });
        //////////////////END///Debug Stuff///////////////////////////////////
    }

    void FixedUpdate()
    {
        camController.FixedUpdate(unityCamera.transform, transform);
        _DEBUG_CHANGE_TO_CYLINDRICAL.Try();
        _DEBUG_CHANGE_TO_SPHERICAL.Try();

        



    }




}
