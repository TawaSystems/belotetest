using System;
using CoreGraphics;
using Foundation;
using SpriteKit;
using UIKit;
using BeloteClient;

namespace BLOTONLINE
{
	public class BaseBeloteScene : SKScene
	{
		public BaseBeloteScene (IntPtr handle) : base (handle)
		{
		}

		public virtual void Initialize(Game Game)
		{
			ActiveForm = null;
			AnchorPoint = new CGPoint (0, 0);
			this.Game = Game;
		}

		public void ChangeActiveForm(BaseBeloteForm NewActiveForm)
		{
			ActiveForm = NewActiveForm;	
		}

		public Game Game {
			get;
			set;
		}

		public BaseBeloteForm ActiveForm {
			get;
			private set;
		}
	}
}

