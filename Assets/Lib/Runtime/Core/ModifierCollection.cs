using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Do.AttributeSystem
{
    [Serializable]
    public class ModifierCollection
    {
        [SerializeField] List<Modifier> modifiers = new List<Modifier>();
        public event Action OnModifiersAdded;
        public event Action OnModifiersRemoved;

        public float ComputeValue(float baseValue, float totalValue, Modifier.ValueTarget target, bool overTime)
        {
            float rawValueAdd = 0;
            float rawValuePercent = 0;
            float totalValueAdd = 0;
            float totalValuePercent = 0;

            var selectedModifiers = modifiers.Where((Modifier modifier) => modifier.Target == target && modifier.Overtime == overTime);
            foreach (Modifier modifier in selectedModifiers)
            {
                if (modifier.Type == Modifier.ValueType.Add)
                {
                    if (modifier.Scope == Modifier.ValueScope.Base)
                        rawValueAdd += modifier.Value;
                    else if (modifier.Scope == Modifier.ValueScope.Total)
                        totalValueAdd += modifier.Value;
                }
                else if (modifier.Type == Modifier.ValueType.Percent)
                {
                    if (modifier.Scope == Modifier.ValueScope.Base)
                        rawValuePercent += modifier.Value;
                    else if (modifier.Scope == Modifier.ValueScope.Total)
                        totalValuePercent += modifier.Value;
                }
            }

            float addValue = (baseValue * rawValuePercent) + rawValueAdd;
            float percentValue = ((baseValue + addValue) * totalValuePercent) + totalValueAdd;

            return addValue + percentValue;
        }

        public void Step(float timeElapsed)
        {
            foreach (Modifier modifier in modifiers)
                modifier.TimeElapsed += timeElapsed;

            RemoveExpiredModifiers();
        }

        public List<Modifier> Modifiers => modifiers;

        public Modifier Add(Modifier modifier, bool notify = true)
        {
            if (modifiers.Contains(modifier))
                return modifier;

            modifier.TimeElapsed = 0;
            modifiers.Add(modifier);

            if (notify && OnModifiersAdded != null)
                OnModifiersAdded();

            return modifier;
        }

        public void Add(ModifierCollection modifierCollection)
        {
            foreach (Modifier modifier in modifierCollection.Modifiers)
                Add(modifier, false);

            if (OnModifiersAdded != null)
                OnModifiersAdded();
        }

        public bool Remove(Modifier modifier, bool notify = true)
        {
            if (modifier.Constant)
                return false;

            bool removed = modifiers.Remove(modifier);
            if (removed && notify && OnModifiersRemoved != null)
                OnModifiersRemoved();

            return removed;
        }

        public void Remove(ModifierCollection modifierCollection)
        {
            foreach (Modifier modifier in modifierCollection.Modifiers)
                Remove(modifier, false);

            if (OnModifiersRemoved != null)
                OnModifiersRemoved();
        }

        public int RemoveAllModifiers()
        {
            int modifiersCount = 0;
            foreach (Modifier modifier in modifiers)
                modifiersCount += Remove(modifier) ? 1 : 0;

            if (OnModifiersRemoved != null)
                OnModifiersRemoved();

            return modifiersCount;
        }

        public int RemoveExpiredModifiers()
        {
            int removedCount = modifiers.RemoveAll(modifier => modifier.IsExpired);
            if (removedCount > 0 && OnModifiersRemoved != null)
                OnModifiersRemoved();

            return removedCount;
        }

        public bool HasNonConstantModifiers() => modifiers.FindIndex(modifier => modifier.Constant == false) != -1;
        public bool Remove(string identifier) => Remove(Find(identifier));
        public Modifier Find(string identifier) => modifiers.Find((Modifier modifier) => modifier.Identifier == identifier);
    }
}
