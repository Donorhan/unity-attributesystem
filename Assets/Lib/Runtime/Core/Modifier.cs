using System;
using UnityEngine;

namespace Do.AttributeSystem
{
    [Serializable]
    public class Modifier
    {
        [Serializable]
        public enum ValueScope
        {
            Base = 0,
            Total,
        }

        [Serializable]
        public enum ValueType
        {
            Add = 0,
            Percent,
        }

        [SerializeField] ValueScope scope = ValueScope.Base;
        [SerializeField] protected ValueType type = ValueType.Add;
        [SerializeField] protected float value = 0;

        public ValueScope Scope
        {
            get => scope;
            set => scope = value;
        }

        public ValueType Type
        {
            get => type;
            set => type = value;
        }

        public float Value
        {
            get => value;
            set => this.value = value;
        }
    }
}
