using NUnit.Framework;
using Do.AttributeSystem;
using UnityEngine;

namespace Tests
{
    public class AttributeCollectionTests
    {
        AttributeCollection attributeCollection = null;

        [SetUp]
        public void Setup()
        {
            attributeCollection = new AttributeCollection();
        }

        AttributeType CreateType(string identifier, string label)
        {
            AttributeType type = ScriptableObject.CreateInstance<AttributeType>();
            type.Identifier = identifier;
            type.Label = label;

            return type;
        }

        [Test]
        public void FindOrCreate()
        {
            Assert.AreEqual(attributeCollection.Attributes.Count, 0);
            attributeCollection.FindOrCreate(CreateType("health", "Health"));

            Assert.AreEqual(attributeCollection.Attributes.Count, 1);
        }

        [Test]
        public void FindFromIdentifier()
        {
            attributeCollection.FindOrCreate(CreateType("health", "Health"));

            Assert.NotNull(attributeCollection.Find("health"));
        }

        [Test]
        public void FindFromType()
        {
            AttributeType type = CreateType("health", "Health");
            attributeCollection.FindOrCreate(CreateType("health", "Health"));

            Assert.NotNull(attributeCollection.Find(type));
        }
    }
}
