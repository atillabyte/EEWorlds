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
        public void LoadWorldFromEELEVEL()
        {
            var world = WorldManager.LoadFromEEditor(File.ReadAllBytes(Path.Combine("includes", "PWfGHlYfF6cUI.eelevel")), EELevelVersion.V6);
            Assert.Pass();
        }


        [Test]
        public void LoadWorldFromEELVL()
        {
            var world = WorldManager.LoadFromEELVL(File.ReadAllBytes(Path.Combine("includes", "PWXFk-UKg_b0I.eelvl")));
            Assert.Pass();
        }

        [Test]
        public void LoadWorldFromTSON()
        {
            var world = WorldManager.LoadFromTSON(File.ReadAllText(Path.Combine("includes", "PW_Dc2Pqq8a0I.tson")));
            Assert.Pass();
        }

        [Test]
        public void LoadWorldFromJSON()
        {
            var world = WorldManager.LoadFromJSON(File.ReadAllText(Path.Combine("includes", "PW9ZxUoVbBb0I.json")));
            Assert.Pass();
        }
    }
}