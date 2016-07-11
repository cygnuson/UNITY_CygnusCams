using System;
using UnityEngine;

public class SphericalCamera : AbstractCamera
{

    /// <summary>
    /// The coordinates on spherical components of the camera.
    /// </summary>
    public SphericalVector sphericalVector;
    /// <summary>
    /// Zoom in or zoom out with a negative amount.  The radius filter will 
    /// apply automatically if its set.
    /// </summary>
    /// <param name="amt">The amount to zoom. amt tess than 0 for zoom out.
    /// </param>
    override
    public void Zoom(float amt)
    {
        sphericalVector.radius += amt;
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
    public void Spin(float aboutUpAxis_phi, float aboutRightAxis_theta,
        float aboutForwardAxis_roll)
    {
        /*Phi and theta will be filtered automaticaly if they have it set.*/
        if (aboutUpAxis_phi != 0)
            sphericalVector.phi += aboutUpAxis_phi;
        if (aboutRightAxis_theta != 0)
            sphericalVector.theta += aboutRightAxis_theta;
        if (aboutForwardAxis_roll != 0)
            roll += aboutForwardAxis_roll;
    }


    /// <summary>
    /// Create the class.
    /// </summary>
    /// <param name="distance">The distance that the camera should be about 
    /// the target (How far behind the target does the camera start).</param>
    /// <param name="rollAmount">the Roll in radians of the forward axis tilting.
    /// zero is straight up and down.</param>
    /// <param name="moveSpeed">The speed that the camera will move.</param>
    /// <param name="phi">The default value of the polar angle (how rotated to
    /// the sides does the camera start - Zero is directly behind).</param>
    /// <param name="theta">The default value of the azithmal angle. (How high
    /// above the player does the camera start - Zero is behind).</param>
    public SphericalCamera(float distance,
        float rollAmount, float moveSpeed, float rollSpeed, float theta,
        float phi) : base(rollAmount)
    {
        sphericalVector = SphericalVector.Create(distance, phi, theta);
    }

    /// <summary>
    /// Get the Vector3 from this camera trackign object.
    /// </summary>
    /// <returns>A vector in cartesian coordinates.</returns>
    override
    public Vector3 GetLocation()
    {
        return SphericalVector.ToCartesian(sphericalVector);
    }

    /// <summary>
    /// Do all the zoom key/axis checks.
    /// </summary>
    void ProcessZoom()
    {
        /*zoom the camera outward.*/
        if (AxisSet(CameraControl.AxisType.ZoomAxis))
        {
            float scrollAmount =
                GetAxis(CameraControl.AxisType.ZoomAxis);
            if (scrollAmount != 0)
            {
                sphericalVector.radius += scrollAmount;
            }
        }
        else if (Input.anyKey &&
            KeySet(CameraControl.KeyType.ZoomIn, CameraControl.KeyType.ZoomOut))
        {
            float scrollAmount = 1;
            if (IsPressed(CameraControl.KeyType.ZoomIn))
            {
                Zoom(-scrollAmount);
            }
            else if (IsPressed(CameraControl.KeyType.ZoomOut))
            {
                Zoom(scrollAmount);
            }
            else
            {
                //do nothing...for now.
            }
        }
        else
        {

        }
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
    /// Update the camera. Change position, rotation, direction, etc.
    /// </summary>
    /// <param name="camera">The GameObject that will be updated (the camera)
    /// </param>
    public override void FixedUpdate(GameObject camera)
    {
        //see if there needs to be some zooming.
        ProcessZoom();
        //Change any angles that need to be changed.
        ProcessAngles();

    }
}
