namespace Do.AttributeSystem
{
    public struct ModifierCalculator
    {
        float baseValue;
        float rawValueAdd;
        float rawValuePercent;
        float totalValueAdd;
        float totalValuePercent;

        public ModifierCalculator(float baseValue)
        {
            this.baseValue = baseValue;
            rawValueAdd = 0;
            rawValuePercent = 0;
            totalValueAdd = 0;
            totalValuePercent = 0;
        }

        public void AddModifier(Modifier modifier)
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

        public void Reset()
        {
            rawValueAdd = 0;
            rawValuePercent = 0;
            totalValueAdd = 0;
            totalValuePercent = 0;
        }

        public float BaseValue
        {
            get => baseValue;
            set => baseValue = value;
        }

        public float AddValue => (baseValue * rawValuePercent) + rawValueAdd;
        public float PercentValue => ((baseValue + AddValue) * totalValuePercent) + totalValueAdd;
        public float Total => AddValue + PercentValue;
    }
}
