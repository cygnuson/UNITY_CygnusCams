using UnityEngine;


class TiltVector
{
    private Vector2 _vec;
    /// <summary>
    /// Get access to the internal unity Vector2.  When `get` happens, a new
    /// Vector2 is retured with the values translated by `tilt`.  Set the value
    /// with untranslated x and y.
    /// </summary>
    public Vector2 vector2
    {
        get
        {
            return CreateTiltedVector(_vec, _tilt);
        }
        set
        {
            _vec = value;
        }
    }
    private float _tilt;
    /// <summary>
    /// Set the tilt IN RADIANS for the translator.  Set values will be clamped
    /// to [0,2pi].
    /// </summary>
    public float tilt
    {
        get
        {
            return _tilt;
        }
        set
        {
            _tilt = Mathf.Clamp(value,0,2*Mathf.PI);
        }
    }

    /// <summary>
    /// Create a tilted vector. The value will be tilted by a rotation. Its
    /// usefull for translating screen drags when there is a roll applied to
    /// the object.
    /// </summary>
    /// <param name="tiltInRadians">The tilt, in radians to apply.</param>
    /// <param name="vector">The vector components.</param>
    public TiltVector(float tiltInRadians, Vector2 vector)
    {
        tilt = tiltInRadians;
        vector2 = vector;
    }

    /// <summary>
    /// Get the values of the vector without calculating tilts.
    /// </summary>
    /// <returns>A new vector2 with no tilt calculated.</returns>
    public Vector2 Untilted()
    {
        return new Vector2(_vec.x, _vec.y);
    }

    /// <summary>
    /// Implicitly get a Unity Vector2 that is tilted properly.
    /// </summary>
    /// <param name="vec">The TiltVector to convert.</param>
    public static implicit operator Vector2(TiltVector vec)
    {
        return vec.vector2;
    }

    public static Vector2 CreateTiltedVector(Vector2 vec, float tilt)
    {
        float _tilt = Mathf.Clamp(tilt, 0, 2 * Mathf.PI);
        float x = vec.x * Mathf.Cos(_tilt) - vec.y * Mathf.Sin(_tilt);
        float y = vec.x * Mathf.Sin(_tilt) + vec.y * Mathf.Cos(_tilt);
        return new Vector2(x, y);
    }
}

