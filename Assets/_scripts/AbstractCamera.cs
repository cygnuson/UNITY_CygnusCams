using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public struct DebugInfo
{
    public string name;
    public string info;
}

public abstract class AbstractCamera
{
    private bool _enabled = false;
    /// <summary>
    /// If enabled is false, then this camera will not update or even do 
    /// anything.
    /// </summary>
    public bool enabled
    {
        get
        {
            return _enabled;
        }
        set
        {
            //perform disable/enable stuff
            _enabled = value;
        }
    }
    private bool _doDebug = false;
    /// <summary>
    /// If enabled is false, then this camera will not update or even do 
    /// anything.
    /// </summary>
    public bool doDebug
    {
        get
        {
            return _doDebug;
        }
        set
        {
            //perform disable/enable debug stuff
            _doDebug = value;
        }
    }
    /// <summary>
    /// The text element that will show debug information.
    /// </summary>
    public Text debugText;
    /// <summary>
    /// The amount of roll on this camera in radians. The angle that rotates
    /// around the forward axis.
    /// </summary>
    public Component roll = new Component();

    /// <summary>
    /// Get the Vector3 from this camera trackign object.
    /// </summary>
    /// <returns>A vector in cartesian coordinates.</returns>
    abstract public Vector3 GetLocation();
    /// <summary>
    /// The control settings for the camera.
    /// </summary>
    public CameraControl controls = new CameraControl();

    /// <summary>
    /// Spin the camera around. If its on a track that it will stay on its
    /// track properly.
    /// </summary>
    /// <param name="aboutUpAxis_phi">The amount to spin about the Up axis. 
    /// Also known as YAW</param>
    /// <param name="aboutRightAxis_theta">The amount to spin about the right
    /// axis. Also known as PITCH</param>
    /// <param name="aboutForwardAxis_roll">The amount to spin about the 
    /// forward axis. Also known as ROLL</param>
    abstract public void Spin(float aboutUpAxis_phi,
        float aboutRightAxis_theta, float aboutForwardAxis_roll);
    /// <summary>
    /// Zoom in or zoom out with a negative amount.  The radius filter will 
    /// apply automatically if its set.
    /// </summary>
    /// <param name="amt">The amount to zoom. amt tess than 0 for zoom out.
    /// </param>
    abstract public void Zoom(float amt);

    /// <summary>
    /// Update the camera. Change position, rotation, direction, etc.
    /// </summary>
    /// <param name="camera">The GameObject that will be updated (the camera)
    /// </param>
    abstract public void FixedUpdate(GameObject camera);
    /// <summary>
    /// Process internal debug information. With optional additional text.
    /// </summary>
    /// <param name="additionalText">Additional information to show.</param>
    /// <param name="newlines">True for each additionalText item to have a 
    /// new line.</param>
    abstract public void ProcessDebugScreen(
        bool newlines, params DebugInfo[] additionalText);
    /// <summary>
    /// Create the base part of the class.
    /// </summary>
    /// <param name="rollAmount">the Roll in radians of the forward axis 
    /// tilting.
    /// <param name="defTarget">The first target for the camera.
    /// </param>
    public AbstractCamera(float rollAmount)
    {
        roll.ForceSet(rollAmount);
    }
    /// <summary>
    /// Check if a non axis is pressed.
    /// </summary>
    /// <param name="type">The control slot to be checked.</param>
    /// <returns>True if the control is active.</returns>
    public bool IsPressed(CameraControl.KeyType type)
    {
        return controls.IsPressed(type);
    }
    /// <summary>
    /// Enable the use of an axis.
    /// </summary>
    /// <param name="type">The type of axis to set.</param>
    /// <param name="axis">the name of the axis to use.</param>
    public void EnableAxis(CameraControl.AxisType type, string axis)
    {
        controls.EnableAxis(type, axis);
    }
    /// <summary>
    /// Enable a control with a key.
    /// </summary>
    /// <param name="type">The control to enable.</param>
    /// <param name="key">the key to use</param>
    public void EnableControl(CameraControl.KeyType type, KeyCode key)
    {
        controls.EnableControl(type, key);
    }
    /// <summary>
    /// enable a control with a key.
    /// </summary>
    /// <param name="type">The control to enable.</param>
    /// <param name="button">the button number to use</param>
    public void EnableControl(CameraControl.KeyType type, int button)
    {
        controls.EnableControl(type, button);
    }
    /// <summary>
    /// Get the value of an axis.
    /// </summary>
    /// <param name="type">The type to get.</param>
    /// <returns>The value of the axis</returns>
    public float GetAxis(CameraControl.AxisType type)
    {
        return controls.GetAxis(type);
    }
    /// <summary>
    /// Dtermine if an axis is to be used.
    /// </summary>
    /// <param name="type">The axis to check</param>
    /// <returns>True if an axis should be used for `type`</returns>
    public bool AxisSet(params CameraControl.AxisType[] types)
    {
        return controls.AxisSet(types);
    }
    /// <summary>
    /// Determine if a key has been set or not.
    /// </summary>
    /// <param name="type">The key to check.</param>
    /// <returns>True if the key for `type` has been set.</returns>
    public bool KeySet(params CameraControl.KeyType[] types)
    {
        return controls.KeySet(types);
    }
    /// <summary>
    /// Check if the controls are locked.
    /// </summary>
    /// <returns>True if the controls are locked.</returns>
    public bool IsUsable()
    {
        return controls.IsUsable();
    }
    /// <summary>
    /// Check any amount of types for usability.
    /// </summary>
    /// <param name="types">The types to check.</param>
    /// <returns>True if all types can give input.</returns>
    public bool IsUsable(params CameraControl.KeyType[] types)
    {
        return controls.IsUsable(types);
    }
    /// <summary>
    /// Determine if some keys are locked.
    /// </summary>
    /// <param name="types">The types to check.</param>
    /// <returns>True if even one key is locked.</returns>
    public bool KeyLocked(params CameraControl.KeyType[] types)
    {
        return controls.KeyLocked(types);
    }
    /// <summary>
    /// Check if an axis is usable
    /// </summary>
    /// <param name="type">the type of axis to check.</param>
    /// <returns>True if input can be gotten from the axis</returns>
    public bool IsUsable(params CameraControl.AxisType[] types)
    {
        return controls.IsUsable(types);
    }
    /// <summary>
    /// Check if some axis are locked.
    /// </summary>
    /// <param name="types">The types to check.</param>
    /// <returns>Returns true if even one axis passed is locked.</returns>
    public bool AxisLocked(params CameraControl.AxisType[] types)
    {
        return controls.AxisLocked(types);
    }
    /// <summary>
    /// Lock some keys.
    /// </summary>
    /// <param name="types">The controls to lock</param>
    public void Lock(params CameraControl.KeyType[] types)
    {
        controls.Lock(types);
    }
    /// <summary>
    /// Lock some axis.
    /// </summary>
    /// <param name="types">The axis to lock</param>
    public void Lock(params CameraControl.AxisType[] types)
    {
        controls.Lock(types);
    }
    /// <summary>
    /// UnLock some keys.
    /// </summary>
    /// <param name="types">The controls to unlock</param>
    public void UnLock(params CameraControl.KeyType[] types)
    {
        controls.UnLock(types);
    }
    /// <summary>
    /// UnLock some axis.
    /// </summary>
    /// <param name="types">The axis to unlock</param>
    public void UnLock(params CameraControl.AxisType[] types)
    {
        controls.UnLock(types);
    }
}
