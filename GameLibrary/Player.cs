using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
//Name: Anastasiia Slyvka
//Date: October 15, 2023
//Project: Lab 1B - Game
namespace GameLibrary
{
	//Player class inherits from the GamePiece
	public class Player : GamePiece
	{
		public Player(Image img, string imageName) : base(img, imageName)
		{
		}

		public static Player CreatePlayer(Grid grid, string imgSrc, int size, int left, int top)
		{
			Image img = new Image()
			{
				Source = new BitmapImage(new Uri($"ms-appx:///Assets/{imgSrc}.png")),
				Width = size,
				Height = size,
				Margin = new Thickness(left, top, 0, 0),
				VerticalAlignment = VerticalAlignment.Top,
				HorizontalAlignment = HorizontalAlignment.Left
			};
			grid.Children.Add(img);
			return new Player(img, imgSrc);
		}

		//Method to move the player in Level1
		public void OnKeyDown(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs e, double containerWidth)
		{
			int marginLeft = 40;
			int marginRight = (int)containerWidth - 40 - (int)ImageWidth;

			if (e.VirtualKey == Windows.System.VirtualKey.Left && Location.Left > marginLeft)
			{
				Move(e.VirtualKey);
			}
			else if (e.VirtualKey == Windows.System.VirtualKey.Right && Location.Left < marginRight)
			{
				Move(e.VirtualKey);
			}
		}
		//Method to move the player in Level2
		public void HandleKeyDown(Windows.System.VirtualKey virtualKey, double containerWidth)
		{
			int marginLeft = 310; // 80-pixel margin on the left
			int marginRight = (int)containerWidth - 310 - (int)ImageWidth; // 80-pixel margin on the right

			if (virtualKey == Windows.System.VirtualKey.Left)
			{
				// Move left while ensuring the player doesn't go beyond the left boundary
				if (Location.Left > marginLeft)
				{
					Move(virtualKey);
				}
			}
			else if (virtualKey == Windows.System.VirtualKey.Right)
			{
				// Move right while ensuring the player doesn't go beyond the right boundary
				if (Location.Left < marginRight)
				{
					Move(virtualKey);
				}
			}
		}
		//Method to move the player in Level3
		public void HandleKeyPress(Windows.System.VirtualKey virtualKey, double containerWidth, List<CollectibleItem> items)
		{
			int marginLeft = 100;
			int marginRight = (int)containerWidth - 100 - (int)ImageWidth;

			if (virtualKey == Windows.System.VirtualKey.Left)
			{
				if (Location.Left > marginLeft)
				{
					Move(virtualKey);
				}
			}
			else if (virtualKey == Windows.System.VirtualKey.Right)
			{
				if (Location.Left < marginRight)
				{
					Move(virtualKey);
				}
			}
		}
	}
}
