using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
//Name: Anastasiia Slyvka
//Date: October 5, 2023
//Project: Lab 1 - Game
namespace GameLibrary
{
	public class GameLevel3
	{
		// Fields
		private Grid gameGrid;

		// Public properties for TextBlocks
		public TextBlock lblScore { get; private set; }
		public TextBlock lblTimer { get; private set; }
		public TextBlock lblMissedItems { get; private set; }

		// Constructor
		[Obsolete]
		public GameLevel3(Grid grid)
		{
			gameGrid = grid;
			LoadGameGrid();
		}
		//Loading game grid and addin music element
		[Obsolete]
		private void LoadGameGrid()
		{
			Audio audio = new Audio();
			audio.StopMusicRoom1();
			MediaElement musicElement = audio.BackgroundMusicGame;
			musicElement.Visibility = Visibility.Visible;
			gameGrid.Children.Add(musicElement);
			musicElement.Play();

			// Background 
			ImageBrush backgroundBrush = new ImageBrush
			{
				ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/Level3.jpg")),
				Stretch = Stretch.UniformToFill
			};
			gameGrid.Background = backgroundBrush;

			// Animation 
			Border AnimationContainer = new Border
			{
				Name = "AnimationContainer",
				Background = new SolidColorBrush(Windows.UI.Colors.Transparent),
				Width = 400,
				Height = 400,
				Visibility = Visibility.Collapsed,
				VerticalAlignment = VerticalAlignment.Top,
				HorizontalAlignment = HorizontalAlignment.Left,
				Margin = new Thickness(339, 230, 0, 0)
			};
			gameGrid.Children.Add(AnimationContainer);

			//TextBlocks
			lblScore = new TextBlock
			{
				Name = "lblScore",
				Text = "Score: 0",
				FontSize = 60,
				VerticalAlignment = VerticalAlignment.Top,
				HorizontalAlignment = HorizontalAlignment.Left,
				FontFamily = new FontFamily("Snap ITC"),
				Foreground = new SolidColorBrush(Color.FromArgb(255, 187, 197, 212)),
				Margin = new Thickness(30, 6, 0, 0)
			};
			gameGrid.Children.Add(lblScore);

			lblMissedItems = new TextBlock
			{
				Name = "lblMissedItems",
				Text = "Missed: 0",
				FontSize = 60,
				VerticalAlignment = VerticalAlignment.Top,
				HorizontalAlignment = HorizontalAlignment.Left,
				FontFamily = new FontFamily("Snap ITC"),
				Foreground = new SolidColorBrush(Color.FromArgb(255, 187, 197, 212)),
				Margin = new Thickness(1530, 0, 0, 0)
			};
			gameGrid.Children.Add(lblMissedItems);

			lblTimer = new TextBlock
			{
				Name = "lblTimer",
				Text = "Timer: 30",
				FontSize = 60,
				VerticalAlignment = VerticalAlignment.Top,
				HorizontalAlignment = HorizontalAlignment.Left,
				FontFamily = new FontFamily("Snap ITC"),
				Foreground = new SolidColorBrush(Color.FromArgb(255, 187, 197, 212)),
				Margin = new Thickness(837, 6, 0, 0)
			};
			gameGrid.Children.Add(lblTimer);
		}
	}
}
