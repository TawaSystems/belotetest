using System;

using CoreGraphics;
using Foundation;
using SpriteKit;
using UIKit;
using BeloteClient;

namespace BLOTONLINE
{
	public class GuestScene : SKScene
	{
		public Game Game {
			get;
			set;
		}

		public GuestScene (IntPtr handle) : base (handle)
		{
		}

		public override void DidMoveToView (SKView view)
		{
			string txt = "";
			if (Game.AutorizationEmail ("a", "a"))
				txt = "Login";
			else
				txt = "((((";
			// Setup your scene here
			var myLabel = new SKLabelNode ("Chalkduster") {
				Text = txt,
				FontSize = 50,
				Position = new CGPoint (Frame.Width / 2, Frame.Height / 2)
			};


			AddChild (myLabel);
		}

		public override void TouchesBegan (NSSet touches, UIEvent evt)
		{
			// Create and configure the scene.
			var scene = SKNode.FromFile<EmailRegistrationScene> ("EmailRegistrationScene");
			scene.ScaleMode = SKSceneScaleMode.ResizeFill;

			this.View.PresentScene (scene);
		}

		public override void Update (double currentTime)
		{
			// Called before each frame is rendered
		}
	}
}

