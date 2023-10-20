using GameLibrary;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml;
//Name: Anastasiia Slyvka
//Date: October 15, 2023
//Project: Lab 1B - Game

//Threat class inherits from GamePiece and is used in Level3
public class Threat : GamePiece
{
	//setting bool IsHit to false
	public new bool IsHit { get; set; } = false;

	public Threat(Image img, string imageName) : base(img, imageName)
	{
	}

	//Creating a thread
	public static Threat CreateThreat(Grid grid, string imgSrc, int size, int left, int top)
	{
		Image img = CreateImage(imgSrc, size, left, top, grid);
		return new Threat(img, imgSrc);
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

	//Creating a random item
	public static Threat CreateRandomThreat(Grid grid, Random random)
	{
		int left = random.Next(250, 1350);
		string imgSrc = $"threat{random.Next(1, 5)}";
		return CreateThreat(grid, imgSrc, 130, left, 0);
	}
	//Method that updates threat's location from top to bottom
	public void MoveDown(double pixels)
	{
		UpdateLocation(pixels);
	}
	//Checking if the threat has been hit
	public void Hit()
	{
		this.IsHit = true;
		this.ImageVisibility = Visibility.Collapsed;
	}
}
