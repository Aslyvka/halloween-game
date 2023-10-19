using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
//Name: Anastasiia Slyvka
//Date: October 15, 2023
//Project: Lab 1B - Game

namespace GameInterface
{
	//This class is responsible for the scores, score labels and updates
	public class ManageScore
	{
		//fields
		private int score;
		private TextBlock lblScore;

		//property
		public int Score
		{ 
			get 
			{ 
				return score; 
			} 
		}

		//constructor
		public ManageScore(TextBlock lblScore)
		{
			this.score = 0;
			this.lblScore = lblScore;
			UpdateScoreLabel();
		}
		//methods
		public void IncrementScore()
		{
			score++;
			UpdateScoreLabel();
		}
		//*********** BONUS FEATURE ******** this method is taking a point parameter to give bonus points
		public void IncrementScore(int points = 2)
		{
			score += points;
			UpdateScoreLabel();
		}

		//method to reset the scores
		public void ResetScore()
		{
			score = 0;
			UpdateScoreLabel();
		}
		
		//method to update the score label
		public void UpdateScoreLabel()
		{
			lblScore.Text = $"Score: {score}";
		}

		//method to get the score
		public int GetScore()
		{
			return score;
		}

	}


}
