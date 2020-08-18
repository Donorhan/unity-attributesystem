using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Do.AttributeSystem
{
    [Serializable]
    public class Attribute
    {
        [Header("Base")]
        [SerializeField] protected AttributeType type = null;
        [SerializeField] protected List<Modifier> modifiers = new List<Modifier>();
        ModifierCalculator modifiersCalculator = new ModifierCalculator(0);

        [Header("Values")]
        [SerializeField] float baseValue = 0;
        [SerializeField] float currentValue = 0;
        float bonusValue = 0;

        [Header("Events")]
        [SerializeField] UnityEvent onModifierAdded = new UnityEvent();
        [SerializeField] UnityEvent onModifierRemoved = new UnityEvent();
        [SerializeField] UnityEvent<float> onMinimumValueReachedEvent = new UnityEvent<float>();
        [SerializeField] UnityEvent<float> onMaximumValueReachedEvent = new UnityEvent<float>();
        [SerializeField] UnityEvent<float, float> onValueChangedEvent = new UnityEvent<float, float>();

        public Attribute()
        {
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

        public void AddModifier(Modifier modifier)
        {
            modifiers.Add(modifier);
            onModifierAdded.Invoke();
            UpdateBonusValue();
        }

        public void AddModifiers(List<Modifier> modifiers)
        {
            foreach (Modifier modifier in modifiers)
                this.modifiers.Add(modifier);

            onModifierAdded.Invoke();
            UpdateBonusValue();
        }

        public void RemoveModifier(Modifier modifier)
        {
            modifiers.Remove(modifier);
            onModifierRemoved.Invoke();
            UpdateBonusValue();
        }

        public void RemoveModifiers(List<Modifier> modifiers)
        {
            int modifiersAmount = modifiers.Count;
            for (int i = modifiersAmount - 1; i >= 0; i--)
                this.modifiers.Remove(modifiers[i]);

            onModifierRemoved.Invoke();
            UpdateBonusValue();
        }

        public void ApplyModifier(Modifier modifier, float scaler = 1)
        {
            if (modifier.Type == Modifier.ValueType.Add)
                Value += modifier.Value * scaler;
            else
                Value += Value * ((modifier.Value * scaler) / 100.0f);
        }

        public void ApplyModifiers(List<Modifier> modifiers, float scaler = 1)
        {
            foreach (Modifier modifier in modifiers)
                ApplyModifier(modifier, scaler);
        }

        public float ComputeValue(float baseValue)
        {
            modifiersCalculator.BaseValue = baseValue;
            modifiersCalculator.Reset();

            foreach (Modifier modifier in modifiers)
                modifiersCalculator.AddModifier(modifier);

            return modifiersCalculator.Total;
        }

        public void SetToMaxValue() => Value = MaxValue;
        public void SetToMinValue() => Value = MinValue;

        void UpdateBonusValue()
        {
            float oldValue = bonusValue;
            float newValue = ComputeValue(baseValue);

            // Update bonus value
            bonusValue = newValue;

            // Add difference to the current value
            if (type.SynchronizeWithModifiers)
                Value = currentValue + (newValue - oldValue);
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
                    onValueChangedEvent.Invoke(previousValue, currentValue);

                    if (IsMaximum)
                        onMaximumValueReachedEvent.Invoke(MaxValue);
                    else if (IsMinimum)
                        onMinimumValueReachedEvent.Invoke(MinValue);
                }
            }
        }

        public float MinValue => type.MinValue;
        public float MaxValue => Math.Min(baseValue + bonusValue, type.MaxValue);
        public float Percent => ((Value - MinValue) * 100) / (MaxValue - MinValue);
        public bool IsMaximum => Value >= MaxValue;
        public bool IsMinimum => Value <= MinValue;
        public UnityEvent<float> OnMinimumValueReachedEvent => onMinimumValueReachedEvent;
        public UnityEvent<float> OnMaximumValueReachedEvent => onMaximumValueReachedEvent;
        public UnityEvent<float, float> OnValueChangedEvent => onValueChangedEvent;
        public List<Modifier> Modifiers => modifiers;
        public AttributeType Type => type;
    }
}
