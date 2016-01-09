using System;

using CoreGraphics;
using Foundation;
using SpriteKit;
using UIKit;
using BeloteClient;

namespace BLOTONLINE
{
	public class MainMenuForm : BaseBeloteForm
	{
		private SKSpriteNode ToplineSprite;

		private BeloteButton ProfileButton;
		private BeloteLabel ProfileLabel;

		private BeloteButton MoneyButton;
		private BeloteButton ChipsButton;
		private BeloteLabel MoneyLabel;
		private BeloteLabel ChipsLabel;

		private BeloteButton StoreButton;

		private BeloteButton ExitButton;

		private BeloteLabel MenuButtonLabel;

		private BeloteButton[] MenuButtons;

		private BeloteButton RightButton;
		private BeloteButton LeftButton;

		private DaylyBonusForm DaylyBonusForm;
		private CreationTableForm CreationTableForm;

		public MainMenuForm (Game Game, BaseBeloteScene ParentScene, BeloteFormAction CloseAction, BaseBeloteForm ParentForm) : base(Game, ParentScene, CloseAction, ParentForm)
		{
			DaylyBonusForm = null;
			CreationTableForm = null;
		}

		public override void Show (float Width, float Height, float X, float Y)
		{
			base.Show (Width + 10, Height + 10, X - 5, Y - 5);

			DrawBackground ("Textures/bluebg.png", "MainMenuBackground");

			ToplineSprite = SKSpriteNode.FromTexture (SKTexture.FromImageNamed("Textures/MainMenuScreen/topbg.png"));
			ToplineSprite.Size = new CGSize(this.Width + 5, 75);
			ToplineSprite.Position = new CGPoint (this.X, this.Height - 75);
			ToplineSprite.AnchorPoint = new CGPoint (0, 0);
			ToplineSprite.ZPosition = this.BaseZPosition;
			ToplineSprite.Name = "Topline";

			ProfileButton = new BeloteButton ("ProfileButton", 65, 65, this.X + 15, this.Height - 72, OnProfileClick, null, SKTexture.FromImageNamed ("Textures/MainMenuScreen/photoback.png"), null);
			ProfileLabel = new BeloteLabel ("ProfileLabel", 90, this.Height - 45, "User Name", UIColor.White, 20, "Roboto");
			ProfileLabel.HorizontalAlignment = SKLabelHorizontalAlignmentMode.Left;

			MoneyButton = new BeloteButton ("MoneyButton", 105, 65, 300, this.Height - 70, null, null, SKTexture.FromImageNamed ("Textures/MainMenuScreen/money.png"), null);
			MoneyLabel = new BeloteLabel ("MoneyLabel", 415, this.Height - 45, "100500", UIColor.White, 20, "Roboto");

			ChipsButton = new BeloteButton ("ChipsButton", 115, 65, 500, this.Height - 70, null, null, SKTexture.FromImageNamed ("Textures/MainMenuScreen/chips.png"), null);
			ChipsLabel = new BeloteLabel ("ChipsLabel", 625, this.Height - 45, "100500", UIColor.White, 20, "Roboto");

			StoreButton = new BeloteButton ("StoreButton", 65, 65, this.Width - 150, this.Height - 72, OnStoreClick, null, SKTexture.FromImageNamed ("Textures/MainMenuScreen/store.png"), null);

			ExitButton = new BeloteButton ("ExitButton", 65, 65, this.Width - 75, this.Height - 72, OnExitClick, null, SKTexture.FromImageNamed ("Textures/MainMenuScreen/exit.png"), null);

			MenuButtons = new BeloteButton[3];
			MenuButtons [0] = new BeloteButton ("TrainingButton", 263, 277, this.Width / 2 - 175 - 200, this.Height / 2 - 139, null, null, SKTexture.FromImageNamed ("Textures/MainMenuScreen/traininglevel.png"), null);
			MenuButtons [1] = new BeloteButton ("NewTableButton", 350, 369, this.Width / 2 - 175, this.Height / 2 - 185, OnMenuButtonClick, null, SKTexture.FromImageNamed ("Textures/MainMenuScreen/createtable.png"), null);
			MenuButtons [2] = new BeloteButton ("TablesListButton", 263, 277, this.Width / 2 + 175 - 63, this.Height / 2 - 139, null, null, SKTexture.FromImageNamed ("Textures/MainMenuScreen/tables.png"), null);

			MenuButtons [0].Sprite.Alpha = 100;
			MenuButtons [2].Sprite.Alpha = 100;

			MenuButtonLabel = new BeloteLabel ("MenuButtonsLabel", Width / 2, this.Height / 7 + 60, "", UIColor.White, 40, "Roboto");

			RightButton = new BeloteButton ("Right", 131, 142, Width / 2 + 169, this.Height / 7, OnRightClick, null, SKTexture.FromImageNamed ("Textures/MainMenuScreen/right.png"), null);

			LeftButton = new BeloteButton ("Left", 131, 142, Width / 2 - 300, this.Height / 7, OnLeftClick, null, SKTexture.FromImageNamed ("Textures/MainMenuScreen/left.png"), null);

			DrawSprite (ToplineSprite);
			AddChildControl (ProfileButton);
			AddChildControl (ProfileLabel);
			AddChildControl (MoneyButton);
			AddChildControl (MoneyLabel);
			AddChildControl (ChipsButton);
			AddChildControl (ChipsLabel);
			AddChildControl (StoreButton);
			AddChildControl (ExitButton);

			AddChildControl (MenuButtons [0]);
			AddChildControl (MenuButtons [1]);
			AddChildControl (MenuButtons [2]);

			MenuButtons [0].Sprite.ZPosition += 4;
			MenuButtons [1].Sprite.ZPosition += 5;
			MenuButtons [2].Sprite.ZPosition += 4;

			AddChildControl (RightButton);
			AddChildControl (LeftButton);
			AddChildControl (MenuButtonLabel);

			UpdateMenuButtonLabel ();
		}

		public void OnProfileClick(BaseBeloteControl Sender)
		{
		}

		public void OnStoreClick(BaseBeloteControl Sender)
		{
		}

		public void OnExitClick(BaseBeloteControl Sender)
		{
			Close ();
		}

		public void OnMenuButtonClick(BaseBeloteControl Sender)
		{
			if (MenuButtons [1].Name == "TrainingButton") {
				
			}
			if (MenuButtons [1].Name == "NewTableButton") {
				CreationTableForm = new CreationTableForm (this.Game, this.ParentScene, OnCreationTableFormClose, this);
				CreationTableForm.Show (Width / 2, Height / 2, Width / 4, Height / 4);
				ParentScene.ChangeActiveForm (CreationTableForm);
			}
			if (MenuButtons [1].Name == "TablesListButton") {
			}
		}

		public void TestDaylyBonus()
		{
			DaylyBonusForm = new DaylyBonusForm (this.Game, this.ParentScene, OnDaylyBonusFormClose, this);
			DaylyBonusForm.Show (Width - (Width / 6), Height / 2, Width / 12, Height / 4);
			ParentScene.ChangeActiveForm (DaylyBonusForm);
		}

		public void OnCreationTableFormClose(BaseBeloteControl Sender)
		{
			ParentScene.ChangeActiveForm (this);
		}

		public void OnDaylyBonusFormClose(BaseBeloteControl Sender)
		{
			ParentScene.ChangeActiveForm (this);
		}

		private void UpdateMenuButtonLabel()
		{
			if (MenuButtons [1].Name == "TrainingButton")
				MenuButtonLabel.Text = "Training level";
			if (MenuButtons [1].Name == "NewTableButton")
				MenuButtonLabel.Text = "New Table";
			if (MenuButtons [1].Name == "TablesListButton")
				MenuButtonLabel.Text = "Show Tables";
		}

		public void OnLeftClick(BaseBeloteControl Sender)
		{
			SKAction _1to0 = SKAction.Sequence (SKAction.MoveTo (new CGPoint (this.Width / 2 - 175 - 200, this.Height / 2 - 139), 0.2), SKAction.ResizeTo (new CGSize (263, 277), 0.2));
			SKAction _0to2 = SKAction.MoveTo (new CGPoint(this.Width / 2 + 175 - 63, this.Height / 2 - 139), 0.2);
			SKAction _2to1 = SKAction.Sequence (SKAction.MoveTo (new CGPoint (this.Width / 2 - 175, this.Height / 2 - 185), 0.2), SKAction.ResizeTo (new CGSize (350, 369), 0.2));

			MenuButtons [1].OnTouchStart = null;

			BeloteButton [] tmp = new BeloteButton[3];
			tmp [0] = MenuButtons [1];
			tmp [1] = MenuButtons [2];
			tmp [2] = MenuButtons [0];
			MenuButtons = tmp;

			MenuButtons [2].Sprite.RunAction (_0to2);
			MenuButtons [0].Sprite.RunAction (_1to0, new Action (() => MenuButtons [0].Sprite.ZPosition -= 1));
			MenuButtons [1].Sprite.RunAction (_2to1, new Action (() => MenuButtons [1].Sprite.ZPosition += 1));

			MenuButtons [1].OnTouchStart = OnMenuButtonClick;

			UpdateMenuButtonLabel ();
		}

		public void OnRightClick(BaseBeloteControl Sender)
		{
			SKAction _1to2 = SKAction.Sequence (SKAction.MoveTo (new CGPoint (this.Width / 2 + 175 - 63, this.Height / 2 - 139), 0.2), SKAction.ResizeTo (new CGSize (263, 277), 0.2));
			SKAction _0to1 = SKAction.Sequence (SKAction.MoveTo (new CGPoint (this.Width / 2 - 175, this.Height / 2 - 185), 0.2), SKAction.ResizeTo (new CGSize (350, 369), 0.2)); 
			SKAction _2to0 = SKAction.MoveTo (new CGPoint(this.Width / 2 - 175 - 200, this.Height / 2 - 139), 0.2);

			MenuButtons [1].OnTouchStart = null;

			BeloteButton [] tmp = new BeloteButton[3];
			tmp [0] = MenuButtons [2];
			tmp [1] = MenuButtons [0];
			tmp [2] = MenuButtons [1];
			MenuButtons = tmp;

			MenuButtons [2].Sprite.RunAction (_1to2, new Action (() => MenuButtons [2].Sprite.ZPosition -= 1));
			MenuButtons [1].Sprite.RunAction (_0to1, new Action (() => MenuButtons [1].Sprite.ZPosition += 1));
			MenuButtons [0].Sprite.RunAction (_2to0);

			MenuButtons [1].OnTouchStart = OnMenuButtonClick;

			UpdateMenuButtonLabel ();
		}
	}
}

