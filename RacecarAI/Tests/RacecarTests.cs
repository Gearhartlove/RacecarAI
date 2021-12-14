using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RacecarAI.Tests {
    namespace RacecarTests {

        [TestClass]
        public class TestRacecar {
            
            // initialize each of these for each test performed
            private Racecar racecar;

            [TestInitialize]
            public void TestInitialize() {
                racecar = new Racecar(0, 0, 0, 0, 0, 0);
            }

            [TestMethod]
            public void Invalid_X_Velocity() {
                // cannot assign a velocity to something
                
            }

            [TestMethod]
            public void equality() {
                Assert.AreEqual(racecar, new Racecar(0, 0, 0, 0, 0, 0));
            }
            // Test 
           //  [TestMethod]
           //  public void Valid_Acceleration() {
           //      
           //  }
           //  
           // [TestMethod]
           // public void Invalid_Acceleration() {
           //     
           // }
        }
    }
}