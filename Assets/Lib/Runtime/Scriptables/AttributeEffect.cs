using System;
using UnityEngine;

namespace Do.AttributeSystem.Scriptables
{
    [CreateAssetMenu(fileName = "AttributeEffect", menuName = "Do/AttributesSystem/AttributeEffect")]
    public class AttributeEffect : AttributeModifiers
    {
        [Serializable]
        public enum EffectType
        {
            Steps,
            Duration,
        }

        [SerializeField] EffectType type = EffectType.Steps;
        [SerializeField] float duration = -1;
        [SerializeField] float timeBetweenSteps = 0;

        public EffectType Type => type;

        public float Duration
        {
            get => duration;
            set => duration = value;
        }

        public float TimeBetweenSteps
        {
            get => timeBetweenSteps;
            set => timeBetweenSteps = value;
        }
    }
}
