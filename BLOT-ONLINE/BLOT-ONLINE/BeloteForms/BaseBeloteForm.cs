using System;
using System.Collections.Generic;

using CoreGraphics;
using Foundation;
using SpriteKit;
using UIKit;
using BeloteClient;

namespace BLOTONLINE
{
	// Тип действия, выполняемый при закрытии формы
	public delegate void BeloteFormAction(BaseBeloteControl Sender);

	public class BaseBeloteForm
	{
		// Ссылка на игровой объект
		protected Game Game;
		// Ссылка на родительскую сцену
		protected BaseBeloteScene ParentScene;
		// Ссылка на действие по закрытию формы
		protected BeloteFormAction CloseAction;
		// Список спрайтов на форме
		private List<BaseBeloteControl> Controls; 
		// Спрайт фона
		protected SKSpriteNode BackgroundSprite;

		// Конструтор: игровой объект, родительская сцена, событие завершения, родительская форма
		public BaseBeloteForm (Game Game, BaseBeloteScene ParentScene, BeloteFormAction CloseAction, BaseBeloteForm ParentForm)
		{
			this.Game = Game;
			this.ParentScene = ParentScene;
			this.CloseAction = CloseAction;
			this.Parent = ParentForm;
			if (Parent != null)
				BaseZPosition = Parent.BaseZPosition + 10;
			else
				BaseZPosition = 0;
			Controls = new List<BaseBeloteControl> ();
		}

		// Добавление контрола на форму
		public void AddChildControl(BaseBeloteControl Child)
		{
			Controls.Add (Child);
			Child.Parent = this;
			DrawSprite (Child.Sprite);
			Child.ShowView (ParentScene);
		}

		// Удаление контрола с формы по имени
		public void RemoveChildControl(string Name)
		{
			BaseBeloteControl n = Controls.Find(node => node.Name == Name);
			RemoveChildControl (n);
		}

		// Удаление контрола с формы по ссылке
		public void RemoveChildControl(BaseBeloteControl Control)
		{
			if (Control != null)
			{
				Controls.Remove(Control);
				Control.Destroy ();
			}
		}

		protected void DrawBackground(string BackGroundTexture, string BackName)
		{
			if (ParentScene == null)
				return;

			SKTexture backTexture = SKTexture.FromImageNamed (BackGroundTexture);
			BackgroundSprite = SKSpriteNode.FromTexture (backTexture);
			BackgroundSprite.Size = new CGSize(Width, Height);
			BackgroundSprite.Position = new CGPoint (X, Y);
			BackgroundSprite.AnchorPoint = new CGPoint (0, 0);
			BackgroundSprite.ZPosition = this.BaseZPosition;
			BackgroundSprite.Name = BackName;
			ParentScene.AddChild (BackgroundSprite);
		}

		// Рисует на сцене спрайт
		protected void DrawSprite(SKNode DrawingSprite)
		{
			if (BackgroundSprite != null)
				BackgroundSprite.AddChild (DrawingSprite);
			else
				ParentScene.AddChild (DrawingSprite);
		}

		// Метод для перегрузки - показ формы
		// Показ всплывающей формы с заданными параметрами размера
		public virtual void Show(float Width, float Height, float X, float Y)
		{
			this.Width = Width;
			this.Height = Height;
			this.X = X;
			this.Y = Y;
		}

		// Методы обработки начала и конца тапа, для обработки нажатий
		public virtual void OnTouchesBegan (NSSet touches, UIEvent evt)
		{
			NSObject touch = touches.AnyObject;
			CGPoint location = ((UITouch)touch).LocationInNode (ParentScene);
			SKNode node = this.ParentScene.GetNodeAtPoint (location);
			foreach (BaseBeloteControl control in Controls) {
				if (node.Name.Contains(control.Name)) {
					if (control.Enabled)
						control.TouchStart ();
					break;
				}
			}
		}

		public virtual void OnTouchesEnded (NSSet touches, UIEvent evt)
		{
			NSObject touch = touches.AnyObject;
			CGPoint location = ((UITouch)touch).LocationInNode (ParentScene);
			SKNode node = this.ParentScene.GetNodeAtPoint (location);
			foreach (BaseBeloteControl control in Controls) {
				if (node.Name.Contains(control.Name)) {
					if (control.Enabled)
						control.TouchEnd ();
					break;
				}
			}
		}

		// Метод закрытия формы и удаления контролов
		public void Close()
		{
			foreach (BaseBeloteControl control in Controls) {
				control.Destroy ();
			}
			Controls.Clear ();
			if (CloseAction != null)
				CloseAction (null);
			if (BackgroundSprite != null)
				BackgroundSprite.RemoveFromParent ();
		}

		// Родительский спрайт
		public BaseBeloteForm Parent {
			get;
			private set;
		}

		// Базовая позиция по оси Z для этой формы
		public int BaseZPosition {
			get;
			private set;
		}


		public float Width
		{
			get;
			protected set;
		}

		public float Height
		{
			get;
			protected set;
		}

		public float X
		{
			get;
			protected set;
		}

		public float Y
		{
			get;
			protected set;
		}
	}
}

