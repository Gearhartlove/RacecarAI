using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RacecarAI.Tests {
    namespace RacecarTests {

        [TestClass]
        public class TestRacecar {
            
            // initialize each of these for each test performed
            private RacecarV2 racecar;

            [TestInitialize]
            public void TestInitialize() {
                racecar = new RacecarV2();
            }

            [TestMethod]
            public void Invalid_X_Velocity() {
                // cannot assign a velocity to something
                
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