using System.Collections.Generic;
using UnityEngine;

namespace Do.AttributeSystem.Scriptables
{
    [CreateAssetMenu(fileName = "AttributeModifiers", menuName = "Do/AttributesSystem/AttributeModifiers")]
    public class AttributeModifiers : ScriptableObject
    {
        [SerializeField] protected AttributeType attributeType = null;
        [SerializeField] protected List<Modifier> modifiers = new List<Modifier>();
        public List<Modifier> Modifiers => modifiers;
        public AttributeType AttributeType => attributeType;
    }
}
