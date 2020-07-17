using UnityEngine;

namespace Do.AttributeSystem
{
    [CreateAssetMenu(fileName = "AttributeModifier", menuName = "Do/AttributesSystem/AttributeModifier")]
    public class AttributeModifier : ScriptableObject
    {
        [SerializeField] protected AttributeType type = null;
        [SerializeField] protected ModifierCollection modifiers = new ModifierCollection();
        public ModifierCollection Modifiers => modifiers;
        public AttributeType Type => type;
    }
}
