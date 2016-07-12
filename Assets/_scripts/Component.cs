using System.Collections.Generic;
using UnityEngine;
using Filters;
/// <summary>
/// A value that holds filters that are applied to the it when set.  Filters
/// are applied from first to last in order.  Filters added with 
/// Filters.Type.Set will be applied to new values being applied into the 
/// Component.  Filters added with Filters.Type.Get will be applied as a whole
/// each time the value is gotten.
/// </summary>
public class Component
{
    /// <summary>
    /// If the component is locked, it will not change, but can still be called
    /// as usual. trying to set a locked component will not cause a problem,
    /// but simply will not do anything except print a debug message.
    /// </summary>
    public bool locked { get; set; }

    private float _value;
    /// <summary>
    /// Get the value after applied filters.  To set without filters, use 
    /// ForceSet.
    /// Values set with this setter will have the entire result value affected
    /// by the set filters. If something turns out not to be correct, it may be
    /// that you should use the math operators (EG: Component + float)  So that
    /// only the float that is being put in will be effected by the set 
    /// filters.
    /// </summary>
    public float value
    {
        get
        {
            _value = ApplyFilters(_value, Filters.Type.Get);
            return _value;
        }
        set
        {
            _value = ApplyFilters(value, Filters.Type.Set);
        }
    }
    /// <summary>
    /// A set of Filters that if set will apply each time that the variable is 
    /// set.  adding the same type of filter will overwrite the old one.
    /// </summary>
    private List<MathFilter> setFilters = new List<MathFilter>();
    /// <summary>
    /// The filters that are applied to the values that are recieved from the 
    /// component.  The get filters will be applied to the result.
    /// </summary>
    private List<MathFilter> getFilters = new List<MathFilter>();
    /// <summary>
    /// Add to this value (+=). The change filters will be applied numerical order.
    /// </summary>
    /// <param name="value">The value to add.</param>
    private void AddTo(float value)
    {
        if (locked)
        {
            Debug.Log("Tried to set a locked Component: "
                + (new System.Diagnostics.StackTrace()).GetFrame(1)
                .ToString());
            return;
        }
        _value += ApplyFilters(value, Filters.Type.Set);
    }
    /// <summary>
    /// Same as *= Will apply the set filters.
    /// </summary>
    /// <param name="value">The value to multiply by.</param>
    private void MultiplyBy(float value)
    {
        if (locked)
        {
            Debug.Log("Tried to set a locked Component: "
                + (new System.Diagnostics.StackTrace()).GetFrame(1)
                .ToString());
            return;
        }
        _value *= ApplyFilters(value, Filters.Type.Set);
    }
    /// <summary>
    /// Same as /=. Will apply the set filters.
    /// </summary>
    /// <param name="value">The value or devide the component by.</param>
    private void DivideBy(float value)
    {
        MultiplyBy(1 / value);
    }
    /// <summary>
    /// Set this Component to be equal to a value.  The set filters will NOT
    /// be applied.
    /// </summary>
    /// <param name="value">The value to set.</param>
    public void ForceSet(float value)
    {
        if (locked)
        {
            Debug.Log("Tried to set a locked Component: "
                + (new System.Diagnostics.StackTrace()).GetFrame(1)
                .ToString());
            return;
        }
        _value = value;
    }
    /// <summary>
    /// Apply a set of filters to the value.
    /// </summary>
    /// <param name="value">The value to apply filters to.</param>
    /// <param name="type">Apply the Set or Get filters?</param>
    /// <returns></returns>
    private float ApplyFilters(float value, Filters.Type type)
    {
        List<MathFilter> filters = type == Filters.Type.Set ?
            setFilters : getFilters;
        int filterCount = filters.Count;
        if (filterCount > 0)
        {
            float newVal = value;
            for (int i = 0; i < filterCount; ++i)
            {
                /*filters are applied from 0 up and in order.*/
                newVal = filters[i].GetValue(newVal);
            }
            value = newVal;
        }
        return value;
    }
    /// <summary>
    /// Subtract from this value (-=). The change filters will be applied numerical 
    /// order.
    /// </summary>
    /// <param name="value">The value to subtract.</param>
    private void SubFrom(float value)
    {
        AddTo(-value);
    }
    /// <summary>
    /// Insert a filter. A filter of a type that is already in the list, will
    /// replace the old filter.  If custom filters are needed maek your own
    /// and implement MathFilter.
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="type">Remove Get or Set filters?</param>
    public void AddFilter(MathFilter filter, Filters.Type type)
    {
        List<MathFilter> filters = type == Filters.Type.Set ?
           setFilters : getFilters;
        int filterCount = filters.Count;
        for (int i = 0; i < filterCount; ++i)
        {
            if (filters[i].SameType(filter))
            {
                /*replace and return.*/
                filters[i] = filter;
                return;
            }
        }
        /*does not exists, so its added.*/
        filters.Add(filter);
    }

    /// <summary>
    /// Clear all the filters.
    /// </summary>
    ///  <param name="type">Remove Get or Set filters?</param>
    public void ClearFilters(Filters.Type type)
    {
        List<MathFilter> filters = type == Filters.Type.Set ?
           setFilters : getFilters;
        filters.Clear();
    }

    /// <summary>
    /// Remove the filter of the type otherType from the filter list. Lists 
    /// can only contain one filter of each type.  If it does not exist, it 
    /// returns false.
    /// </summary>
    /// <typeparam name="Filter">The type of filter to remove.</typeparam>
    /// <param name="type">Remove Get or Set filters?</param>
    /// <returns>true if a filter was removed.</returns>
    public bool RemoveFilter<Filter>(Filters.Type type)
    {
        List<MathFilter> filters = type == Filters.Type.Set ?
           setFilters : getFilters;
        int size = filters.Count;
        for (int i = 0; i < size; ++i)
        {
            if (filters[i] is Filter)
            {
                /*remove and return.*/
                filters.RemoveAt(i);
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Create a deep clone of this component.
    /// </summary>
    /// <returns>A complete copy of the Component.</returns>
    public Component Clone()
    {
        Component comp = new Component();
        comp._value = _value;
        foreach (MathFilter filter in setFilters)
        {
            comp.setFilters.Add(filter.Clone());
        }
        foreach (MathFilter filter in getFilters)
        {
            comp.getFilters.Add(filter.Clone());
        }
        return comp;
    }

    /// <summary>
    /// Add a Component and a float. The float will have lhs set 
    /// filters applied.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs">RHS will have set filters applied to it.</param>
    /// <returns>A new component that is the result.</returns>
    public static Component operator +(Component lhs, float rhs)
    {
        Component comp = lhs.Clone();
        comp.AddTo(rhs);
        return comp;
    }
    /// <summary>
    /// Subtract a float from the Component. The rhs will have lhs set filters
    /// applied to it.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs">RHS will have set filters applied to it.</param>
    /// <returns>A new component that is the result.</returns>
    public static Component operator -(Component lhs, float rhs)
    {
        Component comp = lhs.Clone();
        comp.SubFrom(rhs);
        return comp;
    }
    /// <summary>
    /// Multiply a float to the Component. The rhs will have lhs set filters
    /// applied to it.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs">RHS will have set filters applied to it.</param>
    /// <returns>A new component that is the result.</returns>
    public static Component operator *(Component lhs, float rhs)
    {
        Component comp = lhs.Clone();
        comp.MultiplyBy(rhs);
        return comp;
    }
    /// <summary>
    /// Divide a Component by the a float. The rhs will have lhs set filters
    /// applied to it.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs">RHS will have set filters applied to it.</param>
    /// <returns>A new component that is the result.</returns>
    public static Component operator /(Component lhs, float rhs)
    {
        Component comp = lhs.Clone();
        comp.DivideBy(rhs);
        return comp;
    }
}