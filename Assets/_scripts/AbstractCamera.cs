using UnityEngine;
using System.Collections.Generic;
using System;

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
    /// True if the key or button or key is pressed. False otherwise.
    /// </returns>
    public bool IsPressed()
    {
        return useMouse ?
            Input.GetMouseButton(button)
            : Input.GetKey(key);
    }
}

public abstract class AbstractCamera
{
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
    /// Create the base part of the class.
    /// </summary>
    /// <param name="rollAmount">the Roll in radians of the forward axis tilting.
    /// </param>
    public AbstractCamera(float rollAmount)
    {
        this.roll.ForceSet(rollAmount);
    }
}
