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

			BetUpDown = new BeloteUpDown ("BetUpDown", (Width / 2) - 250, (Height / 5) * 3, 20, 250, 10, 20);
			LevelUpDown = new BeloteUpDown ("LevelUpDown", (Width / 2) + 50, (Height / 5) * 3, 0, 25, 1, 0);

			BetLabel = new BeloteLabel ("BetLabel", BetUpDown.X + BetUpDown.Width / 2, (Height / 5) * 3 + 75, "Choose bet for table:", UIColor.White, 20, "Roboto");
			LevelLabel = new BeloteLabel ("LevelLabel", LevelUpDown.X + LevelUpDown.Width / 2, (Height / 5) * 3 + 75, "Choose minimal player level:", UIColor.White, 20, "Roboto");
			AICheckBox = new BeloteCheckBox ("AICheck", 200, (Width / 2) - 275, (Height / 2) - 40, "Change player to AI");
			VIPCheckBox = new BeloteCheckBox ("VIPCheck", 200, (Width / 2) + 75, (Height / 2) - 40, "Only for VIP");
			VIPCheckBox.OnTouchStart = OnVIPCheckBoxClick; //ПЕРЕКРЫВАЕТСЯ МЕТОД ИЗМЕНЕНИЯ CHECK
			CreateTableButton = new BeloteButton ("CreateTableButton", 300, 65, this.Width / 2 - 150, Height / 6, OnCreateTableClick, null, SKTexture.FromImageNamed ("Textures/ActiveButton"), null, "Create table!", 20, UIColor.White);

			AddChildControl (BetUpDown);
			AddChildControl (LevelUpDown);
			AddChildControl (BetLabel);
			AddChildControl (LevelLabel);
			AddChildControl (AICheckBox);
			AddChildControl (VIPCheckBox);
			AddChildControl (CreateTableButton);

			this.AnimateWindow ();
		}
			
		private void OnCreateTableClick(BaseBeloteControl Sender, string SpriteName)
		{
			Close ();
		}

		private void OnVIPCheckBoxClick(BaseBeloteControl Sender, string SpriteName)
		{
		}

	}
}

