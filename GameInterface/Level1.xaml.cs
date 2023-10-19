using GameLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
//Name: Anastasiia Slyvka
//Date: October 15, 2023
//Project: Lab 1B - Game

//Modified and implemented CollectibleItem Class
//Added logic to store the scores, usernames, date and time in a txt file, maintaining the scores
//User will receive the feedback after the game if they won the top winning score and see all players results
//Added a new challenge if user collects more items at once - every point will get them x10 points
namespace GameInterface
{
	public sealed partial class Level1 : Page
	{
		private Player player;
		private List<CollectibleItem> items;
		private GameLevel gameGrid;
		private int missedItems = 0;
		private DispatcherTimer gameTimer;
		private Random random = new Random();
		private TimeSpan nextItemInterval;
		private DateTime nextItemStartTime;
		private CountdownTimer countdownTimer;
		private ManageScore manageScore;
		private string playerName;
		private const string Game1ScoresFileName = "game1_scores.txt";
		private List<GameResult> game1_scores;

		[Obsolete]
		public Level1()
		{
			this.InitializeComponent();
			gameGrid = new GameLevel(gridMain);
			items = new List<CollectibleItem>();
			player = Player.CreatePlayer(gridMain, "pot", 300, 715, 650);
			manageScore = new ManageScore(gameGrid.lblScore);
			game1_scores = new List<GameResult>(); // Initialize the game1 scores list with GameResult objects
			LoadGame1Scores(); // Load existing game1 scores from the file
			InitializeGame();
			this.NavigationCacheMode = NavigationCacheMode.Disabled;

			string localFolderPath = ApplicationData.Current.LocalFolder.Path;
			System.Diagnostics.Debug.WriteLine("Local Folder Path: " + localFolderPath);
		}


		//Added the logic to pass username
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
		//Added the logic to pass username
		protected override void OnNavigatedFrom(NavigationEventArgs e)
		{
			base.OnNavigatedFrom(e);
			if (e.Parameter != null && e.Parameter is string username)
			{
				playerName = username;
			}
			Window.Current.CoreWindow.KeyDown -= CoreWindow_KeyDown;
		}

		//Method handles game over logic if user missed 4 itmes
		//Stopping all timers, storing the results in the textfile, resetting the game
		//navigating to game over page, passing the same username
		private void HandleGameOver()
		{
			if (missedItems > 3)
			{
				gameTimer?.Stop();
				countdownTimer.Stop();
				int actualScore = manageScore.Score;
				GameResult gameResult = new GameResult(playerName, DateTime.Now, actualScore);
				game1_scores.Add(gameResult);
				SaveGame1Scores();
				ResetGameState();
				Frame.Navigate(typeof(GameOver), playerName);
			}
		}

		// New method to load existing game1 scores from the file
		private void LoadGame1Scores()
		{
			try
			{
				StorageFolder localFolder = ApplicationData.Current.LocalFolder;
				StorageFile scoreFile = localFolder.GetFileAsync("game1_scores.txt").GetAwaiter().GetResult();
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
						game1_scores.Add(gameResult); // Add the parsed GameResult object
					}
				}
			}
			catch (FileNotFoundException)
			{
				var dialog = new MessageDialog("File not found: game1_scores.txt", "Error");
				dialog.ShowAsync().AsTask().Wait();
			}
		}

		// New method to save game1 scores to the file
		private void SaveGame1Scores()
		{
			try
			{
				// Getting the local folder, creatingthe score file
				StorageFolder localFolder = ApplicationData.Current.LocalFolder;
				StorageFile scoreFile = localFolder.CreateFileAsync(Game1ScoresFileName, CreationCollisionOption.OpenIfExists).GetAwaiter().GetResult();

				// reading the file
				string scoresText = FileIO.ReadTextAsync(scoreFile).GetAwaiter().GetResult();

				// Adding to the list
				List<GameResult> existingScores = new List<GameResult>();
				string[] scoreStrings = scoresText.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

				//splitting the results by comma delimeter
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

				// Defining the actual score user achieved and storring into current game result
				int actualScore = manageScore.Score;
				GameResult currentGameResult = new GameResult(playerName, DateTime.Now, actualScore);

				// Tracking all results to avoid duplicates
				List<GameResult> uniqueScores = new List<GameResult>();
				foreach (var existingResult in existingScores)
				{
					bool isDuplicate = existingResult.Username == currentGameResult.Username &&
									   existingResult.DateTime == currentGameResult.DateTime &&
									   existingResult.Score == currentGameResult.Score;

					if (!isDuplicate)
					{
						uniqueScores.Add(existingResult);
					}
				}
				uniqueScores.Add(currentGameResult);

				//Sorting by Score
				uniqueScores = uniqueScores.OrderByDescending(score => score.Score).ToList();

				// Output string
				List<string> scoreLines = uniqueScores.Select(gameResult =>
					$"{gameResult.Username},{gameResult.DateTime},{gameResult.Score}").ToList();

				//Updating the file
				File.WriteAllText(scoreFile.Path, string.Join(Environment.NewLine, scoreLines));
			}
			catch (Exception)
			{

			}
		}
		//Updating the timer
		private void OnTimerTick(int secondsRemaining)
		{
			gameGrid.lblTimer.Text = $"Timer: {secondsRemaining}";
		}

		//When timer is 0, stopping all timers, overriding the file with game results, saving the score, resetting the game ad navigating to the next level and passing the username
		private async void OnTimeElapsed()
		{
			gameTimer?.Stop();
			countdownTimer.Stop();
			int actualScore = manageScore.Score;
			GameResult gameResult = new GameResult(playerName, DateTime.Now, actualScore);
			game1_scores.Add(gameResult);
			SaveGame1Scores();

			// Checking if the user has the highest score and displaying appropriate message
			if (IsHighestScore(gameResult))
			{
				await ShowCongratulationsPopup(actualScore, true);
			}
			else
			{
				await ShowCongratulationsPopup(actualScore, false);
			}

			ResetGameState();
			Frame.Navigate(typeof(StartGame2), playerName);
		}

		// Check if the user's score is the highest among all players
		private bool IsHighestScore(GameResult userResult)
		{
			int userScore = userResult.Score;
			int highestScore = game1_scores.Max(score => score.Score);

			return userScore >= highestScore;
		}

		private async Task ShowCongratulationsPopup(int userScore, bool isWinner)
		{
			var dialog = new MessageDialog(isWinner
				? $"Congratulations! You earned the highest score! Score: {userScore}"
				: $"You didn't become the top score winner. Your Score: {userScore}");

			dialog.Title = isWinner ? "Congratulations" : "You completed the level";

			// Sort players and scores by the highest score, date, and type
			var sortedScores = game1_scores.OrderByDescending(result => result.Score).ToList();

			// Create a list of players and their scores with formatted text
			var playersAndScores = sortedScores.Select((result, index) => $"{index + 1}. {result.Username} (Score: {result.Score}, Date: {result.DateTime})").ToList();
			var playersAndScoresText = string.Join("\n", playersAndScores);

			// Add players and scores underneath the message
			dialog.Content = $"{dialog.Content}\n\nAll Players\n{playersAndScoresText}";

			// Add a button to close the dialog
			dialog.Commands.Add(new UICommand("OK"));

			// Show the dialog and wait for user input
			await dialog.ShowAsync();
		}
		//Method to start the game from the default
		private void InitializeGame()
		{
			gameTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(33) };
			gameTimer.Tick += GameTimer_Tick;

			// Set up the countdown timer
			countdownTimer = new CountdownTimer(30);
			countdownTimer.TimerTick += OnTimerTick;
			countdownTimer.TimeElapsed += OnTimeElapsed;

			// Set up the next item spawn timing
			nextItemInterval = TimeSpan.FromSeconds(random.Next(500, 1200) / 1000.0);
			nextItemStartTime = DateTime.Now.Add(nextItemInterval);

			// Reset the game state and start timers
			ResetGameState();
			gameTimer.Start();
			countdownTimer.Start();
		}
		//Method to Reset the Game State
		private void ResetGameState()
		{
			foreach (var item in items)
			{
				item.ImageVisibility = Visibility.Collapsed;
			}
			items.Clear();
			manageScore.ResetScore();
			missedItems = 0;
			nextItemInterval = TimeSpan.FromSeconds(random.Next(500, 1200) / 1000.0);
			nextItemStartTime = DateTime.Now.Add(nextItemInterval);
			countdownTimer.Reset();
		}

		//Event Tick
		private void GameTimer_Tick(object sender, object e)
		{
			var currentTime = DateTime.Now;

			if (currentTime >= nextItemStartTime)
			{
				var newItem = CreateRandomItem();
				items.Add(newItem);
				nextItemInterval = TimeSpan.FromSeconds(random.Next(600, 1200) / 1000.0);
				nextItemStartTime = currentTime.Add(nextItemInterval);
			}

			int itemsCollided = 0;

			//Detecting if user collided with an item
			foreach (var item in items.ToList())
			{
				item.UpdateLocation(10);

				if (item.ImageVisibility == Visibility.Visible && player.CollisionDetected(item))
				{
					item.ImageVisibility = Visibility.Collapsed;
					items.Remove(item);
					itemsCollided++; 
				}

				double containerHeight = player.Location.Top + player.ImageHeight + 50;

				if (item.ImageVisibility == Visibility.Visible && item.Location.Top + item.ImageHeight >= containerHeight)
				{
					item.ImageVisibility = Visibility.Collapsed;
					items.Remove(item);
					missedItems += 1;
				}
				//If user missed 4 items - calling game over
				if (missedItems > 3)
				{
					HandleGameOver();
					return;
				}
			}
			//Bonus logic if user collided "collected" 2 mushrooms at once, they will get 10 bonus points per point
			//if they collect 3 and more - they will get 20 points per hit
			if (itemsCollided == 2)
			{
				manageScore.IncrementScore(10);
			}
			else if (itemsCollided >= 3)
			{
				manageScore.IncrementScore(20);
			}
			else if (itemsCollided == 1)
			{
				manageScore.IncrementScore();
			}

			manageScore.UpdateScoreLabel();
			gameGrid.lblMissedItems.Text = $"Missed: {missedItems}";
		}

		//Method to create random mushrooms
		private CollectibleItem CreateRandomItem()
		{
			return CollectibleItem.CreateRandomItem(gridMain, random);
		}
		//Calling the method to move the player
		private void CoreWindow_KeyDown(object sender, Windows.UI.Core.KeyEventArgs e)
		{
			player.OnKeyDown((Windows.UI.Core.CoreWindow)sender, e, gridMain.ActualWidth);
		}
	}
}