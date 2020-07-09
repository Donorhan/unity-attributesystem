using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Do.AttributeSystem;

namespace Tests
{
    public class AttributeTests
    {
        AttributeType CreateType(string identifier, string label, float min = 0, float max = 100, bool synchronizeWithModifiers = true)
        {
            AttributeType type = ScriptableObject.CreateInstance<AttributeType>();
            type.Identifier = identifier;
            type.Label = label;
            type.MinValue = min;
            type.MaxValue = max;
            type.SynchronizeWithModifiers = synchronizeWithModifiers;

            return type;
        }

        Attribute CreateAttribute(AttributeType type, float baseValue, float value)
        {
            Attribute attribute = new Attribute(type);
            attribute.BaseValue = baseValue;
            attribute.Value = value;

            return attribute;
        }

        Attribute attackAttribute = null;

        [SetUp]
        public void Setup()
        {
            attackAttribute = CreateAttribute(CreateType("Attack", "attack"), 100, 50);
        }

        [Test]
        public void MaximumEventIsInvoked()
        {
            int callCounter = 0;
            attackAttribute.OnMaximumValueReachedEvent.AddListener(() => callCounter++);
            Assert.AreEqual(0, callCounter);
            attackAttribute.Value = 50;
            Assert.AreEqual(0, callCounter);
            attackAttribute.Value = 100;
            Assert.AreEqual(1, callCounter);
            attackAttribute.Value = 101;
            Assert.AreEqual(1, callCounter);
            attackAttribute.Value = 99;
            Assert.AreEqual(1, callCounter);
            attackAttribute.Value = 100;
            Assert.AreEqual(2, callCounter);
        }

        [Test]
        public void MinimumEventIsInvoked()
        {
            int callCounter = 0;
            attackAttribute.OnMinimumValueReachedEvent.AddListener(() => callCounter++);
            Assert.AreEqual(0, callCounter);
            attackAttribute.Value = 50;
            Assert.AreEqual(0, callCounter);
            attackAttribute.Value = 0;
            Assert.AreEqual(1, callCounter);
            attackAttribute.Value = -1;
            Assert.AreEqual(1, callCounter);
            attackAttribute.Value = 1;
            Assert.AreEqual(1, callCounter);
            attackAttribute.Value = 0;
            Assert.AreEqual(2, callCounter);
        }

        [Test]
        public void CheckPercent()
        {
            attackAttribute.Value = 50;
            Assert.AreEqual(50.0f, attackAttribute.Percent);
            attackAttribute.Value = 0;
            Assert.AreEqual(0.0f, attackAttribute.Percent);
            attackAttribute.Value = 100;
            Assert.AreEqual(100.0f, attackAttribute.Percent);
            attackAttribute.Value = 17.5f;
            Assert.AreEqual(17.5f, attackAttribute.Percent);

            Attribute defenseAttribute = CreateAttribute(CreateType("Defense", "defense", 50, 100), 100, 75);
            defenseAttribute.Value = 50;
            Assert.AreEqual(0.0f, defenseAttribute.Percent);
            defenseAttribute.Value = 75;
            Assert.AreEqual(50.0f, defenseAttribute.Percent);
            defenseAttribute.Value = 100;
            Assert.AreEqual(100.0f, defenseAttribute.Percent);
        }

        [Test]
        public void SetToMinValue()
        {
            attackAttribute.Value = 50;
            Assert.AreEqual(50.0f, attackAttribute.Value);
            attackAttribute.SetToMinValue();
            Assert.AreEqual(0.0f, attackAttribute.Value);
        }

        [Test]
        public void SetToMaxValue()
        {
            attackAttribute.Value = 50;
            Assert.AreEqual(50.0f, attackAttribute.Value);
            attackAttribute.SetToMaxValue();
            Assert.AreEqual(100.0f, attackAttribute.Value);
        }

        [Test]
        public void UpdateBonus()
        {
            attackAttribute.Modifiers.Add(new Modifier(Modifier.ValueTarget.Bonus, Modifier.ValueScope.Base, Modifier.ValueType.Add, 10));
            Assert.AreEqual(60.0f, attackAttribute.Value);
            attackAttribute.Modifiers.Add(new Modifier(Modifier.ValueTarget.Bonus, Modifier.ValueScope.Base, Modifier.ValueType.Add, -15));
            Assert.AreEqual(45.0f, attackAttribute.Value);

            // Base value is not at 95, put current value to the maximum value
            attackAttribute.SetToMaxValue();
            attackAttribute.Modifiers.Add(new Modifier(Modifier.ValueTarget.Bonus, Modifier.ValueScope.Total, Modifier.ValueType.Percent, 0.05f));
            Assert.AreEqual(99.75f, attackAttribute.Value); // 95 + 5%
        }

        [Test]
        public void TestSync()
        {
            Attribute defenseAttribute = CreateAttribute(CreateType("Defense", "defense", 0, 200, false), 100, 100);
            defenseAttribute.Modifiers.Add(new Modifier(Modifier.ValueTarget.Bonus, Modifier.ValueScope.Base, Modifier.ValueType.Add, 10));
            Assert.AreEqual(100.0f, defenseAttribute.Value);
            Assert.AreEqual(110.0f, defenseAttribute.MaxValue);

            Attribute speedAttribute = CreateAttribute(CreateType("Defense", "defense", 0, 200, true), 100, 100);
            speedAttribute.Modifiers.Add(new Modifier(Modifier.ValueTarget.Bonus, Modifier.ValueScope.Base, Modifier.ValueType.Add, 10));
            Assert.AreEqual(110.0f, speedAttribute.Value);
            Assert.AreEqual(110.0f, speedAttribute.MaxValue);
        }
    }
}
