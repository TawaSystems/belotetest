using System;

using CoreGraphics;
using Foundation;
using SpriteKit;
using UIKit;
using BeloteClient;

namespace BLOTONLINE
{
	public class BeloteLabel : BaseBeloteControl
	{
		private string labelText;
		private SKLabelHorizontalAlignmentMode alignment;

		public BeloteLabel (string Name) : base(Name)
		{
		}

		public BeloteLabel (string Name, float X, float Y, string Text, UIColor FontColor, float FontSize, string FontName) : this(Name)
		{
			this.X = X;
			this.Y = Y;
			this.Text = Text;
			this.FontName = FontName;
			this.FontSize = FontSize;
			this.FontColor = FontColor;
			this.HorizontalAlignment = SKLabelHorizontalAlignmentMode.Center;
			ConstructControl ();
		}

		// Конструирование компонента по заданным свойствам
		public override void ConstructControl ()
		{
			base.ConstructControl ();
			Sprite = new SKLabelNode ();
			((SKLabelNode)Sprite).FontName = FontName;
			((SKLabelNode)Sprite).FontColor = FontColor;
			((SKLabelNode)Sprite).FontSize = FontSize;
			((SKLabelNode)Sprite).Position = new CGPoint (this.X, this.Y);
			((SKLabelNode)Sprite).ZPosition = this.Z;
			((SKLabelNode)Sprite).Text = this.Text;
			((SKLabelNode)Sprite).HorizontalAlignmentMode = HorizontalAlignment;
			Sprite.Name = this.Name;
		}

		public override void Destroy ()
		{
			base.Destroy ();
		}

		public override void ShowView (BaseBeloteScene Scene)
		{
			base.ShowView (Scene);
		}

		public string Text
		{
			get {
				return labelText;
			}
			set {
				labelText = value;
				if (this.Sprite != null) {
					((SKLabelNode)Sprite).Text = value;
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

		public SKLabelHorizontalAlignmentMode HorizontalAlignment
		{
			get {
				return alignment;
			}
			set {
				alignment = value;
				if (Sprite != null)
					((SKLabelNode)Sprite).HorizontalAlignmentMode = value;
			}
		}
	}
}

