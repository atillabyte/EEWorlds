using System.IO;
using NUnit.Framework;

namespace EEWorlds.UnitTests
{
    public class UnitTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void LoadWorldFromTSON()
        {
            var world = World.LoadFromTSON(File.ReadAllText(Path.Combine("includes", "PW_Dc2Pqq8a0I.tson")));
            Assert.Pass();
        }

        [Test]
        public void LoadWorldFromJSON()
        {
            var world = World.LoadFromJSON(File.ReadAllText(Path.Combine("includes", "PW9ZxUoVbBb0I.json")));
            Assert.Pass();
        }
    }
}