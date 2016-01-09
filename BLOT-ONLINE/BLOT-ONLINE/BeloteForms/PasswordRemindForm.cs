using System;

using CoreGraphics;
using Foundation;
using SpriteKit;
using UIKit;
using BeloteClient;

namespace BLOTONLINE
{
	public class PasswordRemindForm : BaseBelotePopupForm
	{
		private BeloteEdit EmailRemindEdit;
		private BeloteButton SendPasswordButton;

		public PasswordRemindForm (Game Game, BaseBeloteScene ParentScene, BeloteFormAction CloseAction, BaseBeloteForm ParentForm) : base(Game, ParentScene, CloseAction, ParentForm)
		{
			
		}

		public override void Show (float Width, float Height, float X, float Y)
		{
			base.Show (Width, Height, X, Y);

			SendPasswordButton = new BeloteButton ("SendPasswordButton", 400, 65, (Width / 2) - 200, (Height / 2) - 100, OnSendPasswordButtonClick, null, SKTexture.FromImageNamed("Textures/ActiveButton.png"), SKTexture.FromImageNamed("Textures/UnactiveButton.png"), "Отправить пароль на E-mail", 25);
			SendPasswordButton.Enabled = false;

			EmailRemindEdit = new BeloteEdit ("EmailRemindEdit", 400, 65, (Width / 2) - 200, (Height / 2), SKTexture.FromImageNamed("Textures/TextField.png"), "", false, OnEmailEndEditing);

			AddChildControl (SendPasswordButton);
			AddChildControl (EmailRemindEdit);
			EmailRemindEdit.TextField.BecomeFirstResponder ();
			this.AnimateWindow ();
		}

		public void OnSendPasswordButtonClick(BaseBeloteControl Sender)
		{
			Close ();
		}

		public void OnEmailEndEditing(BaseBeloteControl Sender)
		{
			if (EmailRemindEdit.Text != "")
				SendPasswordButton.Enabled = true;
			else
				SendPasswordButton.Enabled = false;
		}
	}
}

