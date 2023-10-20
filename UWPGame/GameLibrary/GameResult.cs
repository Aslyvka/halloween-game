using System;
//Name: Anastasiia Slyvka
//Date: October 15, 2023
//Project: Lab 1B - Game

//This class is created to store the User's Name, Score and Date and time when the each level is played
public class GameResult
{
	//Properties
	public string Username { get; set; }
	public int Score { get; set; }
	public DateTime DateTime { get; set; }

	//Constructors
	public GameResult(string username, DateTime dateTime, int score)
	{
		Username = username;
		DateTime = dateTime;
		Score = score;
	}

	public GameResult(string username)
	{
		Username = username;
	}
	//ToString() method
	public override string ToString()
	{
		return $"{this.Username} {this.Score} {this.DateTime}";
	}
}
