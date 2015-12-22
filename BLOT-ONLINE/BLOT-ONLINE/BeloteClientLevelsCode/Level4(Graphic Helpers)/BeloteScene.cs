using System;
using CoreGraphics;
using Foundation;
using SpriteKit;
using UIKit;
using BeloteClient;

namespace BLOTONLINE
{
	public class BeloteScene : SKScene
	{
		protected CoordinatesDictionary coordinates;

		public BeloteScene (IntPtr handle) : base (handle)
		{
		}

		public Game Game {
			get;
			set;
		}

		protected virtual void FillCoordinates()
		{
			coordinates = new CoordinatesDictionary ();
		}

		public void Initialize(Game Game)
		{
			this.Game = Game;
			FillCoordinates ();
		}

		public CGPoint this[string Key]
		{
			get {
				return coordinates.GetValue (Key, Game.GraphicsProvider.DeviceType);
			}
		}
	}
}

