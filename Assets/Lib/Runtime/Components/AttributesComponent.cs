using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Do.AttributeSystem
{
    public class AttributesComponent : MonoBehaviour
    {
        [SerializeField] AttributeCollection attributes = new AttributeCollection();
        List<Scriptables.AttributeEffect> effects = new List<Scriptables.AttributeEffect>();

        public bool ApplyEffect(Scriptables.AttributeEffect effect)
        {
            Attribute attribute = attributes.Find(effect.AttributeType);
            if (attribute == null)
                return false;

            if (effect.Type != Scriptables.AttributeEffect.EffectType.Steps)
                effects.Add(effect);
            else
                StartCoroutine(RunEffect(attribute, effect));

            return true;
        }

        public bool UnapplyEffect(Scriptables.AttributeEffect effect)
        {
            if (effect.Type == Scriptables.AttributeEffect.EffectType.Steps)
            {
                Debug.LogWarning("Can't undo an effect that is a sequence");

                return false;
            }

            return effects.Remove(effect);
        }

        public bool AddModifier(AttributeType targetedAttribute, Modifier modifier)
        {
            Attribute attribute = attributes.Find(targetedAttribute);
            if (attribute == null)
                return false;

            attribute.AddModifier(modifier);

            return true;
        }

        public bool AddModifiers(AttributeType targetedAttribute, List<Modifier> modifiers)
        {
            Attribute attribute = attributes.Find(targetedAttribute);
            if (attribute == null)
                return false;

            attribute.AddModifiers(modifiers);

            return true;
        }

        public bool RemoveModifier(AttributeType targetedAttribute, Modifier modifier)
        {
            Attribute attribute = attributes.Find(targetedAttribute);
            if (attribute == null)
                return false;

            attribute.RemoveModifier(modifier);

            return true;
        }

        public bool RemoveModifiers(AttributeType targetedAttribute, List<Modifier> modifiers)
        {
            Attribute attribute = attributes.Find(targetedAttribute);
            if (attribute == null)
                return false;

            attribute.RemoveModifiers(modifiers);

            return true;
        }

        void LateUpdate()
        {
            Attribute attribute = null;
            foreach (Scriptables.AttributeEffect effect in effects)
            {
                attribute = attributes.Find(effect.AttributeType);
                foreach (Modifier modifier in effect.Modifiers)
                    attribute.ApplyModifier(modifier, Time.deltaTime);
            }
        }

        IEnumerator RunEffect(Attribute attribute, Scriptables.AttributeEffect effect)
        {
            foreach (var modifier in effect.Modifiers)
            {
                attribute.ApplyModifier(modifier);
                yield return new WaitForSeconds(effect.TimeBetweenSteps);
            }
        }

        public AttributeCollection Attributes => attributes;
    }
}
