using GameLibrary;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml;
//Name: Anastasiia Slyvka
//Date: October 15, 2023
//Project: Lab 1B - Game

//Inheritance from abstract GamePiece class
public class Enemy : GamePiece
{
	public new bool IsHit { get; set; } = false;

	public Enemy(Image img, string imageName) : base(img, imageName)
	{
	}

	//Method to create an enemy
	public static Enemy CreateEnemy(Grid grid, string imgSrc, int size, int left, int top)
	{
		Image img = CreateImage(imgSrc, size, left, top, grid);
		return new Enemy(img, imgSrc);
	}

	private static Image CreateImage(string imgSrc, int size, int left, int top, Grid grid)
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
		return img;
	}
	//creating enemies randomly
	public static Enemy CreateRandomItem(Grid grid, Random random)
	{
		int left = random.Next(200, 1400);
		string imgSrc = $"enemy{random.Next(1, 5)}";
		return CreateEnemy(grid, imgSrc, 130, left, 0);
	}

	//Method that updating enemies location and moving down
	public void MoveDown(double pixels)
	{
		UpdateLocation(pixels);
	}
	//Method that is checking if enemy has been hit
	public void Hit()
	{
		this.IsHit = true;
		this.ImageVisibility = Visibility.Collapsed;
	}
}
