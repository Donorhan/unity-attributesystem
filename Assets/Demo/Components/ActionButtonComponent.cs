using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button), typeof(AttributeModifierComponent))]
public class ActionButtonComponent : MonoBehaviour
{
    AttributeModifierComponent[] attributeModifierComponents = null;
    [SerializeField] Color enabledButtonColor = Color.green;
    [SerializeField] Color disabledButtonColor = Color.red;
    [SerializeField] Color defaultButtonColor = Color.white;
    [SerializeField] bool equipeable = true;
    Button button = null;
    bool activated = false;

    void Start()
    {
        attributeModifierComponents = GetComponents<AttributeModifierComponent>();
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
        UpdateButtonColor();
    }

    public void OnClick()
    {
        if (!equipeable)
            foreach (AttributeModifierComponent attributeModifier in attributeModifierComponents)
                attributeModifier.Enable();
        else
            Toggle();
    }

    void Toggle()
    {
        if (!activated)
            foreach (AttributeModifierComponent attributeModifier in attributeModifierComponents)
                attributeModifier.Enable();
        else
            foreach (AttributeModifierComponent attributeModifier in attributeModifierComponents)
                attributeModifier.Disable();

        activated = !activated;
        UpdateButtonColor();
    }

    void UpdateButtonColor()
    {
        Color color = !equipeable ? defaultButtonColor : (activated ? disabledButtonColor : enabledButtonColor);

        ColorBlock colors = button.colors;
        colors.highlightedColor = color;
        colors.normalColor = color;
        colors.pressedColor = color;
        colors.selectedColor = color;
        button.colors = colors;
    }
}
