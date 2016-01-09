using System;

using CoreGraphics;
using Foundation;
using SpriteKit;
using UIKit;
using BeloteClient;

namespace BLOTONLINE
{
	public class BeloteCheckBox : BaseBeloteControl
	{
		private bool isCheck;
		private SKTexture CheckedTexture;
		private SKTexture UncheckedTexture;
		private string checkText;
		private SKLabelNode checkLabel;

		public BeloteCheckBox (string Name) : base(Name)
		{
			isCheck = false;
			CheckedTexture = SKTexture.FromImageNamed ("Textures/checkon.png");
			UncheckedTexture = SKTexture.FromImageNamed ("Textures/checkoff.png");
			this.Height = 40;
		}

		public BeloteCheckBox (string Name, float Width, float X, float Y, string Text) : this (Name)
		{
			this.Width = Width;
			this.X = X;
			this.Y = Y;
			this.checkText = Text;
			ConstructControl ();
		}

		private void CreateLabel()
		{
			if (Text != "") {
				checkLabel = new SKLabelNode ();
				checkLabel.FontName = "Roboto";
				checkLabel.FontColor = UIColor.White;
				checkLabel.FontSize = 20;
				checkLabel.Position = new CGPoint (40, (this.Height / 2) - 10);
				checkLabel.ZPosition = this.Z + 1;
				checkLabel.Text = this.Text;
				checkLabel.HorizontalAlignmentMode = SKLabelHorizontalAlignmentMode.Left;
				checkLabel.Name = this.Name + "Label";
				this.Sprite.AddChild (checkLabel);
			}
		}

		public override void ConstructControl ()
		{
			base.ConstructControl ();
			SKTexture startTexture = (Checked) ? CheckedTexture : UncheckedTexture;
			Sprite = SKSpriteNode.FromTexture (startTexture);
			((SKSpriteNode)Sprite).Size = new CGSize (this.Width, this.Height);
			((SKSpriteNode)Sprite).AnchorPoint = AnchorPoint;
			((SKSpriteNode)Sprite).Position = new CGPoint (this.X, this.Y);
			((SKSpriteNode)Sprite).ZPosition = this.Z;
			Sprite.Name = this.Name;
			CreateLabel ();
		}

		public override void Destroy ()
		{
			if (checkLabel != null)
				checkLabel.RemoveFromParent ();
			base.Destroy ();
		}
		public string Text
		{
			get {
				return checkText;
			}
			set {
				checkText = value;
				if (checkLabel != null)
					checkLabel.Text = value;
				else
					CreateLabel ();
			}
		}

		public bool Checked
		{
			get {
				return isCheck;
			}
			set {
				isCheck = value;
				if (Sprite != null)
					((SKSpriteNode)Sprite).Texture = (value) ? CheckedTexture : UncheckedTexture;
			}
		}
	}
}

