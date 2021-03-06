﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Interval.cs" company="Nanchang Jinchen Software Co., Ltd.">
//   Copyright (c) 2010 Nanchang Jinchen Software Co., Ltd. All rights reserved.
// </copyright>
// <summary>
//   The interval object.
// </summary>
// <author>Kingcean Tuan</author>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace Trivial.Maths
{
    /// <summary>
    /// The utility of interval.
    /// </summary>
    public static class IntervalUtility
    {
        /// <summary>
        /// Loads a copier into current instance.
        /// </summary>
        /// <param name="interval">The interval to be filled.</param>
        /// <param name="value">The instance to copy.</param>
        public static void Load<T>(this ISimpleInterval<T> interval, ISimpleInterval<T> value)
        {
            if (value == null) return;
            interval.MinValue = value.MinValue;
            interval.MaxValue = value.MaxValue;
            interval.LeftOpen = value.LeftOpen;
            interval.RightOpen = value.RightOpen;
        }

        /// <summary>
        /// Sets the values for the simple closed interval.
        /// The greater one will be set as MaxValue, the less one will be set as MinValue.
        /// </summary>
        /// <param name="interval">The interval to be filled.</param>
        /// <param name="valueA">The first value.</param>
        /// <param name="valueB">The seconde value.</param>
        public static void SetValues<T>(this ISimpleInterval<T> interval, T valueA, T valueB)
        {
            if (interval.IsGreaterThanOrEqualMinValue(valueA)) interval.MaxValue = valueA;
            interval.MinValue = valueA;
            interval.MaxValue = valueA;
            if (interval.IsGreaterThanOrEqualMinValue(valueB) || interval.IsGreaterThanOrEqualMaxValue(valueB))
            {
                interval.MaxValue = valueB;
                return;
            }

            interval.MinValue = valueB;
            interval.MaxValue = valueA;
        }

        /// <summary>
        /// Sets the values for the simple closed interval.
        /// The greater one will be set as MaxValue, the less one will be set as MinValue.
        /// </summary>
        /// <param name="interval">The interval to be filled.</param>
        /// <param name="valueA">The first value.</param>
        /// <param name="valueB">The seconde value.</param>
        /// <param name="leftOpen">true if it is left open; otherwise, left closed.</param>
        /// <param name="rightOpen">true if it is right open; otherwise, right closed.</param>
        public static void SetValues<T>(this ISimpleInterval<T> interval, T valueA, T valueB, bool leftOpen, bool rightOpen)
        {
            interval.LeftOpen = leftOpen;
            interval.RightOpen = rightOpen;
            interval.SetValues(valueA, valueB);
        }

        /// <summary>
        /// Sets a greater value if the it is not in the interval.
        /// If the parameter is greater than MaxValue, this value will instead MaxValue; otherwise, nothing will be changed.
        /// </summary>
        /// <param name="interval">The interval to be filled.</param>
        /// <param name="value">A greater value to make sure the interval can include.</param>
        public static void SetGreaterValue<T>(this ISimpleInterval<T> interval, T value)
        {
            if (interval.IsGreaterThanOrEqualMaxValue(value))
            {
                interval.MaxValue = value;
            }
        }

        /// <summary>
        /// Sets a less value if the it is not in the interval.
        /// If the parameter is less than MinValue, this value will instead MinValue; otherwise, nothing will be changed.
        /// </summary>
        /// <param name="interval">The interval to be filled.</param>
        /// <param name="value">A less value to make sure the interval can include.</param>
        public static void SetLessValue<T>(this ISimpleInterval<T> interval, T value)
        {
            if (interval.IsLessThanOrEqualMinValue(value))
            {
                interval.MinValue = value;
            }
        }

        /// <summary>
        /// Sets a greater value if the it is not in the interval.
        /// If the parameter is greater than MaxValue, this value will instead MaxValue; otherwise, nothing will be changed.
        /// </summary>
        /// <param name="interval">The interval to be filled.</param>
        /// <param name="value">A greater value to make sure the interval can include.</param>
        /// <param name="rightOpen">true if it is right open; otherwise, right closed.</param>
        public static void SetGreaterValue<T>(this ISimpleInterval<T> interval, T value, bool rightOpen)
        {
            interval.RightOpen = rightOpen;
            if (interval.IsGreaterThanOrEqualMaxValue(value))
            {
                interval.MaxValue = value;
            }
        }

        /// <summary>
        /// Sets a less value if the it is not in the interval.
        /// If the parameter is less than MinValue, this value will instead MinValue; otherwise, nothing will be changed.
        /// </summary>
        /// <param name="interval">The interval to be filled.</param>
        /// <param name="value">A less value to make sure the interval can include.</param>
        /// <param name="leftOpen">true if it is left open; otherwise, left closed.</param>
        public static void SetLessValue<T>(this ISimpleInterval<T> interval, T value, bool leftOpen)
        {
            interval.LeftOpen = leftOpen;
            if (interval.IsLessThanOrEqualMinValue(value))
            {
                interval.MinValue = value;
            }
        }

        /// <summary>
        /// Checks if a value is in the interval.
        /// </summary>
        /// <param name="value">A value to check.</param>
        /// <param name="interval">The interval to compare.</param>
        /// <returns>true if the value is in the interval; otherwise, false.</returns>
        public static bool IsInInterval<T>(this ISimpleInterval<T> interval, T value)
        {
            var compareLeft = interval.LeftOpen ? interval.IsGreaterThanMinValue(value) : interval.IsGreaterThanOrEqualMinValue(value);
            if (!compareLeft) return false;
            return interval.RightOpen ? interval.IsLessThanMaxValue(value) : interval.IsLessThanOrEqualMaxValue(value);
        }

        /// <summary>
        /// Checks if the value is less than or equal MinValue.
        /// </summary>
        /// <param name="interval">The interval to compare.</param>
        /// <param name="value">A value to compare with MinValue.</param>
        /// <returns>true if the specific value is less than or equal MinValue; otherwise, false.</returns>
        public static bool IsLessThanOrEqualMinValue<T>(this ISimpleInterval<T> interval, T value)
        {
            return interval.IsLessThanMinValue(value) || interval.EqualsMinValue(value);
        }

        /// <summary>
        /// Checks if the value is greater than or equal MinValue.
        /// </summary>
        /// <param name="interval">The interval to compare.</param>
        /// <param name="value">A value to compare with MinValue.</param>
        /// <returns>true if the specific value is greater than or equal MinValue; otherwise, false.</returns>
        public static bool IsGreaterThanOrEqualMinValue<T>(this ISimpleInterval<T> interval, T value)
        {
            return interval.IsGreaterThanMinValue(value) || interval.EqualsMinValue(value);
        }

        /// <summary>
        /// Checks if the value is less than or equal MaxValue.
        /// </summary>
        /// <param name="interval">The interval to compare.</param>
        /// <param name="value">A value to compare with MaxValue.</param>
        /// <returns>true if the specific value is less than or equal MaxValue; otherwise, false.</returns>
        public static bool IsLessThanOrEqualMaxValue<T>(this ISimpleInterval<T> interval, T value)
        {
            return interval.IsLessThanMaxValue(value) || interval.EqualsMaxValue(value);
        }

        /// <summary>
        /// Checks if the value is greater than or equal MaxValue.
        /// </summary>
        /// <param name="interval">The interval to compare.</param>
        /// <param name="value">A value to compare with MaxValue.</param>
        /// <returns>true if the specific value is greater than or equal MaxValue; otherwise, false.</returns>
        public static bool IsGreaterThanOrEqualMaxValue<T>(this ISimpleInterval<T> interval, T value)
        {
            return interval.IsGreaterThanMaxValue(value) || interval.EqualsMaxValue(value);
        }
    }

    /// <summary>
    /// The interface for simple closed interval.
    /// </summary>
    /// <typeparam name="T">The type of the object in interval.</typeparam>
    public interface ISimpleInterval<T>
    {
        /// <summary>
        /// Gets a value indicating whether it is left bounded.
        /// </summary>
        bool LeftBounded { get; }

        /// <summary>
        /// Gets a value indicating whether it is right bounded.
        /// </summary>
        bool RightBounded { get; }

        /// <summary>
        /// Gets or sets a value indicating whether it is left open.
        /// </summary>
        bool LeftOpen { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether it is right open.
        /// </summary>
        bool RightOpen { get; set; }

        /// <summary>
        /// Gets or sets the minimum value of the interval.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">The value should be less than MaxValue.</exception>
        T MinValue { get; set; }

        /// <summary>
        /// Gets or sets the maximum value of the interval.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">The value should be greater than MinValue.</exception>
        T MaxValue { get; set; }

        /// <summary>
        /// Checks if the value is less than MinValue.
        /// </summary>
        /// <param name="value">A value to compare with MinValue.</param>
        /// <returns>true if the specific value is less than or equal MinValue; otherwise, false.</returns>
        bool IsLessThanMinValue(T value);

        /// <summary>
        /// Checks if the value is greater than MinValue.
        /// </summary>
        /// <param name="value">A value to compare with MinValue.</param>
        /// <returns>true if the specific value is greater than or equal MinValue; otherwise, false.</returns>
        bool IsGreaterThanMinValue(T value);

        /// <summary>
        /// Checks if the value equals MinValue.
        /// </summary>
        /// <param name="value">A value to compare with MinValue.</param>
        /// <returns>true if the specific value is greater than or equal MinValue; otherwise, false.</returns>
        bool EqualsMinValue(T value);

        /// <summary>
        /// Checks if the value is less than MaxValue.
        /// </summary>
        /// <param name="value">A value to compare with MaxValue.</param>
        /// <returns>true if the specific value is less than or equal MaxValue; otherwise, false.</returns>
        bool IsLessThanMaxValue(T value);

        /// <summary>
        /// Checks if the value is greater than MaxValue.
        /// </summary>
        /// <param name="value">A value to compare with MaxValue.</param>
        /// <returns>true if the specific value is greater than or equal MaxValue; otherwise, false.</returns>
        bool IsGreaterThanMaxValue(T value);

        /// <summary>
        /// Checks if the value equals MaxValue.
        /// </summary>
        /// <param name="value">A value to compare with MaxValue.</param>
        /// <returns>true if the specific value is greater than or equal MaxValue; otherwise, false.</returns>
        bool EqualsMaxValue(T value);

        /// <summary>
        /// Converts the interval to a tuple, which includes MinValue and MaxValue.
        /// </summary>
        /// <returns>A tuple including MinValue and MaxValue.</returns>
        Tuple<T, T> ToTuple();

        /// <summary>
        /// Converts the interval to a list, which includes MinValue and MaxValue.
        /// </summary>
        /// <returns>A list including MinValue and MaxValue.</returns>
        IList<T> ToList();
    }

    /// <summary>
    /// The simple closed interval base class.
    /// </summary>
    public abstract class SimpleInterval<T> : ISimpleInterval<T>
    {
        /// <summary>
        /// The minimum value of the interval in memory.
        /// </summary>
        private T _minValue;

        /// <summary>
        /// The maximum value of the interval in memory.
        /// </summary>
        private T _maxValue;

        /// <summary>
        /// The value indicating whether it is left open.
        /// </summary>
        private bool _leftOpen;

        /// <summary>
        /// The value indicating whether it is right open.
        /// </summary>
        private bool _rightOpen;
        
        /// <summary>
        /// Initializes a new instance of the SimpleClosedInterval class.
        /// </summary>
        protected SimpleInterval()
        {
            LeftOpen = false;
            RightOpen = false;
        }

        /// <summary>
        /// Initializes a new instance of the SimpleClosedInterval class.
        /// </summary>
        /// <param name="valueA">The first value.</param>
        /// <param name="valueB">The seconde value.</param>
        protected SimpleInterval(T valueA, T valueB)
        {
            this.SetValues(valueA, valueB);
            LeftOpen = false;
            RightOpen = false;
        }

        /// <summary>
        /// Initializes a new instance of the SimpleClosedInterval class.
        /// </summary>
        /// <param name="valueA">The first value.</param>
        /// <param name="valueB">The seconde value.</param>
        /// <param name="leftOpen">true if it is left open; otherwise, left closed.</param>
        /// <param name="rightOpen">true if it is right open; otherwise, right closed.</param>
        protected SimpleInterval(T valueA, T valueB, bool leftOpen, bool rightOpen)
        {
            this.SetValues(valueA, valueB);
            LeftOpen = leftOpen;
            RightOpen = rightOpen;
        }

        /// <summary>
        /// Gets a value indicating whether it is left bounded.
        /// </summary>
        public abstract bool LeftBounded { get; }

        /// <summary>
        /// Gets a value indicating whether it is right bounded.
        /// </summary>
        public abstract bool RightBounded { get; }

        /// <summary>
        /// Gets or sets a value indicating whether it is left open.
        /// </summary>
        public bool LeftOpen { get { return _leftOpen || !LeftBounded; } set { _leftOpen = value; } }

        /// <summary>
        /// Gets or sets a value indicating whether it is right open.
        /// </summary>
        public bool RightOpen { get { return _rightOpen || !RightBounded; } set { _rightOpen = value; } }

        /// <summary>
        /// Gets or sets the minimum value of the interval.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">The value should be less than MaxValue.</exception>
        public T MinValue
        {
            get
            {
                return _minValue;
            }

            set
            {
                const string errStr = "The value should be less than MaxValue.";
                if (!this.IsLessThanOrEqualMinValue(value) && IsGreaterThanMaxValue(value) && RightBounded)
                    throw new ArgumentOutOfRangeException("value", errStr);
                _minValue = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the maximum value of the interval.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">The value should be greater than MinValue.</exception>
        public T MaxValue
        {
            get
            {
                return _maxValue;
            }

            set
            {
                const string errStr = "The value should be greater than MinValue.";
                if (!this.IsGreaterThanOrEqualMaxValue(value) && IsLessThanMinValue(value) && LeftBounded) throw new ArgumentOutOfRangeException("value", errStr);
                _maxValue = value;
            }
        }

        /// <summary>
        /// Checks if the value is less than MinValue.
        /// </summary>
        /// <param name="value">A value to compare with MinValue.</param>
        /// <returns>true if the specific value is less than or equal MinValue; otherwise, false.</returns>
        public abstract bool IsLessThanMinValue(T value);

        /// <summary>
        /// Checks if the value is greater than MinValue.
        /// </summary>
        /// <param name="value">A value to compare with MinValue.</param>
        /// <returns>true if the specific value is greater than or equal MinValue; otherwise, false.</returns>
        public abstract bool IsGreaterThanMinValue(T value);

        /// <summary>
        /// Checks if the value equals MinValue.
        /// </summary>
        /// <param name="value">A value to compare with MinValue.</param>
        /// <returns>true if the specific value is greater than or equal MinValue; otherwise, false.</returns>
        public abstract bool EqualsMinValue(T value);

        /// <summary>
        /// Checks if the value is less than MaxValue.
        /// </summary>
        /// <param name="value">A value to compare with MaxValue.</param>
        /// <returns>true if the specific value is less than or equal MaxValue; otherwise, false.</returns>
        public abstract bool IsLessThanMaxValue(T value);

        /// <summary>
        /// Checks if the value is greater than MaxValue.
        /// </summary>
        /// <param name="value">A value to compare with MaxValue.</param>
        /// <returns>true if the specific value is greater than or equal MaxValue; otherwise, false.</returns>
        public abstract bool IsGreaterThanMaxValue(T value);

        /// <summary>
        /// Checks if the value equals MaxValue.
        /// </summary>
        /// <param name="value">A value to compare with MaxValue.</param>
        /// <returns>true if the specific value is greater than or equal MaxValue; otherwise, false.</returns>
        public abstract bool EqualsMaxValue(T value);

        /// <summary>
        /// Converts the interval to a tuple, which includes MinValue and MaxValue.
        /// </summary>
        /// <returns>A tuple including MinValue and MaxValue.</returns>
        public Tuple<T, T> ToTuple()
        {
            return new Tuple<T, T>(MinValue, MaxValue);
        }

        /// <summary>
        /// Converts the interval to a tuple, which includes MinValue and MaxValue.
        /// </summary>
        /// <returns>A tuple including MinValue and MaxValue.</returns>
        public ValueTuple<T, T> ToValueTuple()
        {
            return new ValueTuple<T, T>(MinValue, MaxValue);
        }

        /// <summary>
        /// Converts the interval to a two elements object, which includes MinValue and MaxValue.
        /// </summary>
        /// <returns>A multiple elements object including MinValue and MaxValue.</returns>
        public TwoElements<T> ToTwoElements()
        {
            return new TwoElements<T>{ ItemA = MinValue, ItemB = MaxValue };
        }

        /// <summary>
        /// Converts the interval to a list, which includes MinValue and MaxValue.
        /// </summary>
        /// <returns>A list including MinValue and MaxValue.</returns>
        public IList<T> ToList()
        {
            return new List<T> { MinValue, MaxValue };
        }

        /// <summary>
        /// Returns the interval string value of this instance.
        /// </summary>
        /// <returns>A System.String containing this interval.</returns>
        public override string ToString()
        {
            var leftStr = LeftBounded ? MinValue.ToString() : NumberSymbols.NegativeInfiniteSymbol;
            var rightStr = RightBounded ? MaxValue.ToString() : NumberSymbols.PositiveInfiniteSymbol;
            var longStr = string.Format("{0} - {1}", leftStr, rightStr);
            var sep = false;
            if (longStr.IndexOfAny(new[] { ',', ';' }) > -1) sep = true;
            if (!sep && longStr.IndexOf(';') > -1)
            {
                const string quoteStr = "\"{0}\"";
                leftStr = string.Format(quoteStr, leftStr.Replace("\"", "\\\""));
                rightStr = string.Format(quoteStr, rightStr.Replace("\"", "\\\""));
            }

            return string.Format("{0}{1}{2} {3}{4}", LeftOpen ? "(" : "[", leftStr, sep ? ";" : ",", rightStr, RightOpen ? ")" : "]");
        }
    }

    /// <summary>
    /// The simple interval with class value.
    /// </summary>
    public class RefValueSimpleInterval<T> : SimpleInterval<T> where T : class, IComparable<T>
    {
        /// <summary>
        /// Initializes a new instance of the RefValueSimpleInterval class.
        /// </summary>
        public RefValueSimpleInterval()
        {
        }

        /// <summary>
        /// Initializes a new instance of the RefValueSimpleInterval class.
        /// </summary>
        /// <param name="valueA">The first value.</param>
        /// <param name="valueB">The seconde value.</param>
        public RefValueSimpleInterval(T valueA, T valueB)
            : base(valueA, valueB)
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the RefValueSimpleInterval class.
        /// </summary>
        /// <param name="valueA">The first value.</param>
        /// <param name="valueB">The seconde value.</param>
        /// <param name="leftOpen">true if it is left open; otherwise, left closed.</param>
        /// <param name="rightOpen">true if it is right open; otherwise, right closed.</param>
        public RefValueSimpleInterval(T valueA, T valueB, bool leftOpen, bool rightOpen)
            : base(valueA, valueB, leftOpen, rightOpen)
        {
        }

        /// <summary>
        /// Gets a value indicating whether it is left bounded.
        /// </summary>
        public override bool LeftBounded { get { return MinValue != null; } }

        /// <summary>
        /// Gets a value indicating whether it is right bounded.
        /// </summary>
        public override bool RightBounded { get { return MaxValue != null; } }

        /// <summary>
        /// Checks if the value is less than MinValue.
        /// </summary>
        /// <param name="value">A value to compare with MinValue.</param>
        /// <returns>true if the specific value is less than or equal MinValue; otherwise, false.</returns>
        public override bool IsLessThanMinValue(T value)
        {
            if (value == null) return true;
            if (MinValue == null) return false;
            return 0 < MinValue.CompareTo(value);
        }

        /// <summary>
        /// Checks if the value is greater than MinValue.
        /// </summary>
        /// <param name="value">A value to compare with MinValue.</param>
        /// <returns>true if the specific value is greater than or equal MinValue; otherwise, false.</returns>
        public override bool IsGreaterThanMinValue(T value)
        {
            if (MinValue == null) return true;
            if (value == null) return false;
            return 0 > MinValue.CompareTo(value);
        }

        /// <summary>
        /// Checks if the value equals MinValue.
        /// </summary>
        /// <param name="value">A value to compare with MinValue.</param>
        /// <returns>true if the specific value is greater than or equal MinValue; otherwise, false.</returns>
        public override bool EqualsMinValue(T value)
        {
            if (MinValue == null && value == null) return true;
            if (MinValue == null || value == null) return false;
            return 0 == MinValue.CompareTo(value);
        }

        /// <summary>
        /// Checks if the value is less than MaxValue.
        /// </summary>
        /// <param name="value">A value to compare with MaxValue.</param>
        /// <returns>true if the specific value is less than or equal MaxValue; otherwise, false.</returns>
        public override bool IsLessThanMaxValue(T value)
        {
            if (MaxValue == null) return true;
            if (value == null) return false;
            return 0 < MaxValue.CompareTo(value);
        }

        /// <summary>
        /// Checks if the value is greater than MaxValue.
        /// </summary>
        /// <param name="value">A value to compare with MaxValue.</param>
        /// <returns>true if the specific value is greater than or equal MaxValue; otherwise, false.</returns>
        public override bool IsGreaterThanMaxValue(T value)
        {
            if (value == null) return true;
            if (MaxValue == null) return false;
            return 0 > MaxValue.CompareTo(value);
        }

        /// <summary>
        /// Checks if the value equals MaxValue.
        /// </summary>
        /// <param name="value">A value to compare with MaxValue.</param>
        /// <returns>true if the specific value equals MaxValue; otherwise, false.</returns>
        public override bool EqualsMaxValue(T value)
        {
            if (MaxValue == null && value == null) return true;
            if (MaxValue == null || value == null) return false;
            return 0 == MaxValue.CompareTo(value);
        }
    }

    /// <summary>
    /// The simple interval with struct value.
    /// </summary>
    public class StructValueSimpleInterval<T> : SimpleInterval<T> where T : struct, IComparable<T>
    {
        /// <summary>
        /// Initializes a new instance of the NullableValueSimpleInterval class.
        /// </summary>
        public StructValueSimpleInterval()
        {
        }

        /// <summary>
        /// Initializes a new instance of the NullableValueSimpleInterval class.
        /// </summary>
        /// <param name="valueA">The first value.</param>
        /// <param name="valueB">The seconde value.</param>
        public StructValueSimpleInterval(T valueA, T valueB)
            : base(valueA, valueB)
        {
        }

        /// <summary>
        /// Initializes a new instance of the NullableValueSimpleInterval class.
        /// </summary>
        /// <param name="valueA">The first value.</param>
        /// <param name="valueB">The seconde value.</param>
        /// <param name="leftOpen">true if it is left open; otherwise, left closed.</param>
        /// <param name="rightOpen">true if it is right open; otherwise, right closed.</param>
        public StructValueSimpleInterval(T valueA, T valueB, bool leftOpen, bool rightOpen)
            : base(valueA, valueB, leftOpen, rightOpen)
        {
        }

        /// <summary>
        /// Gets a value indicating whether it is left bounded.
        /// </summary>
        public override bool LeftBounded { get { return true; } }

        /// <summary>
        /// Gets a value indicating whether it is right bounded.
        /// </summary>
        public override bool RightBounded { get { return true; } }

        /// <summary>
        /// Checks if the value is less than MinValue.
        /// </summary>
        /// <param name="value">A value to compare with MinValue.</param>
        /// <returns>true if the specific value is less than or equal MinValue; otherwise, false.</returns>
        public override bool IsLessThanMinValue(T value)
        {
            return 0 < MinValue.CompareTo(value);
        }

        /// <summary>
        /// Checks if the value is greater than MinValue.
        /// </summary>
        /// <param name="value">A value to compare with MinValue.</param>
        /// <returns>true if the specific value is greater than or equal MinValue; otherwise, false.</returns>
        public override bool IsGreaterThanMinValue(T value)
        {
            return 0 > MinValue.CompareTo(value);
        }

        /// <summary>
        /// Checks if the value equals MinValue.
        /// </summary>
        /// <param name="value">A value to compare with MinValue.</param>
        /// <returns>true if the specific value equals MinValue; otherwise, false.</returns>
        public override bool EqualsMinValue(T value)
        {
            return 0 == MinValue.CompareTo(value);
        }

        /// <summary>
        /// Checks if the value is less than MaxValue.
        /// </summary>
        /// <param name="value">A value to compare with MaxValue.</param>
        /// <returns>true if the specific value is less than or equal MaxValue; otherwise, false.</returns>
        public override bool IsLessThanMaxValue(T value)
        {
            return 0 < MaxValue.CompareTo(value);
        }

        /// <summary>
        /// Checks if the value is greater than MaxValue.
        /// </summary>
        /// <param name="value">A value to compare with MaxValue.</param>
        /// <returns>true if the specific value is greater than or equal MaxValue; otherwise, false.</returns>
        public override bool IsGreaterThanMaxValue(T value)
        {
            return 0 > MaxValue.CompareTo(value);
        }

        /// <summary>
        /// Checks if the value equals MaxValue.
        /// </summary>
        /// <param name="value">A value to compare with MaxValue.</param>
        /// <returns>true if the specific value equals MaxValue; otherwise, false.</returns>
        public override bool EqualsMaxValue(T value)
        {
            return 0 == MaxValue.CompareTo(value);
        }
    }

    /// <summary>
    /// The simple interval with nullable value.
    /// </summary>
    public class NullableValueSimpleInterval<T> : SimpleInterval<T?> where T : struct, IComparable<T>
    {
        /// <summary>
        /// Initializes a new instance of the NullableValueSimpleInterval class.
        /// </summary>
        public NullableValueSimpleInterval()
        {
        }

        /// <summary>
        /// Initializes a new instance of the NullableValueSimpleInterval class.
        /// </summary>
        /// <param name="valueA">The first value.</param>
        /// <param name="valueB">The seconde value.</param>
        public NullableValueSimpleInterval(T valueA, T valueB)
            : base(valueA, valueB)
        {
        }
        
        /// <summary>
        /// Initializes a new instance of the NullableValueSimpleInterval class.
        /// </summary>
        /// <param name="valueA">The first value.</param>
        /// <param name="valueB">The seconde value.</param>
        /// <param name="leftOpen">true if it is left open; otherwise, left closed.</param>
        /// <param name="rightOpen">true if it is right open; otherwise, right closed.</param>
        public NullableValueSimpleInterval(T valueA, T valueB, bool leftOpen, bool rightOpen)
            : base(valueA, valueB, leftOpen, rightOpen)
        {
        }

        /// <summary>
        /// Gets a value indicating whether it is left bounded.
        /// </summary>
        public override bool LeftBounded { get { return MinValue != null; } }

        /// <summary>
        /// Gets a value indicating whether it is right bounded.
        /// </summary>
        public override bool RightBounded { get { return MaxValue != null; } }

        /// <summary>
        /// Checks if the value is less than MinValue.
        /// </summary>
        /// <param name="value">A value to compare with MinValue.</param>
        /// <returns>true if the specific value is less than or equal MinValue; otherwise, false.</returns>
        public override bool IsLessThanMinValue(T? value)
        {
            if (value == null) return true;
            if (MinValue == null) return false;
            return 0 < MinValue.Value.CompareTo(value.Value);
        }

        /// <summary>
        /// Checks if the value is greater than MinValue.
        /// </summary>
        /// <param name="value">A value to compare with MinValue.</param>
        /// <returns>true if the specific value is greater than or equal MinValue; otherwise, false.</returns>
        public override bool IsGreaterThanMinValue(T? value)
        {
            if (MinValue == null) return true;
            if (value == null) return false;
            return 0 > MinValue.Value.CompareTo(value.Value);
        }

        /// <summary>
        /// Checks if the value equals MinValue.
        /// </summary>
        /// <param name="value">A value to compare with MinValue.</param>
        /// <returns>true if the specific value is greater than or equal MinValue; otherwise, false.</returns>
        public override bool EqualsMinValue(T? value)
        {
            if (MinValue == null && value == null) return true;
            if (MinValue == null || value == null) return false;
            return 0 == MinValue.Value.CompareTo(value.Value);
        }

        /// <summary>
        /// Checks if the value is less than MaxValue.
        /// </summary>
        /// <param name="value">A value to compare with MaxValue.</param>
        /// <returns>true if the specific value is less than or equal MaxValue; otherwise, false.</returns>
        public override bool IsLessThanMaxValue(T? value)
        {
            if (MaxValue == null) return true;
            if (value == null) return false;
            return 0 < MaxValue.Value.CompareTo(value.Value);
        }

        /// <summary>
        /// Checks if the value is greater than MaxValue.
        /// </summary>
        /// <param name="value">A value to compare with MaxValue.</param>
        /// <returns>true if the specific value is greater than or equal MaxValue; otherwise, false.</returns>
        public override bool IsGreaterThanMaxValue(T? value)
        {
            if (value == null) return true;
            if (MaxValue == null) return false;
            return 0 > MaxValue.Value.CompareTo(value.Value);
        }

        /// <summary>
        /// Checks if the value equals MaxValue.
        /// </summary>
        /// <param name="value">A value to compare with MaxValue.</param>
        /// <returns>true if the specific value equals MinValue; otherwise, false.</returns>
        public override bool EqualsMaxValue(T? value)
        {
            if (MaxValue == null && value == null) return true;
            if (MaxValue == null || value == null) return false;
            return 0 == MaxValue.Value.CompareTo(value.Value);
        }
    }
}
