using System;

using CoreGraphics;
using Foundation;
using SpriteKit;
using UIKit;
using BeloteClient;

namespace BLOTONLINE
{
	public class TablesSelectOptionsForm : BaseBelotePopupForm
	{
		private BeloteLabel BetSizeLabel, BetToLabel;
		private BeloteUpDown BetFromUpDown, BetToUpDown;
		private BeloteCheckBox BetNoMatterCheck;

		private BeloteLabel MinLevelLabel;
		private BeloteUpDown MinLevelUpDown;
		private BeloteCheckBox MinLevelNoMatterCheck;

		private BeloteCheckBox AIPossibleCheck, AIInpossibleCheck, AINoMatterCheck;

		private BeloteCheckBox VIPPossibleCheck, VIPInpossibleCheck, VIPNoMatterCheck;

		private BeloteButton ConfirmButton;

		public TablesSelectOptionsForm (Game Game, BaseBeloteScene ParentScene, BeloteFormAction CloseAction, BaseBeloteForm ParentForm) : base(Game, ParentScene, CloseAction, ParentForm)
		{
		}

		public override void Show (float Width, float Height, float X, float Y)
		{
			base.Show (Width, Height, X, Y);

			BetSizeLabel = new BeloteLabel ("BetSizeLabel", 100, Height - 100, "Bet size:      From ", UIColor.White, 22, "Roboto");
			BetSizeLabel.HorizontalAlignment = SKLabelHorizontalAlignmentMode.Left;
			BetFromUpDown = new BeloteUpDown ("BetFromUpDown", 330, Height - 130, 20, 250, 10, 20, OnBetFromChanged);
			BetToLabel = new BeloteLabel ("BetToLabel", 540, Height - 100, " To ", UIColor.White, 22, "Roboto");
			BetToLabel.HorizontalAlignment = SKLabelHorizontalAlignmentMode.Left;
			BetToUpDown = new BeloteUpDown ("BetToUpDown", 590, Height - 130, 20, 250, 10, 250, OnBetToChanged);
			BetNoMatterCheck = new BeloteCheckBox ("BetNoMatterCheck", 100, 810, Height - 130, "No matter");

			MinLevelLabel = new BeloteLabel ("MinLevelLabel", 100, Height - 170, "Minimal player level: ", UIColor.White, 22, "Roboto");
			MinLevelLabel.HorizontalAlignment = SKLabelHorizontalAlignmentMode.Left;
			MinLevelUpDown = new BeloteUpDown ("MinLevelUpDown", 330, Height - 200, 0, 25, 1, 0);
			MinLevelNoMatterCheck = new BeloteCheckBox ("MinLevelNoMatterCheck", 100, 550, Height - 200, "No matter");

			AIPossibleCheck = new BeloteCheckBox ("AIPossibleCheck", 150, 100, Height - 280, "AI Possible");
			AIPossibleCheck.OnTouchStart = OnAICheck;
			AIInpossibleCheck = new BeloteCheckBox ("AIInpossibleCheck", 150, 350, Height - 280, "AI Inpossible");
			AIInpossibleCheck.OnTouchStart = OnAICheck;
			AINoMatterCheck = new BeloteCheckBox ("AINoMatterCheck", 150, 600, Height - 280, "AI No Matter");
			AINoMatterCheck.OnTouchStart = OnAICheck;

			VIPPossibleCheck = new BeloteCheckBox ("VIPPossibleCheck", 150, 100, Height - 360, "VIP Possible");
			VIPPossibleCheck.OnTouchStart = OnVIPCheck;
			VIPInpossibleCheck = new BeloteCheckBox ("VIPInpossibleCheck", 150, 350, Height - 360, "VIP Inpossible");
			VIPInpossibleCheck.OnTouchStart = OnVIPCheck;
			VIPNoMatterCheck = new BeloteCheckBox ("VIPNoMatterCheck", 150, 600, Height - 360, "VIP No Matter");
			VIPNoMatterCheck.OnTouchStart = OnVIPCheck;

			ConfirmButton = new BeloteButton ("ConfirmButton", 300, 65, Width / 2 - 150, Height - 470, OnConfirmClick, null, SKTexture.FromImageNamed ("Textures/ActiveButton.png"), null, "Confirm options", 20);

			AddChildControl (BetSizeLabel);
			AddChildControl (BetFromUpDown);
			AddChildControl (BetToLabel);
			AddChildControl (BetToUpDown);
			AddChildControl (BetNoMatterCheck);

			AddChildControl (MinLevelLabel);
			AddChildControl (MinLevelUpDown);
			AddChildControl (MinLevelNoMatterCheck);

			AddChildControl (AIPossibleCheck);
			AddChildControl (AIInpossibleCheck);
			AddChildControl (AINoMatterCheck);

			AddChildControl (VIPPossibleCheck);
			AddChildControl (VIPInpossibleCheck);
			AddChildControl (VIPNoMatterCheck);

			AddChildControl (ConfirmButton);

			this.AnimateWindow ();
		}

		private void OnAICheck(BaseBeloteControl Sender, string SpriteName)
		{
			if (Sender.Name == AIPossibleCheck.Name) {
				AIInpossibleCheck.Checked = false;
				AINoMatterCheck.Checked = false;
			}
			if (Sender.Name == AIInpossibleCheck.Name) {
				AIPossibleCheck.Checked = false;
				AINoMatterCheck.Checked = false;
			}
			if (Sender.Name == AINoMatterCheck.Name) {
				AIPossibleCheck.Checked = false;
				AIInpossibleCheck.Checked = false;
			}
		}

		private void OnVIPCheck(BaseBeloteControl Sender, string SpriteName)
		{
			if (Sender.Name == VIPPossibleCheck.Name) {
				VIPInpossibleCheck.Checked = false;
				VIPNoMatterCheck.Checked = false;
			}
			if (Sender.Name == VIPInpossibleCheck.Name) {
				VIPPossibleCheck.Checked = false;
				VIPNoMatterCheck.Checked = false;
			}
			if (Sender.Name == VIPNoMatterCheck.Name) {
				VIPPossibleCheck.Checked = false;
				VIPInpossibleCheck.Checked = false;
			}
		}

		private void OnBetFromChanged(BaseBeloteControl Sender, string SpriteName)
		{
			BetToUpDown.Min = BetFromUpDown.Current;
		}

		private void OnBetToChanged(BaseBeloteControl Sender, string SpriteName)
		{
			BetFromUpDown.Max = BetToUpDown.Current;
		}

		private void OnConfirmClick(BaseBeloteControl Sender, string SpriteName)
		{
			Close ();
		}
	}
}

