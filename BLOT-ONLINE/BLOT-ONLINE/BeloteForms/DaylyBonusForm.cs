using System;

using CoreGraphics;
using Foundation;
using SpriteKit;
using UIKit;
using BeloteClient;

namespace BLOTONLINE
{
	public class DaylyBonusForm : BaseBelotePopupForm
	{
		private BeloteCard[] BonusCards;
		private BeloteLabel PrizeLabel;
		private BeloteLabel InfoBonusLabel;

		public DaylyBonusForm (Game Game, BaseBeloteScene ParentScene, BeloteFormAction CloseAction, BaseBeloteForm ParentForm) : base(Game, ParentScene, CloseAction, ParentForm)
		{
		}

		public override void Show (float Width, float Height, float X, float Y)
		{
			base.Show (Width, Height, X, Y);

			BonusCards = new BeloteCard[8];

			CardsDeck deck = new CardsDeck ();
			for (var i = 0; i < 8; i++) {
				Card c = deck.GetRandomCard ();
				BonusCards[i] = new BeloteCard(c.Suit, c.Type, true, (i * (120) + 100), (this.Height / 2) - 76, OnCardChoose, null);
				AddChildControl (BonusCards [i]);
			}

			InfoBonusLabel = new BeloteLabel ("InfoBonusLabel", this.Width / 2, this.Height / 2 + 100, "Выберите карту, чтобы получить Ваш дневной бонус", UIColor.White, 20, "Roboto");
			AddChildControl (InfoBonusLabel);

			this.AnimateWindow ();
		}

		public void OnCardChoose(BaseBeloteControl Sender, string SpriteName)
		{
			for (var i = 0; i < 8; i++) {
				if (Sender != BonusCards[i])
					BonusCards [i].Sprite.RunAction (SKAction.Sequence(SKAction.MoveTo (new CGPoint (Sender.X, Sender.Y), 0.3), SKAction.RemoveFromParent()));
			}
			((BeloteCard)Sender).IsHide = false;
			int Count = ((int)((BeloteCard)Sender).Type) * 10;
			Sender.Sprite.RunAction (SKAction.MoveTo (new CGPoint (95, (this.Height / 2) - 76), 0.5), new Action (() => {
				PrizeLabel = new BeloteLabel("PrizeLabel", 250, (this.Height / 2) - 20, String.Format("Поздравляем! Вы получили {0} бесплатных фишек!", Count), UIColor.White, 30, "Roboto");
				PrizeLabel.HorizontalAlignment = SKLabelHorizontalAlignmentMode.Left;
				AddChildControl(PrizeLabel);
			}));
		}
	}
}

