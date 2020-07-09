using System;
using UnityEngine;
using UnityEngine.Events;

namespace Do.AttributeSystem
{
    [Serializable]
    public class Attribute
    {
        [Header("Base")]
        [SerializeField] protected AttributeType type = null;
        [SerializeField] protected ModifierCollection modifiers = new ModifierCollection();

        [Header("Values")]
        [SerializeField] float baseValue = 0;
        [SerializeField] float currentValue = 0;

        [Header("Events")]
        [SerializeField] UnityEvent onMinimumValueReachedEvent = new UnityEvent();
        [SerializeField] UnityEvent onMaximumValueReachedEvent = new UnityEvent();

        float bonusValue = 0;

        public Attribute()
        {
            modifiers.OnModifiersAdded += UpdateBonusValue;
            modifiers.OnModifiersRemoved += UpdateBonusValue;

            if (type != null)
            {
                SetToMaxValue();
                UpdateBonusValue();
            }
        }

        public Attribute(AttributeType type) : this()
        {
            this.type = type;
        }

        public void SetToMaxValue() => Value = MaxValue;
        public void SetToMinValue() => Value = MinValue;

        void UpdateBonusValue()
        {
            float oldValue = bonusValue;
            float newValue = modifiers.ComputeValue(baseValue, baseValue + bonusValue, Modifier.ValueTarget.Bonus, false);

            // Update bonus value
            bonusValue = newValue;

            // Add difference to the current value
            if (type.SynchronizeWithModifiers)
                Value = currentValue + (newValue - oldValue);
        }

        public void ApplyModifiers(float timeElapsed)
        {
            float overTimeValues = modifiers.ComputeValue(baseValue, baseValue + bonusValue, Modifier.ValueTarget.CurrentValue, true) * timeElapsed;
            float normalValues = modifiers.ComputeValue(baseValue, baseValue + bonusValue, Modifier.ValueTarget.CurrentValue, false);

            Value = currentValue + overTimeValues + normalValues;
            modifiers.Step(timeElapsed);
        }

        public float BaseValue
        {
            get => baseValue;
            set
            {
                baseValue = value;
                UpdateBonusValue();
            }
        }

        public float Value
        {
            get => Math.Max(MinValue, Math.Min(MaxValue, currentValue));
            set
            {
                float previousValue = currentValue;
                currentValue = Math.Max(MinValue, Math.Min(MaxValue, value));

                if (previousValue != currentValue)
                {
                    if (IsMaximum)
                        onMaximumValueReachedEvent.Invoke();
                    else if (IsMinimum)
                        onMinimumValueReachedEvent.Invoke();
                }
            }
        }

        public float MinValue => type.MinValue;
        public float MaxValue => Math.Min(baseValue + bonusValue, type.MaxValue);
        public float Percent => ((Value - MinValue) * 100) / (MaxValue - MinValue);
        public bool IsMaximum => Value >= MaxValue;
        public bool IsMinimum => Value <= MinValue;
        public UnityEvent OnMinimumValueReachedEvent => onMinimumValueReachedEvent;
        public UnityEvent OnMaximumValueReachedEvent => onMaximumValueReachedEvent;
        public ModifierCollection Modifiers => modifiers;
        public AttributeType Type => type;
    }
}
