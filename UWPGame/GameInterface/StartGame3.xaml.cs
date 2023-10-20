using GameLibrary;
using System;
using Windows.UI;
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
	public sealed partial class StartGame3 : Page
	{
		[System.Obsolete]
		Audio audio = new Audio();
		private string playerName;

		[System.Obsolete]
		public StartGame3()
		{
			this.InitializeComponent();
			//Displaying a popup message
			PopupMessage();

			// Creating a grid and setting a new background
			Grid grid = new Grid();
			grid.Background = new ImageBrush
			{
				ImageSource = new BitmapImage(new System.Uri("ms-appx:///Assets/StartGame3.jpg")),
				Stretch = Stretch.UniformToFill
			};

			// Play button style
			Button btnPlayLevel3 = new Button
			{
				Content = "Play",
				Margin = new Thickness(398, 477, 0, 0),
				VerticalAlignment = VerticalAlignment.Top,
				Height = 133,
				Width = 351,
				FontSize = 48,
				FontWeight = Windows.UI.Text.FontWeights.Bold,
				FontFamily = new FontFamily("Snap ITC"),
				Background = new SolidColorBrush(Windows.UI.Color.FromArgb(0xFF, 0x1D, 0x7B, 0x10)),
				BorderBrush = new SolidColorBrush(Windows.UI.Colors.Black)
			};
			//Button click
			btnPlayLevel3.Click += btnPlayLevel3_Click;

			//Exit button style
			Button btnExitGame3 = new Button
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
			//Exit button click
			btnExitGame3.Click += btnExitGame3_Click;

			MediaElement musicElement = audio.Level3;
			musicElement.Visibility = Visibility.Visible;
			grid.Children.Add(musicElement);
			// Adding buttons to the grid
			grid.Children.Add(btnPlayLevel3);
			grid.Children.Add(btnExitGame3);

			// Setting main grid as page content
			this.Content = grid;
			audio.Level3.Play();
		}
		//Navigate to Level3
		[System.Obsolete]
		private void btnPlayLevel3_Click(object sender, RoutedEventArgs e)
		{
			audio.Level3.Stop();
			Frame.Navigate(typeof(Level3), playerName);
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
		private void btnExitGame3_Click(object sender, RoutedEventArgs e)
		{
			audio.Level3.Stop();
			Application.Current.Exit();
		}

		//Popup style
		private async void PopupMessage()
		{
			// Nicer shade of blue
			Color color = Color.FromArgb(255, 57, 195, 20); 
			SolidColorBrush blueBrush = new SolidColorBrush(color);

			Image congratsImage = new Image();
			congratsImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/popup3.png"));
			congratsImage.Width = 500;
			congratsImage.Height = 500;

			// TextBlock
			TextBlock messageTextBlock = new TextBlock();
			messageTextBlock.Text = "Yay! After escaping the Haunted Garden, you've landed in Level 3: Escape from the Haunted Pumpkin Patch! Use the Left and Right arrow keys to move, grab all the pumpkins (miss 3 and uh-oh, start over again!), and be sure to avoid those sneaky ghosts and bats. Good Luck!";
			messageTextBlock.TextWrapping = TextWrapping.WrapWholeWords;
			messageTextBlock.Foreground = new SolidColorBrush(Colors.White); 
			
			StackPanel contentPanel = new StackPanel();
			contentPanel.Children.Add(congratsImage);
			contentPanel.Children.Add(messageTextBlock);
			contentPanel.Background = blueBrush; 

			ContentDialog customDialog = new ContentDialog()
			{
				Title = "Haunted Pumpkin Patch **LEVEL 3**",
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
