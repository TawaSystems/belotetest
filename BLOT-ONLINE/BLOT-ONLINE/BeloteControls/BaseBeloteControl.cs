using System;

using CoreGraphics;
using Foundation;
using SpriteKit;
using UIKit;
using BeloteClient;

namespace BLOTONLINE
{
	//
	public class BaseBeloteControl
	{
		// Поле со ссылкой на родительскую форму
		protected BaseBeloteForm parent;
		// Конструктор по имени
		public BaseBeloteControl (String Name)
		{
			AnchorPoint = new CGPoint (0, 0);
			parent = null;
			Sprite = null;
			Enabled = true;
			this.Name = Name;
		}
			
		// Метод для переопределения: в нем должен конструироваться контрол
		public virtual void ConstructControl()
		{
		}

		// Уничтожение контрола
		public virtual void Destroy()
		{
			this.Parent = null;
			if (Sprite != null)
				this.Sprite.RemoveFromParent ();
		}

		// Показ компонентов отображение
		public virtual void ShowView(BaseBeloteScene Scene)
		{
		}

		// функция для выполнения каких-либо системных действий компонента во время клика
		protected virtual void SystemClick(string SpriteName)
		{
		}

		// Клик по контролу
		public void TouchStart(string SpriteName)
		{
			if (OnTouchStart != null)
				OnTouchStart (this, SpriteName);
			SystemClick (SpriteName);
		}

		public void TouchEnd(string SpriteName)
		{
			if (OnTouchEnd != null)
				OnTouchEnd (this, SpriteName);
		}

		// Событие по клику
		public BeloteFormAction OnTouchStart {
			get;
			set;
		}

		public BeloteFormAction OnTouchEnd {
			get;
			set;
		}

		// Точка привязки, по умолчанию (0, 0)
		public CGPoint AnchorPoint {
			get;
			set;
		}

		// Высота
		public float Height
		{
			get;
			set;
		}

		// Ширина
		public float Width
		{
			get;
			set;
		}

		// Позиция по различным осям
		public float X {
			get;
			set;
		}

		public float Y {
			get;
			set;
		}

		public float Z {
			get;
			set;
		}

		// Свойство-родитель. При изменении меняется параметр Z
		public BaseBeloteForm Parent
		{
			get {
				return parent;
			}
			set {
				parent = value;
				if (value != null)
					this.Z += value.BaseZPosition;
			}
		}

		// Спрайт
		public SKNode Sprite
		{
			get;
			set;
		}

		// Имя компонента
		public string Name
		{
			get;
			private set;
		}

		// Включенность компонента
		public virtual bool Enabled {
			get;
			set;
		}
	}
}

