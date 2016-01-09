using System;

using CoreGraphics;
using Foundation;
using SpriteKit;
using UIKit;
using BeloteClient;

namespace BLOTONLINE
{
	public class CreationTableForm : BaseBelotePopupForm
	{
		private BeloteUpDown BetUpDown;
		private BeloteLabel BetLabel;

		private BeloteLabel LevelLabel;
		private BeloteUpDown LevelUpDown;

		private BeloteCheckBox AICheckBox;

		private BeloteCheckBox VIPCheckBox;

		private BeloteButton CreateTableButton;

		public CreationTableForm (Game Game, BaseBeloteScene ParentScene, BeloteFormAction CloseAction, BaseBeloteForm ParentForm) : base(Game, ParentScene, CloseAction, ParentForm)
		{
		}

		public override void Show (float Width, float Height, float X, float Y)
		{
			base.Show (Width, Height, X, Y);

			BetUpDown = new BeloteUpDown ("BetUpDown", 100, 400, 20, 250, 10, 20);
			LevelUpDown = new BeloteUpDown ("LevelUpDown", 300, 400, 0, 25, 1, 0);
			BetLabel = new BeloteLabel ("BetLabel", BetUpDown.X + BetUpDown.Width / 2, 480, "Choose bet for table:", UIColor.White, 20, "Roboto");
			LevelLabel = new BeloteLabel ("LevelLabel", LevelUpDown.X + LevelUpDown.Width / 2, 480, "Choose minimal player level:", UIColor.White, 20, "Roboto");
			AICheckBox = new BeloteCheckBox ("AICheck", 150, 100, 300, "Change player to AI");
			VIPCheckBox = new BeloteCheckBox ("VIPCheck", 150, 300, 300, "Only for VIP");
			VIPCheckBox.OnTouchStart = OnVIPCheckBoxClick;
			CreateTableButton = new BeloteButton ("CreateTableButton", 200, 65, this.Width / 2 - 100, 150, OnCreateTableClick, null, SKTexture.FromImageNamed ("Textures/ActiveButton"), null, "Create table!", 20, UIColor.White);

			AddChildControl (BetUpDown);
			AddChildControl (LevelUpDown);
			AddChildControl (BetLabel);
			AddChildControl (LevelLabel);
			AddChildControl (AICheckBox);
			AddChildControl (VIPCheckBox);
			AddChildControl (CreateTableButton);

			this.AnimateWindow ();
		}
			
		private void OnCreateTableClick(BaseBeloteControl Sender)
		{
			Close ();
		}

		private void OnVIPCheckBoxClick(BaseBeloteControl Sender)
		{
		}

	}
}

