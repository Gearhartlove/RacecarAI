using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RacecarAI.Tests {
    
    namespace RacetrackTests {
        
        // Test suite to test if the racetracks provided in the file imported correctly into the Racetrack custom data
        // structure
        [TestClass]
        public class TestRacetrack {
            
            // initialize each of these for each test performed
            private RacetrackParcer parcer;

            [TestInitialize]
            public void TestInitialize() {
                parcer = new RacetrackParcer();
            }
            
            [TestMethod]
            public void Valid_L_Racetrack() {
                Racetrack l_track = parcer.Parse("L-Track");
                // check size
                Assert.AreEqual(37,l_track.XTracksize);
                Assert.AreEqual(11,l_track.YTracksize);
                // check output
                Assert.AreEqual(l_track.ToString(), 
@"#####################################
################################FFFF#
################################....#
################################....#
################################....#
################################....#
#S..................................#
#S..................................#
#S..................................#
#S..................................#
#####################################
");
            }

            [TestMethod]
            public void Valid_R_Track() {
                Racetrack r_track = parcer.Parse("R-Track");
                // check size
                Assert.AreEqual(30,r_track.XTracksize);
                Assert.AreEqual(28,r_track.YTracksize);
                // check output
                Assert.AreEqual(r_track.ToString(),
@"##############################
#########.............########
#####.....................####
###........................###
##......##########.......#####
##.....############.....######
##.....##########.....########
##.....########.....##########
##.....#######.....###########
##.....#####.....#############
##.....###.....###############
##.....#####.....#############
#.....#######.....############
#.....########.....###########
#.....##########.....#########
#.....#############.....######
##.....##############.......##
##.....################.....##
##.....################.....##
##.....################.....##
##.....################.....##
#.....#################.....##
#.....#################.....##
#.....#################.....##
#.....##################.....#
#.....##################.....#
#SSSSS##################FFFFF#
##############################
");
            }

            [TestMethod]
            public void Valid_O_Track() {
                Racetrack o_track = parcer.Parse("O-Track");
                Assert.AreEqual(25,o_track.XTracksize);
                Assert.AreEqual(25,o_track.YTracksize);
                // check output
                Assert.AreEqual(o_track.ToString(), 
@"#########################
####.................####
###...................###
###....###########....###
##....#############....##
#....###############....#
#....###############....#
#....###############....#
#....###############....#
#....###############....#
#SSSS###############....#
####################....#
#FFFF###############....#
#....###############....#
#....###############....#
#....###############....#
#....###############....#
#....###############....#
#....###############....#
#....###############....#
##....#############....##
###....###########....###
###...................###
####.................####
#########################
");
            }
        }
    }
}