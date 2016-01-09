using System;

using CoreGraphics;
using Foundation;
using SpriteKit;
using UIKit;
using BeloteClient;

namespace BLOTONLINE
{
	public class BeloteEdit : BaseBeloteControl
	{
		private string text;

		public BeloteEdit (string Name) : base(Name)
		{
			Text = "";
			SecureText = false;
		}

		public BeloteEdit (string Name, float Width, float Height, float X, float Y, 
			SKTexture Texture, string Text, bool SecureText, BeloteFormAction onEndEditing) : this(Name)
		{
			this.Width = Width;
			this.Height = Height;
			this.X = X;
			this.Y = Y;
			this.Texture = Texture;
			this.Text = Text;
			this.SecureText = SecureText;
			this.OnEndEditing = onEndEditing;
			ConstructControl ();
		}

		private void EndEditing(object Sender, EventArgs args)
		{
			if (TextField != null)
			{
				Text = TextField.Text;
				if (OnEndEditing != null)
					OnEndEditing (this);	
			}
		}

		// Конструирование компонента по заданным свойствам
		public override void ConstructControl ()
		{
			base.ConstructControl ();
			Sprite = SKSpriteNode.FromTexture (Texture);
			((SKSpriteNode)Sprite).Size = new CGSize (this.Width, this.Height);
			((SKSpriteNode)Sprite).AnchorPoint = AnchorPoint;
			((SKSpriteNode)Sprite).Position = new CGPoint (this.X, this.Y);
			((SKSpriteNode)Sprite).ZPosition = this.Z;
			Sprite.Name = this.Name;
		}
			
		public override void Destroy ()
		{
			base.Destroy ();
			TextField.RemoveFromSuperview ();
		}

		public override void ShowView (BaseBeloteScene Scene)
		{
			base.ShowView (Scene);
			CGPoint sceneCoordinates;
			if (Sprite.Parent.Frame.X == 0)
				sceneCoordinates = new CGPoint (Math.Abs(Sprite.Parent.Frame.X) + X, Math.Abs(Sprite.Parent.Frame.Y) + Y);
			else
				sceneCoordinates = new CGPoint (Math.Abs(Sprite.Parent.Frame.X) + X, Math.Abs(Sprite.Parent.Frame.Y));
			UITextField textField = new UITextField (CoordinatesTransmitor.ConvertRectFromSceneToView(Scene.View, (float)Scene.Frame.Width, (float)Scene.Frame.Height, Width, Height, (float)sceneCoordinates.X, (float)sceneCoordinates.Y));
			textField.BorderStyle = UITextBorderStyle.None;
			textField.Font = Font;
			textField.Text = this.Text;
			textField.SecureTextEntry = SecureText;
			textField.EditingChanged += EndEditing;
			textField.BackgroundColor = UIColor.Clear;
			this.TextField = textField;
			Scene.View.AddSubview (TextField);
		}

		public string Text
		{
			get {
				return text;
			}
			set {
				text = value;
				if (TextField != null)
					TextField.Text = value;
			}
		}

		public SKTexture Texture
		{
			get;
			set;
		}

		public UITextField TextField {
			get;
			private set;
		}

		public UIFont Font
		{
			get;
			set;
		}

		public BeloteFormAction OnEndEditing {
			get;
			set;
		}

		public bool SecureText
		{
			get;
			set;
		}
	}
}

