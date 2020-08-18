using Do.AttributeSystem;
using UnityEngine;

public class AttributeModifierComponent : MonoBehaviour
{
    [SerializeField] AttributesComponent target = null;
    [SerializeField] Do.AttributeSystem.Scriptables.AttributeModifiers[] attributeModifiers = null;
    [SerializeField] Do.AttributeSystem.Scriptables.AttributeEffect[] effects = null;

    public void Enable()
    {
        foreach (var attributeModifier in attributeModifiers)
            target.AddModifiers(attributeModifier.AttributeType, attributeModifier.Modifiers);

        foreach (var effect in effects)
            target.ApplyEffect(effect);
    }

    public void Disable()
    {
        foreach (var attributeModifier in attributeModifiers)
            target.RemoveModifiers(attributeModifier.AttributeType, attributeModifier.Modifiers);

        foreach (var effect in effects)
            target.UnapplyEffect(effect);
    }
}
