using GameLibrary;
using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
//Name: Anastasiia Slyvka
//Date: October 5, 2023
//Project: Lab 1 - Game
namespace GameInterface
{
	public sealed partial class Finish : Page
	{
		private readonly Audio audio;

		//Creating and loading a main grid into our level, adding audio, popup and background image
		[System.Obsolete]
		public Finish()
		{
			this.InitializeComponent();

			//When the page loads - the user will see a popup message
			PopupMessage();

			//Creating new instance of Audio and Grid
			audio = new Audio();
			Grid grid = new Grid();

			//Setting a background with an image
			grid.Background = new ImageBrush
			{
				ImageSource = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new System.Uri("ms-appx:///Assets/Finish.jpg")),
				Stretch = Stretch.UniformToFill
			};

			//Button to exit
			Button btnExitGame = new Button
			{
				Content = "Exit",
				Margin = new Thickness(450, 450, 0, 0),
				VerticalAlignment = VerticalAlignment.Top,
				Height = 120,
				Width = 300,
				FontSize = 48,
				FontWeight = Windows.UI.Text.FontWeights.Bold,
				FontFamily = new FontFamily("Snap ITC"),
				BorderBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(0xFF, 0x00, 0x00, 0x00))
			};
			//Generating click even when the user clicked the button
			btnExitGame.Click += btnExitGame_Click;
			grid.Children.Add(btnExitGame);
			this.Content = grid;

			//Adding music element that will play a Finish music
			MediaElement musicElement = audio.Finish;
			musicElement.Visibility = Visibility.Visible;
			grid.Children.Add(musicElement);
			audio.Finish.Play();
		}
		//Stopping the audio and exiting the game
		private void btnExitGame_Click(object sender, RoutedEventArgs e)
		{
			audio.Finish.Stop();
			Application.Current.Exit();
		}

		//Styled popup with new color, image, describing the end of the game
		private async void PopupMessage()
		{
			Color color = Color.FromArgb(255, 70, 130, 180);
			SolidColorBrush blueBrush = new SolidColorBrush(color);

			Image congratsImage = new Image();
			congratsImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/popup4.png"));
			congratsImage.Width = 500;
			congratsImage.Height = 500;

			TextBlock messageTextBlock = new TextBlock();
			messageTextBlock.Text = "Congrats! You safely escaped, found your friends, and now it's trick or treat time!";
			messageTextBlock.TextWrapping = TextWrapping.WrapWholeWords;
			messageTextBlock.Foreground = new SolidColorBrush(Colors.White);

			StackPanel contentPanel = new StackPanel();
			contentPanel.Children.Add(congratsImage);
			contentPanel.Children.Add(messageTextBlock);
			contentPanel.Background = blueBrush; 

			ContentDialog customDialog = new ContentDialog()
			{
				Title = "FINISH",
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
