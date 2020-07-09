using UnityEngine;

namespace Do.AttributeSystem
{
    [CreateAssetMenu(fileName = "Type", menuName = "Do/AttributesSystem/Type")]
    public class AttributeType : ScriptableObject
    {
        [SerializeField]
        Sprite icon = null;

        [SerializeField]
        string label = "Attribute";

        [SerializeField]
        string identifier = "attribute";

        [SerializeField]
        float maxValue = 10;

        [SerializeField]
        float minValue = 0;

        [SerializeField]
        [Tooltip("Change the value of the current value when a modifier is added or removed")]
        bool synchronizeWithModifiers = true;

        public Sprite Icon
        {
            get => icon;
            set => icon = value;
        }

        public string Label
        {
            get => label;
            set => label = value;
        }

        public string Identifier
        {
            get => identifier;
            set => identifier = value;
        }

        public float MaxValue
        {
            get => maxValue;
            set => maxValue = value;
        }

        public float MinValue
        {
            get => minValue;
            set => minValue = value;
        }

        public bool SynchronizeWithModifiers
        {
            get => synchronizeWithModifiers;
            set => synchronizeWithModifiers = value;
        }
    }
}
