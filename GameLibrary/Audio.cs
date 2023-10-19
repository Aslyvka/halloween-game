using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
//Name: Anastasiia Slyvka
//Date: October 5, 2023
//Project: Lab 1 - Game

//Audio class corresponding the music for each level
namespace GameLibrary
{
	public class Audio
	{
		public MediaElement MainTheme { get; set; }
		public MediaElement BackgroundMusicRoom1 { get; set; }
		public MediaElement BackgroundMusicRoom2 { get; set; }
		public MediaElement BackgroundMusicGame { get; set; }
		public MediaElement BackgroundMusicGameOver1 { get; set; }
		public MediaElement BackgroundMusicGameOver2 { get; set; }
		public MediaElement BackgroundMusicGameOver3 { get; set; }
		public MediaElement SpookyMusic { get; set; }
		public MediaElement Level3 { get; set; }
		public MediaElement Finish { get; set; }

		//the source files added to the assets folder
		//creating new music elements
		[Obsolete]
		public Audio()
		{
			BackgroundMusicRoom1 = new MediaElement
			{
				Name = "backgroundMusicRoom1",
				Source = new Uri("ms-appx:///Assets/Room1.mp3"),
				AutoPlay = false,
				IsLooping = true,
				Width = 0,
				Height = 0,
				AudioCategory = AudioCategory.BackgroundCapableMedia,
				Visibility = Visibility.Collapsed
			};

			MainTheme = new MediaElement
			{
				Name = "MainTheme",
				Source = new Uri("ms-appx:///Assets/MainTheme.mp3"),
				AutoPlay = true,
				IsLooping = true,
				Width = 0,
				Height = 0,
				AudioCategory = AudioCategory.BackgroundCapableMedia,
				Visibility = Visibility.Collapsed
			};

			SpookyMusic = new MediaElement
			{
				Name = "SpookyMusic",
				Source = new Uri("ms-appx:///Assets/Spook2.mp3"),
				AutoPlay = true,
				IsLooping = true,
				Width = 0,
				Height = 0,
				AudioCategory = AudioCategory.BackgroundCapableMedia,
				Visibility = Visibility.Collapsed
			};

			Level3 = new MediaElement
			{
				Name = "Level3",
				Source = new Uri("ms-appx:///Assets/Level3.mp3"),
				AutoPlay = true,
				IsLooping = true,
				Width = 0,
				Height = 0,
				AudioCategory = AudioCategory.BackgroundCapableMedia,
				Visibility = Visibility.Collapsed
			};

			Finish = new MediaElement
			{
				Name = "Finish",
				Source = new Uri("ms-appx:///Assets/Finish.mp3"),
				AutoPlay = true,
				IsLooping = true,
				Width = 0,
				Height = 0,
				AudioCategory = AudioCategory.BackgroundCapableMedia,
				Visibility = Visibility.Collapsed
			};

			BackgroundMusicGame = new MediaElement
			{
				Name = "backgroundMusicGame",
				Source = new Uri("ms-appx:///Assets/Game.mp3"), 
				AutoPlay = true,
				IsLooping = true,
				Width = 0,
				Height = 0,
				AudioCategory = AudioCategory.BackgroundCapableMedia,
				Visibility = Visibility.Collapsed
			};

			BackgroundMusicGameOver1 = new MediaElement
			{
				Name = "backgroundMusicGameOver1",
				Source = new Uri("ms-appx:///Assets/Witch.mp3"),
				AutoPlay = true,
				IsLooping = true,
				Width = 0,
				Height = 0,
				AudioCategory = AudioCategory.BackgroundCapableMedia,
				Visibility = Visibility.Collapsed
			};

			BackgroundMusicGameOver2 = new MediaElement
			{
				Name = "backgroundMusicGameOver2",
				Source = new Uri("ms-appx:///Assets/monsters.mp3"),
				AutoPlay = true,
				IsLooping = true,
				Width = 0,
				Height = 0,
				AudioCategory = AudioCategory.BackgroundCapableMedia,
				Visibility = Visibility.Collapsed
			};

			BackgroundMusicGameOver3 = new MediaElement
			{
				Name = "backgroundMusicGameOver3",
				Source = new Uri("ms-appx:///Assets/EvilLaugh.mp3"),
				AutoPlay = true,
				IsLooping = true,
				Width = 0,
				Height = 0,
				AudioCategory = AudioCategory.BackgroundCapableMedia,
				Visibility = Visibility.Collapsed
			};
		}
		//stopping background music
		public void StopMusicRoom1()
		{
			BackgroundMusicRoom1.Stop();
		}
	}
}
