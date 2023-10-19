using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
//Name: Anastasiia Slyvka
//Date: October 15, 2023
//Project: Lab 1B - Game

namespace GameLibrary
{
	//Inheritance from GamePiece abstract class
	public class CollectibleItem : GamePiece
	{
		public bool IsCollected { get; private set; } = false;

		public CollectibleItem(Image img, string imageName) : base(img, imageName)
		{
		}
		//Methd to collecte
		public void Collect()
		{
			this.ImageVisibility = Visibility.Collapsed;
			IsCollected = true;
		}

		// Using method to create a random item which will be used in Level1
		public static CollectibleItem CreateRandomItem(Grid grid, Random random)
		{
			int left = random.Next(300, 1400);
			string imgSrc = $"item{random.Next(1, 5)}";
			return CreatePiece(grid, imgSrc, 70, left, 0);
		}
		// Creating random pumpkins for the 3 game level
		public static CollectibleItem CreatePumpkin(Grid grid, Random random)
		{
			int left = random.Next(500, 1200);
			int pumpkinNumber = random.Next(1, 7);
			return CreatePiece(grid, $"pumpkin{pumpkinNumber}", 110, left, 0);
		}


		private static CollectibleItem CreatePiece(Grid grid, string imgSrc, int size, int left, int top)
		{
			Image img = new Image()
			{
				Source = new BitmapImage(new Uri($"ms-appx:///Assets/{imgSrc}.png")),
				Width = size,
				Height = size,
				Name = $"img{imgSrc}",
				Margin = new Thickness(left, top, 0, 20),
				VerticalAlignment = VerticalAlignment.Top,
				HorizontalAlignment = HorizontalAlignment.Left
			};
			grid.Children.Add(img);
			return new CollectibleItem(img, imgSrc);
		}

	}
}
