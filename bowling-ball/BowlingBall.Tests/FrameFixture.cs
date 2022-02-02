using BowlingBall.Bowling.Frame;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BowlingBall.Tests
{
    [TestClass]
    public class FrameFixture
    {
        [TestMethod]
        public void test_frame_check_set_frame_type()
        {
            Frame frame = new Frame("test",10);

            PrivateObject privBase = new PrivateObject(frame, new PrivateType(typeof(Frame)));
            privBase.Invoke("checkAndSetFrameType");
            Assert.AreEqual(Bowling.Constants.FrameType.Strike, frame.frameType);

            frame.Score = 10;
            frame.SecondScore = -1;
            privBase.Invoke("checkAndSetFrameType");
            Assert.AreEqual(Bowling.Constants.FrameType.Strike, frame.frameType);


            frame.Score = 10;
            frame.SecondScore = 2;
            privBase.Invoke("checkAndSetFrameType");
            Assert.AreEqual(Bowling.Constants.FrameType.Spare, frame.frameType);
        }

        [TestMethod]
        public void test_frame_add_score()
        {
            Frame frame = new Frame("test", 8);

            PrivateObject privBase = new PrivateObject(frame, new PrivateType(typeof(Frame)));
            privBase.Invoke("AddScore",2);
            Assert.AreEqual(Bowling.Constants.FrameType.Spare, frame.frameType); // As Score is total 10 the type should be spare
            Assert.AreEqual(8, frame.FirstScore);
            Assert.AreEqual(2, frame.SecondScore);           
        }

        [TestMethod]
        public void test_frame_add_bonus()
        {
            Frame frame = new Frame("test", 2);

            PrivateObject privBase = new PrivateObject(frame, new PrivateType(typeof(Frame)));
            privBase.Invoke("AddBonus", 2);
            Assert.AreEqual(4, frame.Score); //We will get the final result after adding Bonus
            Assert.AreEqual(2, frame.Bonus); //As Initially Bonus was 0 the current bonus will be 2

            privBase.Invoke("AddBonus", 2);
            Assert.AreEqual(6, frame.Score); //After adding Bonus the score should increase
            Assert.AreEqual(4, frame.Bonus); //More bonus should be added to current bonus
        }

        [TestMethod]
        public void test_frame_track_score()
        {
            Frame frame = new Frame("test", 2);

            PrivateObject privBase = new PrivateObject(frame, new PrivateType(typeof(Frame)));
            privBase.Invoke("TrackScore", 3);
            Assert.AreEqual(2, frame.FirstScore); //We will get the final result after adding Bonus
            Assert.AreEqual(3, frame.SecondScore); //After adding new score it should get added to Second Score if Secodn score is blank

            privBase.Invoke("TrackScore", 4);
            Assert.AreEqual(4, frame.ThirdScore); //After adding new score if second score is already filled add it to third score
        }
    }
}
