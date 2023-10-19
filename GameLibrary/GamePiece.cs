using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
//Name: Anastasiia Slyvka
//Date Created On: October 5, 2023
//Last Modified On: October 15, 2023
//Project: Lab 1 - Game

/* UWP Game Library
 * Written By: Melissa VanderLely
 * Modified By: Anastasiia Slyvka
 */

//Made this class abstract, so that other classes could inherit
namespace GameLibrary
{
    public abstract class GamePiece
    {
        private Thickness objectMargins;            //represents the location of the piece on the game board
        private Image onScreen;

        public bool IsHit { get; private set; } = false;

		public string ImageName { get; set; } // New property to hold the name of the image

		public GamePiece(Image img, string imageName) // Modified constructor
		{
			onScreen = img;
			objectMargins = img.Margin;
			ImageName = imageName; // Initialize the ImageName property
		}

		public Thickness Location                     //get access only - can not directly modify the location of the piece
        {
            get { return onScreen.Margin; }
        }

        public GamePiece(Image img)                 //constructor creates a piece and a reference to its associated image
        {                                           //use this to set up other GamePiece properties
            onScreen = img;
            objectMargins = img.Margin;
        }

		public GamePiece(object image)
		{
			Image = image;
		}

		public Visibility ImageVisibility
		{
			get { return onScreen.Visibility; }
			set { onScreen.Visibility = value; }
		}

		public object Image { get; set; }

		public double ImageHeight
		{
			get { return onScreen.Height; }
		}

		public double ImageWidth
		{
			get { return onScreen.Width; }
		}

		//New location based on key pressed
		//using 18px to move left right - for better speed and visual results in the game
		public bool Move(Windows.System.VirtualKey direction)
        {
            switch (direction)
            {
                case Windows.System.VirtualKey.Left:
                    objectMargins.Left -= 18;
                    break;
                case Windows.System.VirtualKey.Right:
                    objectMargins.Left += 18;
                    break;
                default:
                    return false;
            }
            onScreen.Margin = objectMargins;
			objectMargins = onScreen.Margin;
			return true;
        }

		//Detecting a proper collion with the default valu of 8 for contraction
		public bool CollisionDetected(GamePiece other)
		{
			double contraction = 8;

			double thisLeft = this.objectMargins.Left + contraction;
			double thisTop = this.objectMargins.Top + contraction;
			double thisRight = thisLeft + this.onScreen.Width - 2 * contraction;
			double thisBottom = thisTop + this.onScreen.Height - 2 * contraction;

			double otherLeft = other.objectMargins.Left + contraction;
			double otherTop = other.objectMargins.Top + contraction;
			double otherRight = otherLeft + other.onScreen.Width - 2 * contraction;
			double otherBottom = otherTop + other.onScreen.Height - 2 * contraction;

			if (thisRight < otherLeft || thisLeft > otherRight)
			{
				return false;
			}
			if (thisBottom < otherTop || thisTop > otherBottom)
			{
				return false;
			}
			return true;
		}

		// Updating the location
		public void UpdateLocation(double topIncrement)
		{
			var margin = onScreen.Margin;
			margin.Top += topIncrement;
			onScreen.Margin = margin;
			objectMargins = margin;
		}


	}
}
