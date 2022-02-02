using System;
using BowlingBall.Bowling.Frame;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static BowlingBall.Bowling.Constants;

namespace BowlingBall.Tests
{
    [TestClass]    
    public class GameFixture
    {

        [TestMethod]
        public void Gutter_game_score_should_be_zero_test()
        {
            Game game = new Game();
            Roll(game, 0, 20);
            Assert.AreEqual(0, game.GetScore());
        }

        private void Roll(Game game, int pins, int times)
        {
            for (int i = 0; i < times; i++)
            {
                game.Roll(pins);
            }
        }

        private void ProcessSample(Game game, int[] sample)
        {
            foreach (int i in sample)
            {
                game.Roll(i);
            }
        }

        [TestMethod]
        public void bowling_real_data_test_1()
        {
            Game game = new Game();
            int[] sample = new int[] { 10, 9, 1, 5, 5, 7, 2, 10, 10, 10, 9, 0, 8, 2, 9, 1, 10 };
            ProcessSample(game,sample);
            Assert.AreEqual(187, game.GetScore());
        }

        [TestMethod]
        //Below Test case is to check the Strike Scenario
        public void bowling_real_data_test_2()
        {
            Game game = new Game();
            int[] sample = new int[] { 10, 8, 2, 9, 1, 8, 0, 10, 10, 9,1, 9, 1, 10, 10, 9, 1 };
            ProcessSample(game, sample);
            Assert.AreEqual(202, game.GetScore());
        }

        [TestMethod]
        //Below Test case is to check the Strike Scenario
        public void bowling_real_data_test_3()
        {
            Game game = new Game();
            int[] sample = new int[] { 7,3,10,10,8,1,9,1,8,1,10,9,1,8,2,6,1 };
            ProcessSample(game, sample);
            Assert.AreEqual(164, game.GetScore());
        }

        [TestMethod]
        public void bowling_real_data_test_4()
        {
            Game game = new Game();
            int[] sample = new int[] { 10,10,10,10,10,10,10,10,6,4,10,10,10 };
            ProcessSample(game, sample);
            Assert.AreEqual(276, game.GetScore());
        }

        [TestMethod]
        public void test_game_is_new_frame()
        {
            Game game = new Game();
            game.frameNumber = 0;
            PrivateObject privBase = new PrivateObject(game, new PrivateType(typeof(Game)));
            var retVal = privBase.Invoke("IsNewFrame");
            Assert.AreEqual(true, retVal);

            //Dont create new frame if already at last frame, It will handle 3 chances in last frame
            game.frameNumber = 10;
            retVal = privBase.Invoke("IsNewFrame");
            Assert.AreEqual(false, retVal);

            //No need to create a new frame as it is a strike
            Frame frame = new Frame("test", 10);
            game.ScoreBoard[0] = frame;
            game.frameNumber = 1;
            retVal = privBase.Invoke("IsNewFrame");
            Assert.AreEqual(true, retVal);

            //create new frame
            frame.FirstScore = 5;
            game.ScoreBoard[0] = frame;
            game.frameNumber = 1;
            retVal = privBase.Invoke("IsNewFrame");
            Assert.AreEqual(false, retVal);
        }

        [TestMethod]
        public void test_game_add_new_frame()
        {
            Game game = new Game();
            PrivateObject privBase = new PrivateObject(game, new PrivateType(typeof(Game)));
            privBase.Invoke("AddNewFrame",5);
            Assert.AreNotEqual(null, game.ScoreBoard[0]);
        }

        [TestMethod]
        public void test_game_update_current_frame()
        {
            Game game = new Game();
            Frame frame = new Frame("test", 6);
            game.ScoreBoard[0] = frame;
            game.frameNumber = 1;
            PrivateObject privBase = new PrivateObject(game, new PrivateType(typeof(Game)));
            privBase.Invoke("UpdateCurrentFrame", 5);
            //The second score we are updating as 5 here
            Assert.AreEqual(5, game.ScoreBoard[0].SecondScore);
        }

        [TestMethod]
        public void test_game_get_name_of_frame()
        {
            Game game = new Game();
            PrivateObject privBase = new PrivateObject(game, new PrivateType(typeof(Game)));
            var retVal =privBase.Invoke("GetNameForFrame", 5);
            Assert.AreEqual("Frame_5", retVal);
        }

        [TestMethod]
        public void test_game_add_spare_strike_bonus()
        {
            Game game = new Game();

            //Frame frame = new Frame("test", 10);
            //Frame frame1 = new Frame("test", 10);
            game.ScoreBoard[0] = new Frame("test", 10);
            game.ScoreBoard[1] = new Frame("test1", 2);
            game.frameNumber = 2;

            PrivateObject privBase = new PrivateObject(game, new PrivateType(typeof(Game)));
            privBase.Invoke("AddSpareStrikeBonus", 2);
            Assert.AreEqual(2,game.ScoreBoard[0].Bonus);// Checking Strike Bonus for previous Frame
            Assert.AreEqual(FrameType.Strike, game.ScoreBoard[0].frameType); //Check while adding a Frame with score 10 , the type is automatically set

            game.ScoreBoard[0] = new Frame("test", 5);
            game.ScoreBoard[1] = new Frame("test1", 2);
            game.frameNumber = 2;
            privBase.Invoke("AddSpareStrikeBonus", 2);
            Assert.AreEqual(0, game.ScoreBoard[0].Bonus);// Checking Strike Bonus for previous Frame

            Frame frame = new Frame("test", 5);
            frame.AddScore(5);
            game.ScoreBoard[0] = frame;
            game.ScoreBoard[1] = new Frame("test1", 2);
            game.frameNumber = 2;
            privBase.Invoke("AddSpareStrikeBonus", 2);
            Assert.AreEqual(2, game.ScoreBoard[0].Bonus);// Checking Spare Bonus for previous Frame
            Assert.AreEqual(FrameType.Spare, game.ScoreBoard[0].frameType); //Check while adding a Frame with total score 10 , the type is automatically set
        }

        [TestMethod]
        public void test_game_add_strike_bonus()
        {
            Game game = new Game();

            //Frame frame = new Frame("test", 10);
            //Frame frame1 = new Frame("test", 10);
            game.ScoreBoard[0] = new Frame("test", 10);
            game.ScoreBoard[1] = new Frame("test1", 2);
            game.frameNumber = 2;

            PrivateObject privBase = new PrivateObject(game, new PrivateType(typeof(Game)));
            privBase.Invoke("AddStrikeBonus", 2);
            Assert.AreEqual(2, game.ScoreBoard[0].Bonus);// Checking Strike Bonus for previous Frame
        }


    }
}
