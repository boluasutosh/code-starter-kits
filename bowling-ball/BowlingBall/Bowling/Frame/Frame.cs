using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BowlingBall.Bowling.Constants;

namespace BowlingBall.Bowling.Frame
{
    public class Frame
    {
        public String Name { get; set; }
        public int Bonus { get; set; }
        public int Score { get; set; }
        public int FirstScore { get; set; }
        public int SecondScore { get; set; }
        public int ThirdScore { get; set; }
        public FrameType frameType;

        //Initialize Second and Third score with -1 to handle the 0 pin drop case
        public Frame(string name,int firstScore)
        {
            this.Name = name;
            this.FirstScore = firstScore;
            this.Score += this.FirstScore;
            this.SecondScore = -1;
            this.ThirdScore = -1;
            checkAndSetFrameType();
        }

        //This method will be called when we will add any new score
        private void checkAndSetFrameType()
        {
            //If all pins are knocked down on the first attempt, No Second attempt yet
            if (Score == 10 && this.SecondScore == -1)
            {
                frameType = FrameType.Strike;
            }
            //If all pins are knocked down Second attempt
            else if (Score == 10 && this.SecondScore != -1)
            {
                frameType = FrameType.Spare;
            }
            else
            {
                frameType = FrameType.Normal;
            }
        }

        //Handle the total score for the frame
        public void AddScore(int score)
        {
            TrackScore(score);
            this.Score += score;
            checkAndSetFrameType();
        }

        //Add bonus when we get strike or spare
        public void AddBonus(int bonus)
        {
            this.Bonus +=  bonus;
            this.Score +=  bonus;
        }

        //Use below method to keep track of FirstScore, SecondScore or ThirdScore
        //Later if we want to Show the values the same can be used to show the values
        private void TrackScore(int score)
        {
            if (this.SecondScore == -1)
            {
                this.SecondScore = score;
                return;
            }
            else if (this.ThirdScore == -1)
            {
                this.ThirdScore = score;
                return;
            }
        }


    }
}
