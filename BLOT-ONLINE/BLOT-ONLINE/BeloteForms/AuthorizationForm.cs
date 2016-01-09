using System;

using CoreGraphics;
using Foundation;
using SpriteKit;
using UIKit;
using BeloteClient;

namespace BLOTONLINE
{
	public class AuthorizationForm : BaseBeloteForm
	{
		private BeloteButton VKButton;
		private BeloteButton FBButton;
		private BeloteEdit EmailEdit;
		private BeloteEdit PasswordEdit;
		private BeloteButton EnterButton;
		private BeloteButton RemindPasswordButton;
		private BeloteLabel EnterLabel;

		private PasswordRemindForm PasswordRemindForm;

		public AuthorizationForm (Game Game, BaseBeloteScene ParentScene, BeloteFormAction CloseAction, BaseBeloteForm ParentForm) : base(Game, ParentScene, CloseAction, ParentForm)
		{
			PasswordRemindForm = null;
		}

		public override void Show(float Width, float Height, float X, float Y)
		{
			base.Show (Width, Height, X, Y);

			DrawBackground ("Textures/woodbg.png", "AuthBackground");

			SKTexture textEditTexture = SKTexture.FromImageNamed ("Textures/TextField.png");
			SKTexture activeButtonTexture = SKTexture.FromImageNamed ("Textures/ActiveButton.png");
			SKTexture unactiveButtonTexture = SKTexture.FromImageNamed ("Textures/UnactiveButton.png");



			VKButton = new BeloteButton ("VKButton", 225, 225, (Width / 2) - 325, 378, OnVKButtonClick, null, SKTexture.FromImageNamed("Textures/GuestScreen/VKButton.png"), null);

			FBButton = new BeloteButton ("FBButton", 225, 225, (Width / 2) + (325 - 225), 378, OnFBButtonClick, null, SKTexture.FromImageNamed("Textures/GuestScreen/FacebookButton.png"), null);

			EmailEdit = new BeloteEdit ("Email", 650, 65, (Width / 2) - 325, 300, textEditTexture, "", false, OnEmailEndEditing);

			PasswordEdit = new BeloteEdit ("Password", 650, 65, (Width / 2) - 325, 225, textEditTexture, "", true, OnPasswordEndEditing);

			EnterButton = new BeloteButton ("EnterButton", 320, 65, (Width / 2) - 325, 150, OnEnterButtonClick, null, activeButtonTexture, unactiveButtonTexture, "Войти", 20);
			EnterButton.Enabled = false;

			RemindPasswordButton = new BeloteButton ("RemindButton", 320, 65, (Width / 2) + 5, 150, OnRemindPasswordButtonClick, null, activeButtonTexture, unactiveButtonTexture, "Напомнить пароль", 20);

			EnterLabel = new BeloteLabel ("EnterLabel", (Width / 2), 630, "Войти через: " + ParentScene.View.Frame.Width.ToString() + " " + ParentScene.View.Frame.Height.ToString(), UIColor.White, 30, "Roboto");

			AddChildControl (VKButton);
			AddChildControl (FBButton);
			AddChildControl (EmailEdit);
			AddChildControl (PasswordEdit);
			AddChildControl (EnterButton);
			AddChildControl (RemindPasswordButton);
			AddChildControl (EnterLabel);
		}

		public void OnVKButtonClick(BaseBeloteControl Sender)
		{
			EnterLabel.Text = "VK";
		}

		public void OnFBButtonClick(BaseBeloteControl Sender)
		{
			EnterLabel.Text = "FB";

		}

		public void OnEnterButtonClick(BaseBeloteControl Sender)
		{
			EnterLabel.Text = "Enter";
			EmailEdit.Text = "";
			PasswordEdit.Text = "";
			Close ();
		}

		public void OnRemindPasswordButtonClick(BaseBeloteControl Sender)
		{
			EnterLabel.Text = "Remind";
			EmailEdit.Text = "";
			PasswordEdit.Text = "";
			PasswordRemindForm = new PasswordRemindForm (this.Game, this.ParentScene, OnClosePasswordRemindForm, this);
			PasswordRemindForm.Show ();
			this.ParentScene.ChangeActiveForm (PasswordRemindForm);
			PasswordEdit.TextField.Enabled = false;
			EmailEdit.TextField.Enabled = false;
		}

		public void OnEmailEndEditing(BaseBeloteControl Sender)
		{
			if (EmailEdit.Text != "")
				EnterButton.Enabled = true;
			else
				EnterButton.Enabled = false;
			EnterLabel.Text = "Email Edited";
		}

		public void OnPasswordEndEditing(BaseBeloteControl Sender)
		{
			EnterLabel.Text = "PasswordEdited";
		}

		public void OnClosePasswordRemindForm(BaseBeloteControl Sender)
		{
			this.ParentScene.ChangeActiveForm (this);
			PasswordEdit.TextField.Enabled = true;
			EmailEdit.TextField.Enabled = true;
			PasswordRemindForm = null;
		}

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

