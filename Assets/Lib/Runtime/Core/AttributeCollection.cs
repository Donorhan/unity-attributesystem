using System;
using System.Collections.Generic;
using UnityEngine;

namespace Do.AttributeSystem
{
    [Serializable]
    public class AttributeCollection
    {
        [SerializeField] List<Attribute> attributes = new List<Attribute>();

        public Attribute Find(string identifier) => attributes.Find((Attribute attribute) => attribute.Type.Identifier == identifier);
        public Attribute Find(AttributeType attributeType) => Find(attributeType.Identifier);

        public Attribute FindOrCreate(AttributeType attributeType)
        {
            Attribute attribute = Find(attributeType);
            if (attribute != null)
                return attribute;

            attribute = new Attribute(attributeType);
            attributes.Add(attribute);

            return attribute;
        }

        public void ApplyModifiers(float timeElapsed)
        {
            foreach (Attribute attribute in attributes)
                attribute.ApplyModifiers(timeElapsed);
        }

        public void SetToMaxValue()
        {
            foreach (Attribute attribute in attributes)
                attribute.SetToMaxValue();
        }

        public List<Attribute> Attributes => attributes;
    }
}
