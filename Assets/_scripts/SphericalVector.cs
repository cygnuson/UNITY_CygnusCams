using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/*
 * TODO
 * Factories
 * ctor
 * cylindrical convert.
 */


/// <summary>
/// Create a spherical vector for use in the Unity coordinate system.  The 
/// class assumes that the z-axis points forward and backward, the x-axis 
/// points left and right, and the y-axis points upward and downward.  The 
/// Spherical Vector can be explicitly casted to a Vector3 with the x,y,z 
/// coordinates set properly to be used directly in unity.  See wikipedia
/// for more info on spherical coordinates.
/// </summary>
[System.Serializable]
public class SphericalVector
{

    /// <summary>
    /// The type of coordinate system to work with.
    /// </summary>
    public enum Type
    {

        /// <summary>
        /// The XYZ coordinate system.
        /// </summary>
        Cartesian = 1,

        /// <summary>
        /// The spherical system using radius, zenith, azimuth.
        /// </summary>
        Spherical,

        /// <summary>
        /// Cylidrical system, using radius, zenith, elevation.
        /// </summary>
        Cylindrical
    }
    
    /// <summary>
    /// The radial coordinate (radius) of the value.
    /// </summary>
    float radial;

    /// <summary>
    /// The zenith angle (theta) of the value.  This is the anfle on the z-axis
    /// and the orthogonal projection plane intersecting the xy plane.
    /// </summary>
    float zenith;

    /// <summary>
    /// The azimuth angle (phi) of the value.  This is the angle on the xy
    /// plane.
    /// </summary>
    float azimuth;

    /// <summary>
    /// Construct a sperical vector from any of the three main coordinate 
    /// systems.
    /// </summary>
    /// <param name="x_or_radius">
    /// The x value in cartesian system, or the radius in the spherical or 
    /// cylindrical.
    /// </param>
    /// <param name="y_or_zenith">
    /// The y value in the cartesian system, or the zenith (theta) in the 
    /// spherical system or cylindrical system.
    /// </param>
    /// <param name="z_or_azimuth_or_elevation">
    /// The z value in the cartesian system, or the azimuth in the spherical 
    /// system, or the elevation in the cylindrical system.
    /// </param>
    /// <param name="loadSystem">
    /// The system to use to load the data. Use SphericalVector.Type.* to 
    /// choose.
    /// </param>
    public SphericalVector(
        float x_or_radius, 
        float y_or_zenith, 
        float z_or_azimuth_or_elevation, 
        SphericalVector.Type loadSystem)
    {
        switch (loadSystem)
        {
            case Type.Cartesian:
                break;
            case Type.Spherical:

                break;
            case Type.Cylindrical:

                break;
        }
    }

    /// <summary>
    /// Convert a spherical vector to a vector3. Its x,y,z coordinates will be
    /// properly calculated so that they can be used with the cartesian system.
    /// </summary>
    /// <param name="sv">The spherical vector to convert.</param>
    /// <returns>A Vector3 with properly calculated values.</returns>
    static public Vector3 ToVector3(SphericalVector sv)
    {
        float z = sv.radial * Mathf.Sin(sv.zenith)
           * Mathf.Cos(sv.azimuth);
        float x = sv.radial * Mathf.Sin(sv.zenith)
            * Mathf.Sin(sv.azimuth);
        float y = sv.radial * Mathf.Cos(sv.zenith);
        return new Vector3(x, y, z);
    }

    /// <summary>
    /// Explicit conversion to a Vector3.
    /// </summary>
    /// <param name="sv">
    /// The spherical coordinate to be converted explicitly.
    /// </param>
    static public explicit operator Vector3(SphericalVector sv)
    {
        return ToVector3(sv);
    }

}
