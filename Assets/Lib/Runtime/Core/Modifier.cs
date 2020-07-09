using System;
using UnityEngine;

namespace Do.AttributeSystem
{
    [Serializable]
    public class Modifier
    {
        public enum ValueType
        {
            Add = 0,
            Percent,
        }

        [System.Serializable]
        public enum ValueScope
        {
            Base = 0,
            Total,
        }

        [System.Serializable]
        public enum ValueTarget
        {
            Bonus = 0,
            CurrentValue,
        }

        string identifier = Guid.NewGuid().ToString();

        [SerializeField]
        ValueScope scope = ValueScope.Base;

        [SerializeField]
        ValueTarget target = ValueTarget.Bonus;

        [SerializeField]
        ValueType type = ValueType.Add;

        [SerializeField]
        float value = 0;

        [SerializeField]
        [Tooltip("-1 for modifier applied infinitely")]
        float duration = -1;

        [SerializeField]
        bool overTime = false;

        [SerializeField]
        bool constant = false;

        float timeElapsed = 0;

        public Modifier() { }

        public Modifier(ValueTarget target, ValueScope scope, ValueType type, float value, float duration = -1, bool overTime = false, bool constant = false)
        {
            this.target = target;
            this.scope = scope;
            this.type = type;
            this.value = value;
            this.duration = duration;
            this.overTime = overTime;
            this.constant = constant;
        }

        public ValueScope Scope
        {
            get => scope;
            set => scope = value;
        }

        public ValueTarget Target
        {
            get => target;
            set => target = value;
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

        public float Duration
        {
            get => duration;
            set => this.duration = value;
        }

        public float TimeElapsed
        {
            get => timeElapsed;
            set => this.timeElapsed = value;
        }

        public bool Constant
        {
            get => constant;
            set => this.constant = value;
        }

        public bool IsExpired => (duration < 0 ? false : timeElapsed >= duration);
        public string Identifier => identifier;
        public bool Overtime => overTime;
    }
}
