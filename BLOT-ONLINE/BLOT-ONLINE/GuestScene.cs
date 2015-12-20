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

		private void DrawBackGround()
		{
			SKTexture backTexture = SKTexture.FromImageNamed ("InterfaceImages/GuestScreen/background.png");
			SKSpriteNode backSprite = SKSpriteNode.FromTexture (backTexture);
			backSprite.Size = new CGSize(1344, 750);
			backSprite.Position = new CGPoint (0, 0);
			backSprite.AnchorPoint = new CGPoint (0, 0);
			backSprite.Name = "BackgroundSprite";
			AddChild (backSprite);

		}

		private void DrawFacebookButton()
		{
			SKTexture fbTexture = SKTexture.FromImageNamed ("InterfaceImages/GuestScreen/FacebookButton.png");
			SKSpriteNode fbSprite = SKSpriteNode.FromTexture (fbTexture);
			fbSprite.Size = new CGSize(227, 227);
			fbSprite.Position = new CGPoint (364, 750 - 150 - 227);
			fbSprite.AnchorPoint = new CGPoint (0, 0);
			fbSprite.Name = "FacebookButtonSprite";
			AddChild (fbSprite);
		}

		private void DrawVKButton()
		{
			SKTexture vkTexture = SKTexture.FromImageNamed ("InterfaceImages/GuestScreen/VKButton.png");
			SKSpriteNode vkSprite = SKSpriteNode.FromTexture (vkTexture);
			vkSprite.Size = new CGSize(227, 227);
			vkSprite.Position = new CGPoint (749, 750 - 150 - 227);
			vkSprite.AnchorPoint = new CGPoint (0, 0);
			vkSprite.Name = "VKButtonSprite";
			AddChild (vkSprite);
		}

		private void DrawTextFields()
		{
			SKTexture tfTexture = SKTexture.FromImageNamed ("InterfaceImages/GuestScreen/TextField.png");
			SKSpriteNode emailSprite = SKSpriteNode.FromTexture (tfTexture);
			emailSprite.Size = new CGSize(647, 65);
			emailSprite.Position = new CGPoint (347, 750 - 405 - 65);
			emailSprite.AnchorPoint = new CGPoint (0, 0);
			emailSprite.Name = "EmailFieldSprite";
			AddChild (emailSprite);
			SKSpriteNode passwordSprite = SKSpriteNode.FromTexture (tfTexture);
			passwordSprite.Size = new CGSize(647, 65);
			passwordSprite.Position = new CGPoint (347, 750 - 478 - 65);
			passwordSprite.AnchorPoint = new CGPoint (0, 0);
			passwordSprite.Name = "PasswordFieldSprite";
			AddChild (passwordSprite);
		}

		private void DrawRegistrationButton()
		{
		}

		private void DrawEnterButton()
		{
		}

		private void DrawLabels()
		{
			SKLabelNode enterLabel = new SKLabelNode ("Roboto") {
				Text = "Войти через: ",
				FontSize = 30,
				Position = new CGPoint (576, 750 - 98 - 30),
				HorizontalAlignmentMode = SKLabelHorizontalAlignmentMode.Left
			};
			AddChild (enterLabel);
		}


		public GuestScene (IntPtr handle) : base (handle)
		{
		}

		public override void DidMoveToView (SKView view)
		{
			DrawBackGround ();
			DrawFacebookButton ();
			DrawVKButton ();
			DrawTextFields ();
			DrawRegistrationButton ();
			DrawEnterButton ();
			DrawLabels ();
			/*string txt = "";
			if (Game.AutorizationEmail ("a", "a"))
				txt = "Login";
			else
				txt = "((((";
			// Setup your scene here
			var myLabel = new SKLabelNode ("Chalkduster") {
				Text = txt,
				FontSize = 50,
				Position = new CGPoint (Frame.Width / 2, Frame.Height / 2)
			};*/


			//AddChild (myLabel);
		}

		public override void TouchesBegan (NSSet touches, UIEvent evt)
		{
		}

		public override void Update (double currentTime)
		{
			// Called before each frame is rendered
		}
	}
}

