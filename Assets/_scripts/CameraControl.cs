using UnityEngine;

[System.Serializable]
public class Control
{
    [Tooltip("True to check a mouse button instead of a key.")]
    public bool useMouse = false;
    [Tooltip("The button to check if `Use Mouse` is true.")]
    public int button;
    [Tooltip("The key to check if `Use Mouse` is not true.")]
    public KeyCode key;
    [Tooltip("False to allow this control to be activated.")]
    public bool locked = false;
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
    /// <summary>
    /// Check if the controls are locked.
    /// </summary>
    /// <returns>True if the controls are locked.</returns>
    public bool IsLocked()
    {
        return locked;
    }
}

/// <summary>
/// Configuration setting for a change to free roam mode.
/// </summary>
[System.Serializable]
public class CameraControl
{
    static long totalControls = 0;
    /// <summary>
    /// Initialize the control set.
    /// </summary>
    public CameraControl()
    {
        Debug.Log("Creating A CameraControl." );
        int keyLength = (int)KeyType.TotalControls;
        int axisLength = (int)AxisType.TotalAxis;
        for (int i = 0; i < axisLength; ++i)
        {
            useAxis[i] = false;
            axisNames[i] = "";
            lockAxis[i] = false;
        }
        for (int i = 0; i < keyLength; ++i)
        {
            controls[i] = new Control();
        }
        for (int i = 0; i < keyLength; ++i)
        {
            keySet[i] = false;
        }
        ++totalControls;
    }
    [Tooltip("True if the controls are locked.")]
    public bool locked = false;

    /// <summary>
    /// The type of control to set or get.
    /// </summary>
    public enum KeyType : int
    {
        Forward,
        Backward,
        Left,
        Right,
        Up,
        Down,
        ZoomIn,
        ZoomOut,
        Rotate,
        Roll,


        TotalControls,
    }

    /// <summary>
    /// The different axis controllable things.
    /// </summary>
    public enum AxisType : int
    {
        UpAxis,
        RightAxis,
        ForwardAxis,
        ZoomAxis,
        CameraLeftRight,
        CameraUpDown,
        RollCamera,

        TotalAxis,
    }
    [Tooltip("True to allow keys to be used.")]
    public bool[] keySet = new bool[(int)KeyType.TotalControls];
    [Tooltip("True to use an axis for the movement.")]
    public bool[] useAxis = new bool[(int)AxisType.TotalAxis];
    [Tooltip("True to lock an axis.")]
    public bool[] lockAxis = new bool[(int)AxisType.TotalAxis];
    [Tooltip("The buttons to move the camera.")]
    public Control[] controls = new Control[(int)KeyType.TotalControls];
    [Tooltip("The axis to use for the movement.")]
    public string[] axisNames = new string[(int)AxisType.TotalAxis];

    /// <summary>
    /// Lock some keys.
    /// </summary>
    /// <param name="types">The controls to lock</param>
    public void Lock(params KeyType[] types)
    {
        int amt = types.Length;
        for (int i = 0; i < amt; ++i)
        {
            controls[(int)types[i]].locked = true;
        }
    }
    /// <summary>
    /// Lock some axis.
    /// </summary>
    /// <param name="types">The axis to lock</param>
    public void Lock(params AxisType[] types)
    {
        int amt = types.Length;
        for (int i = 0; i < amt; ++i)
        {
            lockAxis[(int)types[i]] = true;
        }
    }
    /// <summary>
    /// UnLock some keys.
    /// </summary>
    /// <param name="types">The controls to unlock</param>
    public void UnLock(params KeyType[] types)
    {
        int amt = types.Length;
        for (int i = 0; i < amt; ++i)
        {
            controls[(int)types[i]].locked = false;
        }
    }
    /// <summary>
    /// UnLock some axis.
    /// </summary>
    /// <param name="types">The axis to unlock</param>
    public void UnLock(params AxisType[] types)
    {
        int amt = types.Length;
        for (int i = 0; i < amt; ++i)
        {
            lockAxis[(int)types[i]] = false;
        }
    }
    /// <summary>
    /// Check if the controls are locked.
    /// </summary>
    /// <returns>True if the controls are locked.</returns>
    public bool IsUsable()
    {
        return locked;
    }
    /// <summary>
    /// Check any amount of types for usability.
    /// </summary>
    /// <param name="types">The types to check.</param>
    /// <returns>True if all types can give input.</returns>
    public bool IsUsable(params KeyType[] types)
    {
        return KeySet(types) && !KeyLocked(types) && IsUsable();
    }
    /// <summary>
    /// Determine if some keys are locked.
    /// </summary>
    /// <param name="types">The types to check.</param>
    /// <returns>True if even one key is locked.</returns>
    public bool KeyLocked(params KeyType[] types)
    {
        int amt = types.Length;
        for (int i = 0; i < amt; ++i)
        {
            if (controls[(int)types[i]].IsLocked())
            {
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// Check if an axis is usable
    /// </summary>
    /// <param name="type">the type of axis to check.</param>
    /// <returns>True if input can be gotten from the axis</returns>
    public bool IsUsable(params AxisType[] types)
    {
        return AxisSet(types) && !AxisLocked(types) && IsUsable();
    }
    /// <summary>
    /// Check if some axis are locked.
    /// </summary>
    /// <param name="types">The types to check.</param>
    /// <returns>Returns true if even one axis passed is locked.</returns>
    public bool AxisLocked(params AxisType[] types)
    {
        int amt = types.Length;
        for (int i = 0; i < amt; ++i)
        {
            if (lockAxis[(int)types[i]])
            {
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// Check if a non axis is pressed.
    /// </summary>
    /// <param name="type">The control slot to be checked.</param>
    /// <returns>True if the control is active.</returns>
    public bool IsPressed(KeyType type)
    {
        return controls[(int)type].IsPressed();
    }
    /// <summary>
    /// Enable the use of an axis.
    /// </summary>
    /// <param name="type">The type of axis to set.</param>
    /// <param name="axis">the name of the axis to use.</param>
    public void EnableAxis(AxisType type, string axis)
    {
        Debug.Log("Enabling axis: " + axis);
        useAxis[(int)type] = true;
        axisNames[(int)type] = axis;
    }
    /// <summary>
    /// Enable a control with a key.
    /// </summary>
    /// <param name="type">The control to enable.</param>
    /// <param name="key">the key to use</param>
    public void EnableControl(KeyType type, KeyCode key)
    {
        controls[(int)type].key = key;
        keySet[(int)type] = true;
    }
    /// <summary>
    /// enable a control with a key.
    /// </summary>
    /// <param name="type">The control to enable.</param>
    /// <param name="button">the button number to use</param>
    public void EnableControl(KeyType type, int button)
    {
        controls[(int)type].button = button;
        controls[(int)type].useMouse = true;
        keySet[(int)type] = true;
    }
    /// <summary>
    /// Get the value of an axis.
    /// </summary>
    /// <param name="type">The type to get.</param>
    /// <returns>The value of the axis</returns>
    public float GetAxis(AxisType type)
    {
        return Input.GetAxis(axisNames[(int)type]);
    }
    /// <summary>
    /// Dtermine if some axis can be used.
    /// </summary>
    /// <param name="types">The axis to check</param>
    /// <returns>True if the axis are usable.</returns>
    public bool AxisSet(params AxisType[] types)
    {
        int amt = types.Length;
        for (int i = 0; i < amt; ++i)
        {
            if (!useAxis[(int)types[i]])
            {
                return false;
            }
        }
        return true;
    }
    /// <summary>
    /// Determine if a key has been set or not.
    /// </summary>
    /// <param name="types">The keys to check.</param>
    /// <returns>True if the keys for `types` has been set.</returns>
    public bool KeySet(params KeyType[] types)
    {
        int amt = types.Length;
        for (int i = 0; i < amt; ++i)
        {
            if (!keySet[(int)types[i]])
                return false;
        }
        return true;
    }
}