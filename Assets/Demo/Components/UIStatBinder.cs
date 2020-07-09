using Do.AttributeSystem;
using UnityEngine;
using UnityEngine.UI;

public class UIStatBinder : MonoBehaviour
{
    [SerializeField] AttributesComponent attributesComponent = null;
    [SerializeField] AttributeType type = null;
    [SerializeField] bool showPercent = false;
    Attribute attribute = null;
    Text text = null;

    void Start()
    {
        text = GetComponent<Text>();
        attribute = attributesComponent.Attributes.Find(type);
    }

    void LateUpdate()
    {
        if (attribute == null)
            return;

        string value = attribute.Value.ToString("F2");
        string percent = showPercent ? " (" + attribute.Percent.ToString("F2") + "%)" : "";
        text.text = type.name + " : " + value + percent;
    }
}
