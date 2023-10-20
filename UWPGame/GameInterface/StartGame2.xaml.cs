using GameLibrary;
using System;
using Windows.Devices.Radios;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
//Name: Anastasiia Slyvka
//Date: October 5, 2023
//Project: Lab 1 - Game
namespace GameInterface
{
	public sealed partial class StartGame2 : Page
	{
		[System.Obsolete]
		Audio audio = new Audio();
		private string playerName;

		[System.Obsolete]
		public StartGame2()
		{
			this.InitializeComponent();
			//Showin popup message when page loads
			PopupMessage();


			// Create the main grid
			Grid grid = new Grid();
			grid.Background = new ImageBrush
			{
				ImageSource = new BitmapImage(new System.Uri("ms-appx:///Assets/StartGame2.jpg")),
				Stretch = Stretch.UniformToFill
			};

			// Play button style
			Button btnPlayLevel2 = new Button
			{
				Content = "Play",
				Margin = new Thickness(398, 477, 0, 0),
				VerticalAlignment = VerticalAlignment.Top,
				Height = 133,
				Width = 351,
				FontSize = 48,
				FontWeight = Windows.UI.Text.FontWeights.Bold,
				FontFamily = new FontFamily("Snap ITC"),
				Background = new SolidColorBrush(Windows.UI.Color.FromArgb(0xFF, 0x7E, 0x9B, 0xF5)),
				BorderBrush = new SolidColorBrush(Windows.UI.Colors.Black)
			};
			//button click
			btnPlayLevel2.Click += btnPlayLevel2_Click;

			// Exit button style
			Button btnExitGame2 = new Button
			{
				Content = "Exit",
				Margin = new Thickness(404, 639, 0, 0),
				VerticalAlignment = VerticalAlignment.Top,
				Height = 133,
				Width = 351,
				FontSize = 48,
				FontWeight = Windows.UI.Text.FontWeights.Bold,
				FontFamily = new FontFamily("Snap ITC")
			};
			//Button click
			btnExitGame2.Click += btnExitGame2_Click;

			//Adding music element to the grid
			MediaElement musicElement = audio.SpookyMusic;
			musicElement.Visibility = Visibility.Visible;
			grid.Children.Add(musicElement);
			// Adding buttons to the grid
			grid.Children.Add(btnPlayLevel2);
			grid.Children.Add(btnExitGame2);

			// Set the main grid as the page's content
			this.Content = grid;
			//start playing spooky music
			audio.SpookyMusic.Play();
		}

		//Navigating to Level2
		[System.Obsolete]
		private void btnPlayLevel2_Click(object sender, RoutedEventArgs e)
		{
			audio.SpookyMusic.Stop();
			Frame.Navigate(typeof(Level2), playerName);

		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			if (e.Parameter != null && e.Parameter is string username)
			{
				playerName = username;
			}
		}


		//Exiting the game
		[System.Obsolete]
		private void btnExitGame2_Click(object sender, RoutedEventArgs e)
		{
			audio.SpookyMusic.Stop();
			Application.Current.Exit();
		}
		//Popup style
		private async void PopupMessage()
		{
			//Setting background color
			Color color = Color.FromArgb(255, 70, 130, 180);
			SolidColorBrush blueBrush = new SolidColorBrush(color);

			// Image
			Image congratsImage = new Image();
			congratsImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/popup2.png"));
			congratsImage.Width = 500;
			congratsImage.Height = 500;

			// TextBlock
			TextBlock messageTextBlock = new TextBlock();
			messageTextBlock.Text = "Hooray! You made it out of the Haunted House! Now, Level 2 awaits in the Haunted Garden. Use the Left and Right arrow keys to move. You only have 30 seconds to complete the challenge. Be super careful; those creatures are everywhere! Your challenge? Don't bump into any of them for 30 seconds. Can you do it? Let the adventure continue! Good luck!";
			messageTextBlock.TextWrapping = TextWrapping.WrapWholeWords;
			messageTextBlock.Foreground = new SolidColorBrush(Colors.White);

			StackPanel contentPanel = new StackPanel();
			contentPanel.Children.Add(congratsImage);
			contentPanel.Children.Add(messageTextBlock);
			contentPanel.Background = blueBrush; 

			// Create ContentDialog
			ContentDialog customDialog = new ContentDialog()
			{
				Title = "Haunted Garden **LEVEL 2**",
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
