using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEngine.UI;

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

    public Camera myCamera;

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
        //mainCamera.EnableControl(CameraControl.KeyType.ZoomOut, KeyCode.PageDown);
        //mainCamera.EnableControl(CameraControl.KeyType.ZoomIn, KeyCode.PageUp);
        Debug.Log("Target is at: " + transform.position.ToString());
    }

    void FixedUpdate()
    {
        camController.FixedUpdate(myCamera.transform, transform);
    }




}
