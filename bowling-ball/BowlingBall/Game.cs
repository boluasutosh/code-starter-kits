using BowlingBall.Bowling.Frame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BowlingBall.Bowling.Constants;

namespace BowlingBall
{
    public class Game
    {

        public Frame[] ScoreBoard = new Frame[10];
        public int frameNumber = 0;
        //public static int finalScore = 0;
        public void Roll(int pins)
        {
            // Add your logic here. Add classes as needed.
            //Check if we need to create a new frame
            if (IsNewFrame())
            {                
                //If Player has not scored a strike or We are not already on the last round
                AddNewFrame(pins);               
                AddBonusForContiniousStrike(frameNumber-1, pins, 1); // Handle Turkey, FourBagger Cases
                AddSpareStrikeBonus(pins); //Add Bonus for Spare or Strike to previous round

            }
            else
            {
                UpdateCurrentFrame(pins); // Add second score for the current Round, if it is already not a strike                
                AddStrikeBonus(pins); //Add bonus for Strike. As in strike we require entire Frame Score
                AddBonusForContiniousStrike(frameNumber - 1, ScoreBoard[frameNumber - 1].FirstScore, 1); //Handle Double Scenario

                
            }

        }

        public int GetScore()
        {
            // Returns the final score of the game.
            //Traverce Through the Frame to Get the final Score
            int finalScore = 0;
            for(int i=0;i< ScoreBoard.Length; i++)
            {
                finalScore += ScoreBoard[i].Score;
            }
            return finalScore;
        }

        private bool IsNewFrame()
        {
            //If not a strike and Round not yet complete
            //frame number points to the new frame needs to be addedd, current framenumber is framenumner-1
            if (frameNumber == 0)
            {
                return true;
            }
            if(frameNumber == 10)
            {
                return false;
            }
            if(ScoreBoard[frameNumber-1].SecondScore == -1 && ScoreBoard[frameNumber-1].FirstScore < 10)
            {
                return false;
            }
            return true;
        }
        private void AddNewFrame(int newScore)
        {
            ScoreBoard[frameNumber] = new Frame(GetNameForFrame(frameNumber),newScore);
            ++frameNumber;
        }

        private void UpdateCurrentFrame(int newScore)
        {
            ScoreBoard[frameNumber - 1].AddScore(newScore);
        }
        private string GetNameForFrame(int frameNumber)
        {            
            return "Frame_" + frameNumber;
        }

        private void AddSpareStrikeBonus(int score)
        {
            if (frameNumber==1)
            {
                return;
            }
            //frame number points to the new frame needs to be addedd, current framenumber is framenumner-1
            else if (ScoreBoard[frameNumber - 2].frameType == FrameType.Spare || ScoreBoard[frameNumber - 2].frameType == FrameType.Strike)
            {
                ScoreBoard[frameNumber - 2].AddBonus(score);
            }
        }

        private void AddStrikeBonus(int score)
        {
            if (frameNumber == 1)
            {
                return;
            }
            //frame number points to the new frame needs to be addedd, current framenumber is framenumner-1
            //Adding ThirdScore = -1 check to handle the Last Frame Conflict properly
            if (ScoreBoard[frameNumber - 2].frameType == FrameType.Strike && ScoreBoard[frameNumber - 1].ThirdScore == -1)
            {
                ScoreBoard[frameNumber - 2].AddBonus(score);
            }
        }

        private void AddBonusForContiniousStrike(int currentFrameNumber,int score, int depth)
        {
            //If this is First iteration and only one element is inserted
            //Max bonus can be 20 No Need to check for previous Frames in case of all Strikes
            if(depth > 4 || (depth == 1 && currentFrameNumber==1))
            {
                return;
            }
            //If the Frametype is Strike but we are already at First Frame and If we have added more then one elements to array then only add the bonus
            if(currentFrameNumber - depth == 0 && ScoreBoard[currentFrameNumber - depth].frameType == FrameType.Strike && ScoreBoard[0].Bonus<=10)
            {
                ScoreBoard[currentFrameNumber - depth].AddBonus(10);
                return;
            }

            if(currentFrameNumber- depth < 0 || ScoreBoard[currentFrameNumber- depth].frameType != FrameType.Strike)
            {
                return;
            }
            // &&(currentFrameNumber==9 || ScoreBoard[currentFrameNumber + 1] != null)
            if (ScoreBoard[currentFrameNumber- depth].frameType == FrameType.Strike)
            {
                AddBonusForContiniousStrike(currentFrameNumber, ScoreBoard[currentFrameNumber - depth+1].FirstScore, depth+1);
                if(depth >= 2 && ScoreBoard[currentFrameNumber - depth].Bonus<=10) // Its a double
                {
                   ScoreBoard[currentFrameNumber - depth].AddBonus(score);//Maximun Bonus can be 20 only resulting total score of 30
                }
                //else if(depth>3) //If  Turkey or Four-Bagger
                //{
                //    ScoreBoard[currentFrameNumber - depth].AddBonus(10);
                //}
                
            }
        }
    }
}
