using System;

using CoreGraphics;
using Foundation;
using SpriteKit;
using UIKit;
using BeloteClient;

namespace BLOTONLINE
{
	public class GuestScene : BeloteScene
	{
		private void DrawBackGround()
		{
			SKTexture backTexture;
			backTexture = SKTexture.FromImageNamed (Game.GraphicsProvider.Path + "GuestScreen/background.png");
			//else
			//	backTexture = SKTexture.FromImageNamed ("InterfaceImages/GuestScreen/2X.png");
			SKSpriteNode backSprite = SKSpriteNode.FromTexture (backTexture);
			backSprite.Size = new CGSize(1344, 750);
			backSprite.Position = new CGPoint (0, 0);
			backSprite.AnchorPoint = new CGPoint (0, 0);
			backSprite.Name = "BackgroundSprite";
			AddChild (backSprite);

		}

		private void DrawFacebookButton()
		{
			SKTexture fbTexture = SKTexture.FromImageNamed (Game.GraphicsProvider.Path + "GuestScreen/FacebookButton.png");
			SKSpriteNode fbSprite = SKSpriteNode.FromTexture (fbTexture);
			fbSprite.Size = new CGSize(227, 227);
			fbSprite.Position = this["FBButton"];
			fbSprite.AnchorPoint = new CGPoint (0, 0);
			fbSprite.Name = "FacebookButtonSprite";
			AddChild (fbSprite);
		}

		private void DrawVKButton()
		{
			SKTexture vkTexture = SKTexture.FromImageNamed (Game.GraphicsProvider.Path + "GuestScreen/VKButton.png");
			SKSpriteNode vkSprite = SKSpriteNode.FromTexture (vkTexture);
			vkSprite.Size = new CGSize(227, 227);
			vkSprite.Position = this["VKButton"];
			vkSprite.AnchorPoint = new CGPoint (0, 0);
			vkSprite.Name = "VKButtonSprite";
			AddChild (vkSprite);
		}

		private void DrawTextFields()
		{
			SKTexture tfTexture = SKTexture.FromImageNamed (Game.GraphicsProvider.Path + "GuestScreen/TextField.png");
			SKSpriteNode emailSprite = SKSpriteNode.FromTexture (tfTexture);
			emailSprite.Size = new CGSize(647, 65);
			emailSprite.Position = this["Email"];
			emailSprite.AnchorPoint = new CGPoint (0, 0);
			emailSprite.Name = "EmailFieldSprite";
			AddChild (emailSprite);
			SKSpriteNode passwordSprite = SKSpriteNode.FromTexture (tfTexture);
			passwordSprite.Size = new CGSize(647, 65);
			passwordSprite.Position = this["Password"];
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
				Position = this["EnterLabel"],
				HorizontalAlignmentMode = SKLabelHorizontalAlignmentMode.Left
			};
			AddChild (enterLabel);
		}


		public GuestScene (IntPtr handle) : base (handle)
		{
		}

		protected override void FillCoordinates()
		{
			base.FillCoordinates ();
			coordinates.AddValue ("VKButton", DeviceType.IPhone_5_5S, new CGPoint (749, 378));
			coordinates.AddValue ("VKButton", DeviceType.IPhone_6_6S, new CGPoint (749, 378));
			coordinates.AddValue ("FBButton", DeviceType.IPhone_5_5S, new CGPoint (364, 378));
			coordinates.AddValue ("FBButton", DeviceType.IPhone_6_6S, new CGPoint (364, 378));
			coordinates.AddValue ("Email", DeviceType.IPhone_5_5S, new CGPoint (347, 300));
			coordinates.AddValue ("Email", DeviceType.IPhone_6_6S, new CGPoint (347, 300));
			coordinates.AddValue ("Password", DeviceType.IPhone_5_5S, new CGPoint (347, 225));
			coordinates.AddValue ("Password", DeviceType.IPhone_6_6S, new CGPoint (347, 225));
			coordinates.AddValue ("EnterLabel", DeviceType.IPhone_5_5S, new CGPoint (576, 750 - 98 - 30));
			coordinates.AddValue ("EnterLabel", DeviceType.IPhone_6_6S, new CGPoint (576, 750 - 98 - 30));

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

