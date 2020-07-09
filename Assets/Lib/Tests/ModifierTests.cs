using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Do.AttributeSystem;

namespace Tests
{
    public class ModifierTests
    {
        [Test]
        public void TestIsExpired()
        {
            Modifier modifier = new Modifier();
            modifier.Duration = 5;
            modifier.TimeElapsed = 0;
            Assert.False(modifier.IsExpired);

            modifier.TimeElapsed = 5;
            Assert.True(modifier.IsExpired);
        }
    }
}
