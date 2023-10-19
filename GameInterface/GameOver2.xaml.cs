using GameLibrary;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
//Name: Anastasiia Slyvka
//Date: October 15, 2023
//Project: Lab 1B - Game
namespace GameInterface
{
	//This page will be loaded if user fails Level #2
	//Creating and loading a main grid into our level, adding audio, buttons to start again or exit
	//Added logic to get and pass back the Player Name
	public sealed partial class GameOver2 : Page
	{
		private readonly Audio audio;
		private string playerName;
		private const string PlayerNameKey = "PlayerName";

		[System.Obsolete]
		public GameOver2()
		{
			this.InitializeComponent();

			//Creating new instance of Audio and Grid
			audio = new Audio();
			Grid grid = new Grid();



			//Setting Background image
			grid.Background = new ImageBrush
			{
				ImageSource = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new System.Uri("ms-appx:///Assets/GameOver2.jpg")),
				Stretch = Stretch.UniformToFill
			};

			//TextBlock
			TextBlock lblGameOver = new TextBlock
			{
				Text = "GAME OVER!",
				FontSize = 100,
				VerticalAlignment = VerticalAlignment.Top,
				HorizontalAlignment = HorizontalAlignment.Left,
				FontFamily = new FontFamily("Snap ITC"),
				Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(0xFF, 0xAE, 0xB6, 0xFD)),
				Margin = new Thickness(774, 79, 0, 0),
				Height = 170,
				Width = 800
			};

			// Create and configure the "Try Again" button
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
				Background = new SolidColorBrush(Windows.UI.Color.FromArgb(0xFF, 0x2D, 0x5B, 0xCA)),
				BorderBrush = new SolidColorBrush(Windows.UI.Colors.Black)
			};
			btnPlayAgain.Click += btnPlayAgain_Click;

			// Create and configure the "Exit" button
			Button btnExitGame = new Button
			{
				Content = "Exit",
				Margin = new Thickness(998, 455, 0, 0),
				VerticalAlignment = VerticalAlignment.Top,
				Height = 133,
				Width = 351,
				FontSize = 48,
				FontWeight = Windows.UI.Text.FontWeights.Bold,
				FontFamily = new FontFamily("Snap ITC"),
				BorderBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(0x7F, 0xFF, 0xFF, 0xFF))
			};
			btnExitGame.Click += btnExitGame_Click;

			// Add UI elements to the main grid
			grid.Children.Add(lblGameOver);
			grid.Children.Add(btnPlayAgain);
			grid.Children.Add(btnExitGame);

			// Set the main grid as the page's content
			this.Content = grid;

			// Add the media element to a container so it's part of the visual tree
			MediaElement musicElement = audio.BackgroundMusicGameOver2;
			musicElement.Visibility = Visibility.Visible; // Ensure it's visible
			grid.Children.Add(musicElement);

			audio.BackgroundMusicGameOver2.Play();

			LoadPlayerName();
		}
		// Passing the player's name and navigating to Level2
		private void btnPlayAgain_Click(object sender, RoutedEventArgs e)
		{
			audio.BackgroundMusicGameOver2.Stop();
			Frame.Navigate(typeof(Level2), playerName, new Windows.UI.Xaml.Media.Animation.SuppressNavigationTransitionInfo());
		}

		private void btnExitGame_Click(object sender, RoutedEventArgs e)
		{
			audio.BackgroundMusicGameOver2.Stop();
			Application.Current.Exit();
		}

		//Loading the player name
		private void LoadPlayerName()
		{
			if (string.IsNullOrEmpty(playerName))
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
		}
		//Keeping the player name with OnNAvigateTo logic
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
