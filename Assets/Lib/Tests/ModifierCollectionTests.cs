using NUnit.Framework;
using Do.AttributeSystem;

namespace Tests
{
    public class ModifierCollectionTests
    {
        ModifierCollection modifierCollection = null;

        [SetUp]
        public void Setup()
        {
            modifierCollection = new ModifierCollection();
        }

        [Test]
        public void AddModifier()
        {
            modifierCollection.Add(new Modifier());

            Assert.AreEqual(modifierCollection.Modifiers.Count, 1);
        }

        [Test]
        public void ClearModifiers()
        {
            for (int i = 0; i < 3; i++)
                modifierCollection.Add(new Modifier());

            Assert.AreEqual(3, modifierCollection.Modifiers.Count);
            Assert.AreEqual(3, modifierCollection.RemoveAllModifiers());
            Assert.AreEqual(0, modifierCollection.Modifiers.Count);
        }

        [Test]
        public void FindFromIdentifier()
        {
            Modifier modifier = modifierCollection.Add(new Modifier());

            Assert.Null(modifierCollection.Find("hello"));
            Assert.NotNull(modifierCollection.Find(modifier.Identifier));
        }

        [Test]
        public void RemovedExpiredModifiers()
        {
            Modifier modifier = modifierCollection.Add(new Modifier());
            modifier.Duration = 10;
            modifier.TimeElapsed = 3;

            Modifier modifier2 = modifierCollection.Add(new Modifier());
            modifier2.Duration = 5;
            modifier2.TimeElapsed = 4.99f;

            Assert.AreEqual(0, modifierCollection.RemoveExpiredModifiers());

            modifier.TimeElapsed = 9;
            modifier2.TimeElapsed = 5;
            Assert.AreEqual(1, modifierCollection.RemoveExpiredModifiers());

            modifier.TimeElapsed = 11;
            Assert.AreEqual(1, modifierCollection.RemoveExpiredModifiers());
        }

        [Test]
        public void ComputeValueForBaseValue()
        {
            modifierCollection.Add(new Modifier(Modifier.ValueTarget.Bonus, Modifier.ValueScope.Base, Modifier.ValueType.Add, 10));
            Assert.AreEqual(10, modifierCollection.ComputeValue(0, 0, Modifier.ValueTarget.Bonus, false));

            modifierCollection.Add(new Modifier(Modifier.ValueTarget.Bonus, Modifier.ValueScope.Base, Modifier.ValueType.Add, 25));
            Assert.AreEqual(35, modifierCollection.ComputeValue(0, 0, Modifier.ValueTarget.Bonus, false));

            modifierCollection.Add(new Modifier(Modifier.ValueTarget.Bonus, Modifier.ValueScope.Base, Modifier.ValueType.Add, -8.5f));
            Assert.AreEqual(26.5f, modifierCollection.ComputeValue(0, 0, Modifier.ValueTarget.Bonus, false));

            modifierCollection.Add(new Modifier(Modifier.ValueTarget.Bonus, Modifier.ValueScope.Total, Modifier.ValueType.Percent, 0.5f));
            Assert.AreEqual(39.75f, modifierCollection.ComputeValue(0, 0, Modifier.ValueTarget.Bonus, false));

            modifierCollection.Add(new Modifier(Modifier.ValueTarget.Bonus, Modifier.ValueScope.Total, Modifier.ValueType.Percent, -0.5f));
            Assert.AreEqual(26.5f, modifierCollection.ComputeValue(0, 0, Modifier.ValueTarget.Bonus, false));

            modifierCollection.Add(new Modifier(Modifier.ValueTarget.Bonus, Modifier.ValueScope.Total, Modifier.ValueType.Percent, -0.25f));
            Assert.AreEqual(19.875f, modifierCollection.ComputeValue(0, 0, Modifier.ValueTarget.Bonus, false));
        }
    }
}
