using Do.AttributeSystem;
using UnityEngine;

public class AttributeModifierComponent : MonoBehaviour
{
    [SerializeField] AttributesComponent targetAttributesComponent = null;
    [SerializeField] AttributeModifier[] attributeModifiers = null;

    public void Enable()
    {
        AttributeCollection targetAttributes = targetAttributesComponent.Attributes;

        foreach (AttributeModifier attributeModifier in attributeModifiers)
        {
            var attribute = targetAttributes.Find(attributeModifier.Type);
            if (attribute == null)
                return;

            attribute.Modifiers.Add(attributeModifier.Modifiers);
        }
    }

    public void Disable()
    {
        AttributeCollection targetAttributes = targetAttributesComponent.Attributes;

        foreach (AttributeModifier attributeModifier in attributeModifiers)
        {
            var attribute = targetAttributes.Find(attributeModifier.Type);
            if (attribute == null)
                return;

            attribute.Modifiers.Remove(attributeModifier.Modifiers);
        }
    }
}
