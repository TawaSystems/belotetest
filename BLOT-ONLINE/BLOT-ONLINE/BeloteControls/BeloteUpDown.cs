using System;

using CoreGraphics;
using Foundation;
using SpriteKit;
using UIKit;
using BeloteClient;

namespace BLOTONLINE
{
	public class BeloteUpDown : BaseBeloteControl
	{
		private BeloteButton UpButton;
		private BeloteButton DownButton;
		private BeloteLabel UpDownLabel;

		private int maxValue;
		private int minValue;
		private int stepValue;
		private int currentValue;

		private SKTexture upActiveTexture;
		private SKTexture upUnactiveTexture;
		private SKTexture downActiveTexture;
		private SKTexture downUnactiveTexture;

		public BeloteUpDown (string Name) : base(Name)
		{
			this.Width = 200;
			this.Height = 65;
			this.upActiveTexture = SKTexture.FromImageNamed ("Textures/upactive.png");
			this.upUnactiveTexture = SKTexture.FromImageNamed ("Textures/upunactive.png");
			this.downActiveTexture = SKTexture.FromImageNamed ("Textures/downactive.png");
			this.downUnactiveTexture = SKTexture.FromImageNamed ("Textures/downunactive.png");
		}

		public BeloteUpDown(string Name, float X, float Y, int Min, int Max, int Step, int Start, BeloteFormAction onValueChanged = null) : this(Name)
		{
			this.X = X;
			this.Y = Y;
			this.minValue = Min;
			this.maxValue = Max;
			this.stepValue = Step;
			this.currentValue = Start;
			this.OnValueChanged = onValueChanged;
			ConstructControl ();
		}

		public override void ConstructControl ()
		{
			base.ConstructControl ();
			Sprite = SKSpriteNode.FromTexture (SKTexture.FromImageNamed("Textures/TextField.png"));
			((SKSpriteNode)Sprite).Size = new CGSize (this.Width, this.Height);
			((SKSpriteNode)Sprite).AnchorPoint = AnchorPoint;
			((SKSpriteNode)Sprite).Position = new CGPoint (this.X, this.Y);
			((SKSpriteNode)Sprite).ZPosition = this.Z;
			this.OnTouchStart = OnClickOn;

			UpButton = new BeloteButton (this.Name + "Up", 44, 44, 155, 10, null, null, upActiveTexture, upUnactiveTexture);
			DownButton = new BeloteButton (this.Name + "Down", 44, 26, 0, 20, null, null, downActiveTexture, downUnactiveTexture);
			UpDownLabel = new BeloteLabel (this.Name + "Label", 100, 22, currentValue.ToString(), UIColor.Black, 22, "Roboto");

			Sprite.Name = this.Name;
			Sprite.AddChild (UpDownLabel.Sprite);
			Sprite.AddChild (UpButton.Sprite);
			Sprite.AddChild (DownButton.Sprite);
			TestButtonEnabled ();
		}

		public override void Destroy ()
		{
			if (UpButton != null)
				UpButton.Destroy ();
			if (DownButton != null)
				DownButton.Destroy ();
			if (UpDownLabel != null)
				UpDownLabel.Destroy ();
			base.Destroy ();
		}

		// Выставление активности кнопок вверх-вниз
		private void TestButtonEnabled()
		{
			if ((UpButton == null) || (DownButton == null))
				return;
			UpButton.Enabled = ((currentValue + stepValue) <= maxValue);
			DownButton.Enabled = ((currentValue - stepValue) >= minValue);
		}

		private void OnClickOn(BaseBeloteControl Sender, string SpriteName)
		{
			if (SpriteName == UpButton.Name)
				Inc ();
			if (SpriteName == DownButton.Name)
				Dec ();
		}

		// Обновление надписи
		private void UpdateLabel()
		{
			if (UpDownLabel != null)
				UpDownLabel.Text = currentValue.ToString ();
		}

		// Шаг вверх
		public void Inc()
		{
			Current = currentValue + Step;
		}

		// Шаг вниз
		public void Dec()
		{
			Current = currentValue - Step;
		}

		// Минимальное значение
		public int Min
		{
			get {
				return minValue;
			}
			set {
				minValue = value;
				if (value > currentValue) {
					currentValue = value;
					TestButtonEnabled ();
				}
			}
		}

		// Максимальное значение
		public int Max
		{
			get {
				return maxValue;
			}
			set {
				maxValue = value;
				if (value < currentValue) {
					currentValue = value;
					TestButtonEnabled ();
				}
			}
		}

		// Текущее значение
		public int Current
		{
			get {
				return currentValue;
			}
			set {
				if ((value <= maxValue) && (value >= minValue)) {
					currentValue = value;
					TestButtonEnabled ();
					UpdateLabel ();
					if (OnValueChanged != null)
						OnValueChanged (this, this.Name);
				}
			}
		}

		// Шаг
		public int Step
		{
			get {
				return stepValue;
			}
			set {
				stepValue = value;
			}
		}


		public BeloteFormAction OnValueChanged
		{
			get;
			set;
		}
	}
}

