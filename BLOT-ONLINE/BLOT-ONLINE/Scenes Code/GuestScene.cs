using System;

using CoreGraphics;
using Foundation;
using SpriteKit;
using UIKit;
using BeloteClient;

namespace BLOTONLINE
{
	public class GuestScene : BaseBeloteScene
	{
		private AuthorizationForm authForm;
		private MainMenuForm mainMenuForm;

		public GuestScene (IntPtr handle) : base (handle)
		{
		}

		public override void DidMoveToView (SKView view)
		{
			authForm = new AuthorizationForm (this.Game, this, onAutorizationFormClose, null);
			authForm.Show (1344, 750, 0, 0);
			ChangeActiveForm (authForm);
		}

		public void onAutorizationFormClose(BaseBeloteControl Sender, string SpriteName)
		{
			ChangeActiveForm (null);
			mainMenuForm = new MainMenuForm (this.Game, this, onMainMenuClose, null);
			mainMenuForm.Show (1344, 750, 0, 0);
			ChangeActiveForm (mainMenuForm);
			mainMenuForm.TestDaylyBonus ();
		}

		public void onMainMenuClose(BaseBeloteControl Sender, string SpriteName)
		{
			authForm = new AuthorizationForm (this.Game, this, onAutorizationFormClose, null);
			authForm.Show (1344, 750, 0, 0);
			ChangeActiveForm (authForm);
		}

		public override void TouchesBegan (NSSet touches, UIEvent evt)
		{
			if (ActiveForm != null)
				ActiveForm.OnTouchesBegan (touches, evt);
		}

		public override void TouchesEnded (NSSet touches, UIEvent evt)
		{
			if (ActiveForm != null)
				ActiveForm.OnTouchesEnded (touches, evt);
		}


		public override void Update (double currentTime)
		{
			// Called before each frame is rendered
		}
	}
}

