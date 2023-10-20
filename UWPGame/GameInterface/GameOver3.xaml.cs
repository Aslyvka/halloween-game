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
	//This page will be loaded if user fails Level #3
	//Creating and loading a main grid into our level, adding audio, buttons to start again or exit
	//Added logic to load and pass back the Player Name
	public sealed partial class GameOver3 : Page
	{
		private readonly Audio audio;
		private string playerName;
		private const string PlayerNameKey = "PlayerName";

		[System.Obsolete]
		public GameOver3()
		{
			this.InitializeComponent();

			//Creating a new instance of grid and audio
			audio = new Audio();
			Grid grid = new Grid();
			grid.Background = new ImageBrush
			{
				ImageSource = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new System.Uri("ms-appx:///Assets/GameOver3.jpg")),
				Stretch = Stretch.UniformToFill
			};

			//TextBlock style
			TextBlock lblGameOver = new TextBlock
			{
				Text = "GAME OVER!",
				FontSize = 100,
				VerticalAlignment = VerticalAlignment.Top,
				HorizontalAlignment = HorizontalAlignment.Left,
				FontFamily = new FontFamily("Snap ITC"),
				Foreground = new SolidColorBrush(Windows.UI.Color.FromArgb(0xFF, 0x14, 0x7B, 0x28)),
				Margin = new Thickness(542, 153, 0, 0),
				Height = 170,
				Width = 800
			};

			//Button style to play again
			Button btnPlayAgain = new Button
			{
				Content = "Try Again",
				Margin = new Thickness(768, 411, 0, 0),
				VerticalAlignment = VerticalAlignment.Top,
				Height = 133,
				Width = 351,
				FontSize = 48,
				FontWeight = Windows.UI.Text.FontWeights.Bold,
				FontFamily = new FontFamily("Snap ITC"),
				Background = new SolidColorBrush(Windows.UI.Color.FromArgb(0xFF, 0x1E, 0x7D, 0x16)),
				BorderBrush = new SolidColorBrush(Windows.UI.Colors.Black)
			};
			//Button click
			btnPlayAgain.Click += btnPlayAgain_Click;

			//Button Exit style
			Button btnExitGame = new Button
			{
				Content = "Exit",
				Margin = new Thickness(769, 573, 0, 0),
				VerticalAlignment = VerticalAlignment.Top,
				Height = 133,
				Width = 351,
				FontSize = 48,
				FontWeight = Windows.UI.Text.FontWeights.Bold,
				FontFamily = new FontFamily("Snap ITC"),
				BorderBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(0x7F, 0xFF, 0xFF, 0xFF))
			};
			//Button click
			btnExitGame.Click += btnExitGame_Click;

			// Adding label and buttons to the grid
			grid.Children.Add(lblGameOver);
			grid.Children.Add(btnPlayAgain);
			grid.Children.Add(btnExitGame);

			// Seting grid to page content
			this.Content = grid;

			//Adding media element to play the music
			MediaElement musicElement = audio.BackgroundMusicGameOver3;
			musicElement.Visibility = Visibility.Visible;
			grid.Children.Add(musicElement);

			audio.BackgroundMusicGameOver3.Play();

			LoadPlayerName();
		}

		//Navigating to Level 3 to Play again and stopping the audio, passing the Player name
		private void btnPlayAgain_Click(object sender, RoutedEventArgs e)
		{
			audio.BackgroundMusicGameOver3.Stop();
			Frame.Navigate(typeof(Level3), playerName, new Windows.UI.Xaml.Media.Animation.SuppressNavigationTransitionInfo());
		}
		//Exiting the game
		private void btnExitGame_Click(object sender, RoutedEventArgs e)
		{
			audio.BackgroundMusicGameOver3.Stop();
			Application.Current.Exit();
		}

		//Loading Player Name
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

		//Keeping and passing player name with OnNavigateTo logic
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
