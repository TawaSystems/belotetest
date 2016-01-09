using System;

using CoreGraphics;
using Foundation;
using SpriteKit;
using UIKit;
using BeloteClient;

namespace BLOTONLINE
{
	public class BeloteButton : BaseBeloteControl
	{
		// Активность
		private bool enabled;
		// Надпись на кнопке
		private string buttonText;
		private SKLabelNode ButtonLabel;

		// Конструктор по имени
		public BeloteButton (string Name) : base(Name)
		{
			ButtonLabel = null;
			enabled = true;
		}

		public BeloteButton(string Name, float Width, float Height, float X, float Y, 
			BeloteFormAction onTouchStart, BeloteFormAction onTouchEnd, SKTexture EnabledTexture, SKTexture DisabledTexture = null, string Text = "",
			float FontSize = 12, UIColor FontColor = null, string FontName = "Roboto") : this(Name)
		{
			this.Width = Width;
			this.Height = Height;
			this.X = X;
			this.Y = Y;
			this.OnTouchStart = onTouchStart;
			this.OnTouchEnd = onTouchEnd;
			this.EnabledTexture = EnabledTexture;
			this.DisabledTexture = DisabledTexture;
			buttonText = Text;
			this.FontName = FontName;
			this.FontSize = FontSize;
			if (FontColor != null)
				this.FontColor = FontColor;
			else
				this.FontColor = UIColor.White;
			ConstructControl ();
		}

		// Создание надписи на кнопке
		private void CreateButtonLabel()
		{
			if (Text != "") {
				ButtonLabel = new SKLabelNode ();
				ButtonLabel.FontName = FontName;
				ButtonLabel.FontColor = FontColor;
				ButtonLabel.FontSize = FontSize;
				ButtonLabel.Position = new CGPoint (this.Width / 2, (this.Height / 2) - (this.FontSize / 2));
				ButtonLabel.ZPosition = this.Z + 1;
				ButtonLabel.Text = this.Text;
				ButtonLabel.HorizontalAlignmentMode = SKLabelHorizontalAlignmentMode.Center;
				ButtonLabel.Name = this.Name + "Label";
				this.Sprite.AddChild (ButtonLabel);
			}
		}

		// Конструирование компонента по заданным свойствам
		public override void ConstructControl ()
		{
			base.ConstructControl ();
			SKTexture startTexture = (Enabled) ? EnabledTexture : DisabledTexture;
			Sprite = SKSpriteNode.FromTexture (startTexture);
			((SKSpriteNode)Sprite).Size = new CGSize (this.Width, this.Height);
			((SKSpriteNode)Sprite).AnchorPoint = AnchorPoint;
			((SKSpriteNode)Sprite).Position = new CGPoint (this.X, this.Y);
			((SKSpriteNode)Sprite).ZPosition = this.Z;
			Sprite.Name = this.Name;

			CreateButtonLabel ();
		}

		public override void Destroy ()
		{
			if (ButtonLabel != null) {
				ButtonLabel.RemoveFromParent ();
			}
			base.Destroy ();
		}

		public override void ShowView (BaseBeloteScene Scene)
		{
			base.ShowView (Scene);
		}

		// Изменение свойства активности
		public override bool Enabled
		{
			get
			{ 
				return enabled;
			}
			set
			{ 
				enabled = value;
				if (Sprite != null) {
					if (value)
						((SKSpriteNode)Sprite).Texture = EnabledTexture;
					else
						((SKSpriteNode)Sprite).Texture = DisabledTexture;
				}
			}
		}

		// Текстура активности 
		public SKTexture EnabledTexture
		{
			get;
			set;
		}

		// Текстура неактивности
		public SKTexture DisabledTexture {
			get;
			set;
		}

		public string Text {
			get {
				return buttonText;
			}
			set {
				buttonText = value;
				if (value != "") {
					if (ButtonLabel != null) {
						ButtonLabel.Text = value;
					} else {
						CreateButtonLabel ();
					}
				}
			}
		}

		public UIColor FontColor
		{
			get;
			set;
		}

		public float FontSize
		{
			get;
			set;
		}

		public string FontName
		{
			get;
			set;
		}
	}
}

