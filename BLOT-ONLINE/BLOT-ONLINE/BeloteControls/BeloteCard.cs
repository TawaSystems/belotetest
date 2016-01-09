using System;

using CoreGraphics;
using Foundation;
using SpriteKit;
using UIKit;
using BeloteClient;

namespace BLOTONLINE
{
	public class BeloteCard : BaseBeloteControl
	{
		private bool isHide;
		private SKTexture backTexture;
		private SKTexture cardTexture;

		public BeloteCard (string Name) : base(Name)
		{
			isHide = true;
		}

		public BeloteCard(CardSuit Suit, CardType Type, bool IsHide, float X, float Y, 
			BeloteFormAction onTouchStart, BeloteFormAction onTouchEnd) : this(Suit.ToString () + ((int)Type).ToString ())
		{
			this.Suit = Suit;
			this.Type = Type;
			this.IsHide = IsHide;
			this.Width = 104;
			this.Height = 152;
			this.X = X;
			this.Y = Y;
			this.OnTouchEnd = onTouchEnd;
			this.OnTouchStart = onTouchStart;
			backTexture = SKTexture.FromImageNamed ("Textures/cards/back.png");
			//cardTexture = SKTexture.FromImageNamed ("Textures/cards/" + this.Name + ".png");
			cardTexture = SKTexture.FromImageNamed("Textures/cards/c2.png");
			ConstructControl ();
		}

		public override void ConstructControl ()
		{
			base.ConstructControl ();
			Sprite = SKSpriteNode.FromTexture ((IsHide) ? backTexture : cardTexture);
			((SKSpriteNode)Sprite).Size = new CGSize (this.Width, this.Height);
			((SKSpriteNode)Sprite).AnchorPoint = AnchorPoint;
			((SKSpriteNode)Sprite).Position = new CGPoint (this.X, this.Y);
			((SKSpriteNode)Sprite).ZPosition = this.Z;
			Sprite.Name = this.Name;
		}

		public bool IsHide {
			get {
				return isHide;
			}
			set {
				isHide = value;
				if (Sprite != null)
					((SKSpriteNode)Sprite).Texture = (value) ? backTexture : cardTexture;
			}
		}

		public CardSuit Suit {
			get;
			private set;
		}

		public CardType Type
		{
			get;
			private set;
		}


	}
}

