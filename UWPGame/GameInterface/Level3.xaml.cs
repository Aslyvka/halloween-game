using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using GameLibrary;
using System.Linq;
using Windows.UI.Xaml.Navigation;
using System.IO;
using Windows.Storage;
using Windows.UI.Popups;
using System.Threading.Tasks;
//Name: Anastasiia Slyvka
//Date: October 18, 2023
//Project: Lab 1B - Game

//Modified and implemented CollectibleItem Class
//Added logic to store the scores, usernames, date and time in a txt file, maintaining the scores
//User will receive the feedback after the game if they won the top winning score and see all players results
//Added a new challenge if user collects more items at once - every point will get them x10 points
namespace GameInterface
{
	public sealed partial class Level3 : Page
	{
		private static Player player;
		private List<GamePiece> items;
		private GameLevel3 gameGrid;
		private DispatcherTimer gameTimer;
		private Random random = new Random();
		private TimeSpan nextItemInterval;
		private DateTime nextItemStartTime;
		private CountdownTimer countdownTimer;
		private int missedPumpkins = 0;
		private ManageScore manageScore;
		private string playerName;
		private List<GameResult> game3_scores;
		private const string Game3ScoresFileName = "game3_scores.txt";

		[Obsolete]
		public Level3()
		{
			//Initializing the game load logic, player, items and score results, loading scores
			this.InitializeComponent();

			gameGrid = new GameLevel3(gridMain);
			items = new List<GamePiece>();
			player = Player.CreatePlayer(gridMain, "dog", 220, 715, 750);
			manageScore = new ManageScore(gameGrid.lblScore);
			game3_scores = new List<GameResult>();
			LoadGame3Scores();
			InitializeGame();
			this.NavigationCacheMode = NavigationCacheMode.Disabled;
		}
		//Added the logic to pass username when navigating to the page
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			if (e.Parameter != null && e.Parameter is GameResult gameResult)
			{
				playerName = gameResult.Username;
			}
			else if (e.Parameter != null && e.Parameter is string username)
			{
				playerName = username;
			}
			Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
		}
		//Added the logic to pass username when navigating from the page
		protected override void OnNavigatedFrom(NavigationEventArgs e)
		{
			base.OnNavigatedFrom(e);
			if (e.Parameter != null && e.Parameter is string username)
			{
				playerName = username;
			}

			Window.Current.CoreWindow.KeyDown -= CoreWindow_KeyDown;
		}
		//Updating the textblock showing the timer from 30 to 0
		private void OnTimerTick(int secondsRemaining)
		{
			gameGrid.lblTimer.Text = $"Timer: {secondsRemaining}";
		}

		//This method handles 30 seconds game logic if time elapsed - user will get either congratulations message and their highest score or just that they passed the level and all plays history
		//Stopping the timers
		private async void OnTimeElapsed()
		{
			if (gameTimer != null)
			{
				gameTimer.Stop();
			}
			countdownTimer.Stop();

			//Managing the game result and saving the scores
			int actualScore = manageScore.Score;
			GameResult gameResult = new GameResult(playerName, DateTime.Now, actualScore);
			game3_scores.Add(gameResult);
			SaveGame3Scores(playerName);


			//winning popups
			if (IsHighestScore(gameResult))
			{
				await ShowCongratulationsPopup(actualScore, true);
			}
			else
			{
				await ShowCongratulationsPopup(actualScore, false);
			}
			ResetGameState();
			ResetNextItemInterval();
			Frame.Navigate(typeof(Finish));
		}

		
		//Main method that corresponds for pumpkins and threats appearance and collision detection, calculating the score and setting conditions to game over
		private void GameTimer_Tick(object sender, object e)
		{
			var currentTime = DateTime.Now;

			//Spawning items within the certain random intervals
			if (currentTime >= nextItemStartTime)
			{
				var newItem = CreateRandomItem();
				items.Add(newItem);
				nextItemInterval = TimeSpan.FromSeconds(random.Next(800, 1200) / 1000.0);
				nextItemStartTime = currentTime.Add(nextItemInterval);
				ResetNextItemInterval();
			}


			//Logic to track the collision
			int itemsCollided = 0;
			double containerHeight = player.Location.Top + player.ImageHeight + 15;

			//Looping throught items and checking the collision if images are visible and based by image name
			foreach (var item in items.ToList())
			{
				item.UpdateLocation(10);

				bool isCollided = item.ImageVisibility == Visibility.Visible && player.CollisionDetected(item);

				if (isCollided)
				{
					if (item.ImageName.Contains("threat"))
					{
						itemsCollided++;
						item.ImageVisibility = Visibility.Collapsed;
						items.Remove(item);
						HandleGameOver();
						return; 
					}
					//Added this line instead of else because this way game ran more smoothly, could not figure out what is the issue, but sometimes there is a glitch appearing only die to the code above if threat collision detected, but sometimes it runds smoothly
					if (!item.ImageName.Contains("threat")) 
					{
						if (item.ImageName.Contains("pumpkin"))
						{
							itemsCollided++;
							item.ImageVisibility = Visibility.Collapsed;
							items.Remove(item);

							//BONUS SCORE FEATURE (if user will hit 2 pumkins at once they would get 10 points, if 3 and more - 20 points
							if (itemsCollided == 1)
							{
								manageScore.IncrementScore(1);
							}
							else if (itemsCollided == 2)
							{
								manageScore.IncrementScore(10);
							}
							else if (itemsCollided >= 3)
							{
								manageScore.IncrementScore(20);
							}
						}
					}
				}
				//Handling Game Over logic checking for missing pumkins, if miss more than 3 than redirecting the page to Game Over
				if (item.ImageVisibility == Visibility.Visible && item.Location.Top + item.ImageHeight >= containerHeight)
				{
					item.ImageVisibility = Visibility.Collapsed;
					items.Remove(item);

					if (item.ImageName.Contains("pumpkin"))
					{
						missedPumpkins++;
						gameGrid.lblMissedItems.Text = $"Missed: {missedPumpkins}";

						if (missedPumpkins > 3)
						{
							HandleGameOver();
							return;
						}
					}
				}
					// Update the score label
					manageScore.UpdateScoreLabel();
			}
		}

		//Logic to reset next item interval (for better randomization of how they appear)
		private void ResetNextItemInterval()
		{
			nextItemInterval = TimeSpan.FromSeconds(random.Next(400, 1300) / 1000.0);
			nextItemStartTime = DateTime.Now.Add(nextItemInterval);
		}


		//Pumkins will be created at 70% chance
		//Threats will be created at 30% chance
		private GamePiece CreateRandomItem()
		{
			double randomValue = random.NextDouble();

			if (randomValue < 0.7)
			{
				return CollectibleItem.CreatePumpkin(gridMain, random);
			}
			else
			{
				return Threat.CreateRandomThreat(gridMain, random);
			}
		}

		//Logic to move player
		private void CoreWindow_KeyDown(object sender, Windows.UI.Core.KeyEventArgs e)
		{
			int marginLeft = 100;
			int marginRight = (int)gridMain.ActualWidth - 100 - (int)player.ImageWidth;

			if (e.VirtualKey == Windows.System.VirtualKey.Left)
			{
				if (player.Location.Left > marginLeft)
				{
					player.Move(e.VirtualKey);
				}
			}
			else if (e.VirtualKey == Windows.System.VirtualKey.Right)
			{
				if (player.Location.Left < marginRight)
				{
					player.Move(e.VirtualKey);
				}
			}

			//Collision detection
			var collisionItem = items.FirstOrDefault(item => player.CollisionDetected(item));
			if (collisionItem != null)
			{
				collisionItem.ImageVisibility = Visibility.Collapsed;
				items.Remove(collisionItem);
			}
		}

		//Method to check if the user earned the highest score
		private bool IsHighestScore(GameResult userResult)
		{
			int userScore = userResult.Score;
			int highestScore = game3_scores.Max(score => score.Score);

			return userScore >= highestScore;
		}

		//Popup displaying the score and players results
		//Sorting by the score
		//THe popup will display either congratulations message if player earned top score or if they just passed the level
		//The popup will show the list of players and their play history
		private async Task ShowCongratulationsPopup(int userScore, bool isWinner)
		{
			var dialog = new MessageDialog(isWinner
				? $"Congratulations! You earned the highest score! Score: {userScore}"
				: $"You didn't become the top score winner. Your Score: {userScore}");

			dialog.Title = isWinner ? "Congratulations" : "You completed the level";

			var sortedScores = game3_scores.OrderByDescending(result => result.Score).ToList();

			var playersAndScores = sortedScores.Select((result, index) => $"{index + 1}. {result.Username} (Score: {result.Score}, Date: {result.DateTime})").ToList();
			var playersAndScoresText = string.Join("\n", playersAndScores);
			dialog.Content = $"{dialog.Content}\n\nAll Players\n{playersAndScoresText}";
			dialog.Commands.Add(new UICommand("OK"));

			await dialog.ShowAsync();
		}

		//Logic to Load the the score, spliting our username, score, date by comma delimiter, parsing and extracting the needed values
		private void LoadGame3Scores()
		{
			try
			{
				StorageFolder localFolder = ApplicationData.Current.LocalFolder;
				StorageFile scoreFile = localFolder.GetFileAsync(Game3ScoresFileName).GetAwaiter().GetResult();
				string scoresText = FileIO.ReadTextAsync(scoreFile).GetAwaiter().GetResult();
				string[] scoreStrings = scoresText.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

				foreach (string scoreString in scoreStrings)
				{
					string[] parts = scoreString.Split(',');
					if (parts.Length == 3 &&
						int.TryParse(parts[2], out int score) &&
						DateTime.TryParse(parts[1], out DateTime date))
					{
						string username = parts[0];
						GameResult gameResult = new GameResult(username, date, score);
						game3_scores.Add(gameResult);
					}
				}
			}
			catch (FileNotFoundException)
			{
				var dialog = new MessageDialog("File not found: game3_scores.txt", "Error");
				dialog.ShowAsync().AsTask().Wait();
			}
		}

		//Saving score
		//Creating/Overriding a file with play history
		//Each play records the username, date and score
		//Values will be split by comma delimeter
		private void SaveGame3Scores(string playerName)
		{
			try
			{
				StorageFolder localFolder = ApplicationData.Current.LocalFolder;
				StorageFile scoreFile = localFolder.CreateFileAsync(Game3ScoresFileName, CreationCollisionOption.OpenIfExists).GetAwaiter().GetResult();
				string scoresText = FileIO.ReadTextAsync(scoreFile).GetAwaiter().GetResult();

				List<GameResult> existingScores = new List<GameResult>();
				string[] scoreStrings = scoresText.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

				foreach (string scoreString in scoreStrings)
				{
					string[] parts = scoreString.Split(',');
					if (parts.Length == 3 &&
						int.TryParse(parts[2], out int score) &&
						DateTime.TryParse(parts[1], out DateTime date))
					{
						string username = parts[0];
						GameResult gameResult = new GameResult(username, date, score);
						existingScores.Add(gameResult);
					}
				}

				int actualScore = manageScore.Score;
				GameResult currentGameResult = new GameResult(playerName, DateTime.Now, actualScore);
				List<GameResult> uniqueScores = new List<GameResult>();

				//Handling duplicates, so that we don't override with duplicates by accident
				foreach (var existingResult in existingScores)
				{
					bool isDuplicate = existingResult.Username == currentGameResult.Username && existingResult.DateTime == currentGameResult.DateTime && existingResult.Score == currentGameResult.Score;

					if (!isDuplicate)
					{
						uniqueScores.Add(existingResult);
					}
				}

				uniqueScores.Add(currentGameResult);
				uniqueScores = uniqueScores.OrderByDescending(score => score.Score).ToList();

				List<string> scoreLines = uniqueScores.Select(gameResult =>
					$"{gameResult.Username},{gameResult.DateTime},{gameResult.Score}").ToList();

				//Joing our values to each new line
				File.WriteAllText(scoreFile.Path, string.Join(Environment.NewLine, scoreLines));
			}
			catch (Exception)
			{

			}
		}

		//Game logic to initialize the game start, handling timers
		private void InitializeGame()
		{
			if (gameTimer != null)
			{
				gameTimer.Stop();
			}

			if (countdownTimer != null)
			{
				countdownTimer.Stop();
			}

			gameTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(33) };
			gameTimer.Tick += GameTimer_Tick;

			countdownTimer = new CountdownTimer(30);
			countdownTimer.TimerTick += OnTimerTick;
			countdownTimer.TimeElapsed += OnTimeElapsed;

			//ResetGameState(); 
			//ResetNextItemInterval();

			gameTimer.Start();
			countdownTimer.Start();
		}




		//Method handles game over logic if user missed 4 pumpkins or collides with a threat
		//Stopping all timers, storing the results in the textfile, resetting the game
		//navigating to game over page, passing the same username
		private void HandleGameOver()
		{

				int actualScore = manageScore.Score;
				GameResult gameResult = new GameResult(playerName, DateTime.Now, actualScore);
				game3_scores.Add(gameResult);
				SaveGame3Scores(playerName);
				ResetGameState();
				Frame.Navigate(typeof(GameOver3), playerName);

		}
		//Logic to reset the game, timers, values to defaults
		private void ResetGameState()
		{
			foreach (var item in items)
			{
				item.ImageVisibility = Visibility.Collapsed;
			}
			items.Clear();
			manageScore.ResetScore();
			missedPumpkins = 0;
			countdownTimer.Reset();
			gameTimer.Stop();
			nextItemInterval = TimeSpan.FromSeconds(random.Next(400, 1300) / 1000.0);
			nextItemStartTime = DateTime.Now.Add(nextItemInterval);
		}
	}
}

