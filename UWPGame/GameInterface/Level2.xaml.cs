using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using GameLibrary;
using System.Linq;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using Windows.UI.Popups;
using System.IO;
using System.Threading.Tasks;
//Name: Anastasiia Slyvka
//Date: October 15, 2023
//Project: Lab 1B - Game

//Modified and implemented CollectibleItem Class
//Added logic to store the scores, usernames, date and time in a txt file, maintaining the scores
//User will receive the feedback after the game if they won the top winning score and see all players results
//Added a new challenge if enemies double hit the ground at once - every point will get them x10 points
namespace GameInterface
{
	public sealed partial class Level2 : Page
	{
		private const string Game2ScoresFileName = "game2_scores.txt";
		private Player player;
		private List<Enemy> items;
		private GameLevel2 gameGrid;
		private DispatcherTimer gameTimer;
		private Random random = new Random();
		private TimeSpan nextItemInterval;
		private DateTime nextItemStartTime;
		private CountdownTimer countdownTimer;
		private bool isPlayerHit = false;
		private ManageScore manageScore;
		private string playerName;
		private List<GameResult> game2_scores; // List to store GameResult objects for game2

		[Obsolete]
		public Level2()
		{
			this.InitializeComponent();

			gameGrid = new GameLevel2(gridMain);
			items = new List<Enemy>();
			player = Player.CreatePlayer(gridMain, "dog2", 130, 715, 820);
			manageScore = new ManageScore(gameGrid.lblScore);
			game2_scores = new List<GameResult>(); // Initialize the game2 scores list with GameResult objects
			LoadGame2Scores(); // Load existing game2 scores from the file
			InitializeGame();

			this.NavigationCacheMode = NavigationCacheMode.Disabled;
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

		private void OnTimerTick(int secondsRemaining)
		{
			gameGrid.lblTimer.Text = $"Timer: {secondsRemaining}";
		}

		private async void OnTimeElapsed()
		{
			if (!isPlayerHit)
			{
				gameTimer.Stop();
				int actualScore = manageScore.Score;
				GameResult gameResult = new GameResult(playerName, DateTime.Now, actualScore);
				game2_scores.Add(gameResult);
				SaveGame2Scores(playerName);

				if (IsHighestScore(gameResult))
				{
					// Display a congratulations popup
					await ShowCongratulationsPopup(actualScore, true);
				}
				else
				{
					// User didn't become the top score winner, so show the players and their scores
					await ShowCongratulationsPopup(actualScore, false);
				}

				Frame.Navigate(typeof(StartGame3), playerName);
			}
		}

		private void InitializeGame()
		{
			gameTimer = new DispatcherTimer();
			gameTimer.Interval = TimeSpan.FromMilliseconds(33);
			gameTimer.Tick += GameTimer_Tick;
			gameTimer.Start();

			countdownTimer = new CountdownTimer(30);
			countdownTimer.TimerTick += OnTimerTick;
			countdownTimer.TimeElapsed += OnTimeElapsed;
			countdownTimer.Start();

			nextItemInterval = TimeSpan.FromSeconds(random.Next(400, 1200) / 1000.0);
			nextItemStartTime = DateTime.Now.Add(nextItemInterval);
		}

		private void GameTimer_Tick(object sender, object e)
		{
			var currentTime = DateTime.Now;

			if (currentTime >= nextItemStartTime)
			{
				var newItem = Enemy.CreateRandomItem(gridMain, random);
				items.Add(newItem);
				nextItemInterval = TimeSpan.FromSeconds(random.Next(300, 1200) / 2500.0);
				nextItemStartTime = currentTime.Add(nextItemInterval);
			}

			int enemiesMissed = 0; // Track the number of enemies missed in this tick

			foreach (var item in items.ToList())
			{
				item.UpdateLocation(10);

				double containerHeight = player.Location.Top + player.ImageHeight + 15;

				if (item.ImageVisibility == Visibility.Visible && item.Location.Top + item.ImageHeight >= containerHeight)
				{
					item.ImageVisibility = Visibility.Collapsed;
					items.Remove(item);
					manageScore.IncrementScore();

					enemiesMissed++; // Increment the missed enemy count
				}

				if (item.ImageVisibility == Visibility.Visible && player.CollisionDetected(item))
				{
					isPlayerHit = true;
					gameTimer.Stop();
					int actualScore = manageScore.Score;
					GameResult gameResult = new GameResult(playerName, DateTime.Now, actualScore);
					game2_scores.Add(gameResult);
					SaveGame2Scores(playerName);

					// Include the player name when navigating to GameOver2
					Frame.Navigate(typeof(GameOver2), playerName);
				}
			}

			// Check if multiple enemies were missed in this tick
			if (enemiesMissed > 1)
			{
				int additionalPoints = enemiesMissed * 10; 
				manageScore.IncrementScore(additionalPoints);
			}

			manageScore.UpdateScoreLabel();
		}


		//Logic to move the player
		private void CoreWindow_KeyDown(object sender, Windows.UI.Core.KeyEventArgs e)
		{
			player.HandleKeyDown(e.VirtualKey, gridMain.ActualWidth);

			var collisionItem = items.FirstOrDefault(item => player.CollisionDetected(item));
			if (collisionItem != null)
			{
				collisionItem.ImageVisibility = Visibility.Collapsed;
				items.Remove(collisionItem);
			}
		}

		private void LoadGame2Scores()
		{
			try
			{
				StorageFolder localFolder = ApplicationData.Current.LocalFolder;
				StorageFile scoreFile = localFolder.GetFileAsync(Game2ScoresFileName).GetAwaiter().GetResult();
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
						game2_scores.Add(gameResult);
					}
				}
			}
			catch (FileNotFoundException)
			{
				var dialog = new MessageDialog("File not found: game2_scores.txt", "Error");
				dialog.ShowAsync().AsTask().Wait();
			}
		}

		//Saving the scores, using split somma delimeter to separate username, date and score
		private void SaveGame2Scores(string playerName)
		{
			try
			{
				StorageFolder localFolder = ApplicationData.Current.LocalFolder;
				StorageFile scoreFile = localFolder.CreateFileAsync(Game2ScoresFileName, CreationCollisionOption.OpenIfExists).GetAwaiter().GetResult();
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

				//tracking the current score and the duplicates
				int actualScore = manageScore.Score;
				GameResult currentGameResult = new GameResult(playerName, DateTime.Now, actualScore);
				List<GameResult> uniqueScores = new List<GameResult>();

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

				File.WriteAllText(scoreFile.Path, string.Join(Environment.NewLine, scoreLines));
			}
			catch (Exception)
			{

			}
		}
		//Tracking the plays and getting the highest
		private bool IsHighestScore(GameResult userResult)
		{
			int userScore = userResult.Score;
			int highestScore = game2_scores.Max(score => score.Score);

			return userScore >= highestScore;
		}
		//Popup displaying the score and players results all sorted by the scores
		private async Task ShowCongratulationsPopup(int userScore, bool isWinner)
		{
			var dialog = new MessageDialog(isWinner
				? $"Congratulations! You earned the highest score! Score: {userScore}"
				: $"You didn't become the top score winner. Your Score: {userScore}");

			dialog.Title = isWinner ? "Congratulations" : "You completed the level";

			var sortedScores = game2_scores.OrderByDescending(result => result.Score).ToList();

			// List of players with play history
			var playersAndScores = sortedScores.Select((result, index) => $"{index + 1}. {result.Username} (Score: {result.Score}, Date: {result.DateTime})").ToList();
			var playersAndScoresText = string.Join("\n", playersAndScores);

			dialog.Content = $"{dialog.Content}\n\nAll Players\n{playersAndScoresText}";

			dialog.Commands.Add(new UICommand("OK"));
			await dialog.ShowAsync();
		}
	}
}

