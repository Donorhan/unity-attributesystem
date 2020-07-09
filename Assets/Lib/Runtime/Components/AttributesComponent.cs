using UnityEngine;

namespace Do.AttributeSystem
{
    public class AttributesComponent : MonoBehaviour
    {
        [SerializeField] AttributeCollection attributes = new AttributeCollection();

        void LateUpdate() => attributes.ApplyModifiers(Time.deltaTime);

        public AttributeCollection Attributes => attributes;
    }
}
