﻿using UnityEngine;
using UnityEngine.UI;

public class CylindricalCamera : AbstractCamera
{
    /// <summary>
    /// The coordinates on spherical components of the camera.
    /// </summary>
    private CylindricalVector cylindricalVector;
    /// <summary>
    /// Zoom in or zoom out with a negative amount.  The radius filter will 
    /// apply automatically if its set.
    /// </summary>
    /// <param name="amt">The amount to zoom. amt tess than 0 for zoom out.
    /// </param>
    override
    protected void Zoom(float amt)
    {
        cylindricalVector.rho += amt;
    }

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
    override
    protected void Spin(float aboutUpAxis_phi, float aboutRightAxis_theta,
        float aboutForwardAxis_roll)
    {
        /*Phi and theta will be filtered automaticaly if they have it set.*/
        if (aboutUpAxis_phi != 0)
            cylindricalVector.phi += aboutUpAxis_phi;
        if (aboutRightAxis_theta != 0)
            cylindricalVector.elevation += aboutRightAxis_theta;
        if (aboutForwardAxis_roll != 0)
            roll += aboutForwardAxis_roll;
    }


    /// <summary>
    /// Create the class.
    /// </summary>
    /// <param name="distance">The distance that the camera should be about 
    /// the target (How far behind the target does the camera start).</param>
    /// <param name="rollAmount">the Roll in radians of the forward axis 
    /// tilting. zero is straight up and down.</param>
    /// <param name="moveSpeed">The speed that the camera will move.</param>
    /// <param name="phi">The default value of the polar angle (how rotated to
    /// the sides does the camera start - Zero is directly behind).</param>
    /// <param name="theta">The default value of the azithmal angle. (How high
    /// above the player does the camera start - Zero is behind).</param>
    public CylindricalCamera(float distance,
        float rollAmount, CylindricalVector moveSpeed, float rollSpeed,
        float elevation, float phi, Text debugOutput) : base(rollAmount)
    {
        if (debugOutput != null)
        {
            this.debugOutput = debugOutput;
        }
        else
        {
            Debug.Log("The debug screen is null. No debugging.");
        }
        cylindricalVector = CylindricalVector.Create(distance, phi, elevation);
        if (moveSpeed.phi.value > 1.0f)
        {
            cylindricalVector.phi.AddFilter(
                new Filters.MultiplyFilter(moveSpeed.phi.value),
                Filters.Type.Set);
        }
        if (moveSpeed.elevation.value > 1.0f)
        {
            cylindricalVector.elevation.AddFilter(
                new Filters.MultiplyFilter(moveSpeed.elevation.value),
                Filters.Type.Set);
        }
        if (moveSpeed.rho.value > 1.0f)
        {
            cylindricalVector.rho.AddFilter(
                new Filters.MultiplyFilter(moveSpeed.rho.value),
                Filters.Type.Set);
        }
        if (rollSpeed > 1.0f)
        {
            roll.AddFilter(
                new Filters.MultiplyFilter(rollSpeed), Filters.Type.Set);
        }
        roll.AddFilter(
            new Filters.RepeatFilter(2 * Mathf.PI), Filters.Type.Get);
        cylindricalVector.phi.AddFilter(
            new Filters.RepeatFilter(2 * Mathf.PI), Filters.Type.Get);
        cylindricalVector.elevation.AddFilter(
            new Filters.InversionFilter(), Filters.Type.Set);
    }
    /// <summary>
    /// Add a filter to the Zooming mechanism for the camera.
    /// </summary>
    /// <param name="filter">The filter to add.</param>
    /// <param name="type">The type of filter Filters.Type.Set or .Get</param>
    public override void AddFilter(Filters.MathFilter filter,
        Filters.Type type, PropertyType propType)
    {
        switch (propType)
        {
            case PropertyType.HorizontalRotation:
                cylindricalVector.phi.AddFilter(filter, type);
                break;
            case PropertyType.VerticalRotation:
                cylindricalVector.elevation.AddFilter(filter, type);
                break;
            case PropertyType.Roll:
                roll.AddFilter(filter, type);
                break;
            case PropertyType.Zoom:
                cylindricalVector.rho.AddFilter(filter, type);
                break;
            default:
                Debug.Log("Tried to add a filter for a non-valid property.");
                break;
        }
    }
    /// <summary>
    /// Get the Vector3 from this camera trackign object.
    /// </summary>
    /// <returns>A vector in cartesian coordinates.</returns>
    override
    public Vector3 GetLocation()
    {
        return CylindricalVector.ToCartesian(cylindricalVector);
    }
    /// <summary>
    /// Calculate any angle changes that need to happen.
    /// </summary>
    void ProcessAngles()
    {
        float rollAmt = GetAxis(CameraControl.AxisType.RollCamera);
        float phi = GetAxis(CameraControl.AxisType.CameraLeftRight);
        float theta = GetAxis(CameraControl.AxisType.CameraUpDown);

        if (IsPressed(CameraControl.KeyType.Rotate))
        {
            if (IsPressed(CameraControl.KeyType.Roll))
            {
                Spin(0, 0, rollAmt);
            }
            else
            {
                /*the adjusted vector is to translate the bare xy click to a
                 tilted vector so that the rotation is properly handled due
                 to the roll applied to the camera.*/
                var adjusted = TiltVector.CreateTiltedVector(
                    new Vector2(phi, theta), roll.value);
                Spin(adjusted.x, adjusted.y, 0);
            }
        }
    }
    /// <summary>
    /// Process internal debug information. With optional additional text.
    /// </summary>
    /// <param name="additionalText">Additional information to show.</param>
    public void ProcessDebug(Vector3 rot)
    {
        if (!enabled)
        {
            return;
        }
        if (debugOutput == null)
        {
            Debug.Log("Debug screen is null.");
            return;
        }

        debugOutput.text = "Elevation: " + cylindricalVector.elevation.value
            + "\nPhi: " + cylindricalVector.phi.value
            + "\nRoll: " + roll.value
            + "\nRho: " + cylindricalVector.rho.value
            + "\nXrot: " + rot.x
            + "\nYrot: " + rot.y
            + "\nZrot: " + rot.z;
    }
    /// <summary>
    /// Update the camera. Change position, rotation, direction, etc.
    /// </summary>
    /// <param name="camera">The GameObject that will be updated (the camera)
    /// </param>
    /// <param name="target">The target to focus on.</param>
    public override void FixedUpdate(Transform camera, Transform target)
    {
        if (!enabled)
        {
            return;
        }
        //see if there needs to be some zooming.
        ProcessZoom();
        //Change any angles that need to be changed.
        ProcessAngles();
        //Do the moving.
        ChangePosition(camera, target);
        //process the debug info.
        ProcessDebug(camera.rotation.eulerAngles);
    }
}
