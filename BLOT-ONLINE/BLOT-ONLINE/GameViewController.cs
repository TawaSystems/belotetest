using System;

using SpriteKit;
using UIKit;
using BeloteClient;

namespace BLOTONLINE
{
	public partial class GameViewController : UIViewController
	{
		public Game Game {
			get;
			private set;
		}

		public GameViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			Game = new Game ();

			// Configure the view.
			var skView = (SKView)View;
			skView.ShowsFPS = true;
			skView.ShowsNodeCount = true;
			/* Sprite Kit applies additional optimizations to improve rendering performance */
			//skView.IgnoresSiblingOrder = true;

			// Create and configure the scene.
			GuestScene scene = SKNode.FromFile<GuestScene> ("GuestScene");
			scene.Game = this.Game;
			scene.ScaleMode = SKSceneScaleMode.Fill;
			// Present the scene.
			skView.PresentScene (scene);
		}

		public override bool ShouldAutorotate ()
		{
			return true;
		}

		public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations ()
		{
			return UIInterfaceOrientationMask.Landscape;
			//return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone ? UIInterfaceOrientationMask.AllButUpsideDown : UIInterfaceOrientationMask.All;
		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}

		public override bool PrefersStatusBarHidden ()
		{
			return true;
		}
	}
}

