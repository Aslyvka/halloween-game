using GameLibrary;
using Windows.Storage;
using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
//Lab 1B
//Anastasiia Slyvka
//October 18, 2023
namespace GameInterface
{
	public sealed partial class StartGame1 : Page
	{
		[Obsolete]
		private Audio audio = new Audio();
		private GameResult gameResult;

		[Obsolete]
		public StartGame1()
		{
			this.InitializeComponent();
			PopupMessage();

			// Create the main grid
			Grid grid = new Grid();
			gameResult = new GameResult(string.Empty);

			// Setting a background Image
			grid.Background = new ImageBrush
			{
				ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/StartGame1.png")),
				Stretch = Stretch.UniformToFill
			};

			TextBox txtName = new TextBox
			{
				Name = "txtName",
				HorizontalAlignment = HorizontalAlignment.Left,
				Margin = new Thickness(500, 600, 0, 0),
				TextWrapping = TextWrapping.Wrap,
				FontFamily = new FontFamily("Snap ITC"),
				PlaceholderText = "Enter your name",
				VerticalAlignment = VerticalAlignment.Top,
				Height = 45,
				Width = 450,
				FontSize = 28,
				Background = new SolidColorBrush(Color.FromArgb(0x33, 0xFF, 0xFF, 0xFF))
			};

			// Attempt to retrieve the saved username from local storage
			string savedUsername = LoadSavedUsername();
			if (!string.IsNullOrWhiteSpace(savedUsername))
			{
				txtName.Text = savedUsername;
			}

			grid.Children.Add(txtName);

			// Create and configure the MediaElement for background music
			// Create and configure the "Play" button
			Button btnPlay = new Button
			{
				Content = "Play",
				Margin = new Thickness(735, 665, 0, 0),
				VerticalAlignment = VerticalAlignment.Top,
				Height = 83,
				Width = 217,
				FontSize = 48,
				FontWeight = Windows.UI.Text.FontWeights.Bold,
				FontFamily = new FontFamily("Snap ITC"),
				Background = new SolidColorBrush(Windows.UI.Color.FromArgb(0xFF, 0xFF, 0x99, 0x33)),
				BorderBrush = new SolidColorBrush(Windows.UI.Colors.Black)
			};

			// Play button click
			btnPlay.Click += (sender, e) =>
			{
				string username = txtName.Text.Trim();

				if (string.IsNullOrWhiteSpace(username))
				{
					username = "Unknown User";
				}

				// Save the username to local storage for replay purposes
				SaveUsername(username);


				audio.MainTheme.Stop();
				gameResult.Username = username;
				Frame.Navigate(typeof(Level1), gameResult);
			};

			// Exit button style
			Button btnExit = new Button
			{
				Content = "Exit",
				Margin = new Thickness(500, 665, 0, 0),
				VerticalAlignment = VerticalAlignment.Top,
				Height = 83,
				Width = 217,
				FontSize = 48,
				FontWeight = Windows.UI.Text.FontWeights.Bold,
				FontFamily = new FontFamily("Snap ITC")
			};

			//Exit button click
			btnExit.Click += (sender, e) =>
			{
				audio.MainTheme.Stop();
				Application.Current.Exit();
			};

			//adding music Element and playing the main theme
			MediaElement musicElement = audio.MainTheme;
			musicElement.Visibility = Visibility.Visible;
			grid.Children.Add(musicElement);

			//adding buttons to the grid
			grid.Children.Add(btnPlay);
			grid.Children.Add(btnExit);

			//playin the MainTheme
			audio.MainTheme.Play();

			this.Content = grid;
		}

		// Load saved username from local storage
		private string LoadSavedUsername()
		{
			var localSettings = ApplicationData.Current.LocalSettings;
			if (localSettings.Values.TryGetValue("SavedUsername", out object savedUsername))
			{
				return savedUsername.ToString();
			}
			return string.Empty;
		}

		// Save username to local storage
		private void SaveUsername(string username)
		{
			var localSettings = ApplicationData.Current.LocalSettings;
			localSettings.Values["SavedUsername"] = username;
		}

		//Adding a popup message and style
		private async void PopupMessage()
		{
			Color color = Color.FromArgb(255, 255, 165, 0);
			SolidColorBrush blueBrush = new SolidColorBrush(color);

			Image congratsImage = new Image();
			congratsImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/popup1.png"));
			congratsImage.Width = 500;
			congratsImage.Height = 500;

			TextBlock messageTextBlock = new TextBlock();
			messageTextBlock.Text = "You made a playful deal with a witch inside a Haunted House. She's given you a fun task: collect the ingredients for her potion, and she'll show you a secret exit, so you can escape from the Haunted House! But you need to be quick. You only have 30 seconds, and if you miss more than 3 ingredients, you stay there forever. You may use Left and Right arrow keys to move the pot. Are you up for the challenge? Let's begin!";
			messageTextBlock.TextWrapping = TextWrapping.WrapWholeWords;
			messageTextBlock.Foreground = new SolidColorBrush(Colors.White);

			StackPanel contentPanel = new StackPanel();
			contentPanel.Children.Add(congratsImage);
			contentPanel.Children.Add(messageTextBlock);
			contentPanel.Background = blueBrush;

			// Create ContentDialog
			ContentDialog customDialog = new ContentDialog()
			{
				Title = "Haunted House **LEVEL 1**",
				Content = contentPanel,
				PrimaryButtonText = "OK",
				Background = blueBrush,
				Foreground = new SolidColorBrush(Colors.White),
				CornerRadius = new CornerRadius(15)
			};

			await customDialog.ShowAsync();
		}
	}
}

