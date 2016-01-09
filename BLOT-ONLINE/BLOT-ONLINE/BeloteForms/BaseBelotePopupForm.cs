using System;

using CoreGraphics;
using Foundation;
using SpriteKit;
using UIKit;
using BeloteClient;

namespace BLOTONLINE
{
	public class BaseBelotePopupForm : BaseBeloteForm
	{
		protected BeloteButton CloseButton;

		public BaseBelotePopupForm (Game Game, BaseBeloteScene ParentScene, BeloteFormAction CloseAction, BaseBeloteForm ParentForm) : base(Game, ParentScene, CloseAction, ParentForm)
		{
			
		}

		// Показ всплывающей формы с заданными параметрами размера
		public override void Show(float Width, float Height, float X, float Y)
		{
			base.Show (Width, Height, X, - Y * 2);

			this.DrawBackground ("Textures/popupbg2.png", "PopupBackground");

			SKTexture activeButtonTexture = SKTexture.FromImageNamed ("Textures/PopupCloseActive.png");
			SKTexture unactiveButtonTexture = null;
			CloseButton = new BeloteButton ("PopupCloseButton", 70, 70, this.Width - 65, this.Height - 65, OnClosing, OnClosed, activeButtonTexture, unactiveButtonTexture);

			AddChildControl (CloseButton);
		}
			
		// Анимация всплывания
		public virtual void AnimateWindow()
		{
			SKAction showingAction = SKAction.MoveTo (new CGPoint (X, - Y / 2), 0.3);
			this.BackgroundSprite.RunAction (showingAction);

			this.Width = Width;
			this.Height = Height;
			this.X = X;
			this.Y = - Y / 2;
		}

		// Показ в половину родительской формы
		public void Show()
		{
			this.Show (this.Parent.Width / 2, this.Parent.Height / 2, this.Parent.Width / 4, this.Parent.Height / 4);
		}

		// Вызывается при старте нажатия на кнопку закрытия
		public virtual void OnClosing(BaseBeloteControl Sender)
		{
		}

		// Вызывается при заканчивании нажатия на кнопку закрытия
		public virtual void OnClosed(BaseBeloteControl Sender)
		{
			SKAction closingAction = SKAction.MoveTo (new CGPoint (X, - Y * 2), 0.3);
			this.BackgroundSprite.RunAction(closingAction, new Action(() => Close()));
		}

		// Обработка событий тапа
		public override void OnTouchesBegan (NSSet touches, UIEvent evt)
		{
			base.OnTouchesBegan (touches, evt);
		}

		public override void OnTouchesEnded (NSSet touches, UIEvent evt)
		{
			base.OnTouchesEnded (touches, evt);
		}

	}
}

