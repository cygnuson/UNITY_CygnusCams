using UnityEngine;
using System.Collections.Generic;
using System;

//TODO: Comment filters.
namespace Filters
{
    /// <summary>
    /// Used to determine what type of filters to load.
    /// </summary>
    public enum Type
    {
        /// <summary>
        /// Use change filters.
        /// </summary>
        Set,
        /// <summary>
        /// Use get filters.
        /// </summary>
        Get,
    }

    public interface MathFilter
    {
        float GetValue(float current);
        MathFilter Clone();
        bool SameType(MathFilter other);
    }

    public class ClampFilter : MathFilter
    {
        public float min;
        public float max;

        public ClampFilter(float min, float max)
        {
            this.min = min;
            this.max = max;
        }

        public float GetValue(float current)
        {
            return Mathf.Clamp(current, min, max);
        }

        public bool SameType(MathFilter other)
        {
            return other is ClampFilter;
        }

        public MathFilter Clone()
        {
            return new ClampFilter(min, max);
        }
    }

    public class RepeatFilter : MathFilter
    {

        float max;

        public RepeatFilter(float max)
        {
            this.max = max;
        }

        public float GetValue(float current)
        {
            return Mathf.Repeat(current, max);
        }

        public bool SameType(MathFilter other)
        {
            return other is RepeatFilter;
        }
        public MathFilter Clone()
        {
            return new RepeatFilter(max);
        }
    }

    public class InversionFilter : MathFilter
    {
        public InversionFilter()
        {
        }

        public float GetValue(float current)
        {
            return -current;
        }

        public bool SameType(MathFilter other)
        {
            return other is InversionFilter;
        }
        public MathFilter Clone()
        {
            return new InversionFilter();
        }
    }

    public class MultiplyFilter : MathFilter
    {

        float amt;

        public MultiplyFilter(float amt)
        {
            this.amt = amt;
        }

        public float GetValue(float current)
        {
            return current * amt;
        }

        public bool SameType(MathFilter other)
        {
            return other is MultiplyFilter;
        }
        public MathFilter Clone()
        {
            return new MultiplyFilter(amt);
        }
    }

    public class LowerBoundFilter : MathFilter
    {

        float bound;

        public LowerBoundFilter(float bound)
        {
            this.bound = bound;
        }

        public float GetValue(float current)
        {
            return current < bound ? bound : current;
        }

        public bool SameType(MathFilter other)
        {
            return other is LowerBoundFilter;
        }
        public MathFilter Clone()
        {
            return new LowerBoundFilter(bound);
        }
    }

    public class UpperBoundFilter : MathFilter
    {

        float bound;

        public UpperBoundFilter(float bound)
        {
            this.bound = bound;
        }

        public float GetValue(float current)
        {
            return current > bound ? bound : current;
        }

        public bool SameType(MathFilter other)
        {
            return other is UpperBoundFilter;
        }
        public MathFilter Clone()
        {
            return new UpperBoundFilter(bound);
        }
    }
}