using GameLibrary;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.Storage;
using Windows.UI.Xaml.Navigation;
//Name: Anastasiia Slyvka
//Date: October 15, 2023
//Project: Lab 1B - Game


//Added Player Name ad local storage setting from where the name could be reused
namespace GameInterface
{
	public sealed partial class GameOver : Page
	{
		private readonly Audio audio;
		private string playerName;
		private const string PlayerNameKey = "PlayerName";

		[System.Obsolete]
		public GameOver()
		{
			this.InitializeComponent();

			// Create a new instance of Audio and Grid
			audio = new Audio();
			Grid grid = new Grid();

			// Set Background image
			grid.Background = new ImageBrush
			{
				ImageSource = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new System.Uri("ms-appx:///Assets/GameOver1.jpg")),
				Stretch = Stretch.UniformToFill
			};

			// TextBlock
			TextBlock lblGameOver = new TextBlock
			{
				Text = "GAME OVER!",
				FontSize = 100,
				VerticalAlignment = VerticalAlignment.Top,
				HorizontalAlignment = HorizontalAlignment.Left,
				FontFamily = new FontFamily("Snap ITC"),
				Foreground = new SolidColorBrush(Windows.UI.Colors.White),
				Margin = new Thickness(774, 79, 0, 0),
				Height = 170,
				Width = 800
			};

			// Button style
			Button btnPlayAgain = new Button
			{
				Content = "Try Again",
				Margin = new Thickness(992, 293, 0, 0),
				VerticalAlignment = VerticalAlignment.Top,
				Height = 133,
				Width = 351,
				FontSize = 48,
				FontWeight = Windows.UI.Text.FontWeights.Bold,
				FontFamily = new FontFamily("Snap ITC"),
				Background = new SolidColorBrush(Windows.UI.Color.FromArgb(0x8C, 0xD8, 0x9B, 0xEF)),
				BorderBrush = new SolidColorBrush(Windows.UI.Colors.Black)
			};
			// Play again button click if user wants to try again
			btnPlayAgain.Click += btnPlayAgain_Click;

			// Exit button style
			Button btnExitGame = new Button
			{
				Content = "Exit",
				Margin = new Thickness(991, 455, 0, 0),
				VerticalAlignment = VerticalAlignment.Top,
				Height = 133,
				Width = 351,
				FontSize = 48,
				FontWeight = Windows.UI.Text.FontWeights.Bold,
				FontFamily = new FontFamily("Snap ITC")
			};
			// Exit button click
			btnExitGame.Click += btnExitGame_Click;

			// Adding label and buttons to the grid
			grid.Children.Add(lblGameOver);
			grid.Children.Add(btnPlayAgain);
			grid.Children.Add(btnExitGame);

			// Setting grid as a page content
			this.Content = grid;

			// Adding music element to play music for this level
			MediaElement musicElement = audio.BackgroundMusicGameOver1;
			musicElement.Visibility = Visibility.Visible;
			grid.Children.Add(musicElement);

			audio.BackgroundMusicGameOver1.Play();

			// Load the player's name from local settings
			LoadPlayerName();
		}

		// Navigating to Level1 if the user wants to try again and stop playing that level's music
		private void btnPlayAgain_Click(object sender, RoutedEventArgs e)
		{
			audio.BackgroundMusicGameOver1.Stop();

			// Passing the player's name and navigating to Level1
			Frame.Navigate(typeof(Level1), playerName, new Windows.UI.Xaml.Media.Animation.SuppressNavigationTransitionInfo());
		}


		// Exiting the game
		private void btnExitGame_Click(object sender, RoutedEventArgs e)
		{
			audio.BackgroundMusicGameOver1.Stop();
			Application.Current.Exit();
		}

		// Loaing the player's name from local settings
		private void LoadPlayerName()
		{
			var localSettings = ApplicationData.Current.LocalSettings;
			if (localSettings.Values.ContainsKey(PlayerNameKey))
			{
				playerName = localSettings.Values[PlayerNameKey].ToString();
			}
			else
			{
				playerName = "Unknown User";
			}
		}

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
		}


	}
}

