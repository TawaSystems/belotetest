using System;
using UIKit;

namespace BeloteClient
{
	public enum DeviceType
	{
		IPhone_4S = 1, 
		IPhone_5_5S = 2, 
		IPhone_6_6S = 3, 
		IPhone_6P_6SP = 4, 
		IPad = 5
	}
			
	public class GraphicsProvider
	{
		public GraphicsProvider()
		{
		}

		private DeviceType GetDeviceType()
		{
			int h = (int)UIScreen.MainScreen.Bounds.Height;
			int w = (int)UIScreen.MainScreen.Bounds.Width;
			if (h == 320) {
				return (w == 480) ? DeviceType.IPhone_4S : DeviceType.IPhone_5_5S;
			} else if (h == 375)
				return DeviceType.IPhone_6_6S;
			else if (h == 414)
				return DeviceType.IPhone_6P_6SP;
			else
				return DeviceType.IPad;
		}

		public string Path 
		{
			get 
			{
				switch (GetDeviceType ()) {
					case DeviceType.IPhone_4S:
						return "InterfaceImages/4S/";
					case DeviceType.IPhone_5_5S:
					case DeviceType.IPhone_6_6S:
						return "InterfaceImages/5_6/";
					case DeviceType.IPhone_6P_6SP:
						return "InterfaceImages/6P/";
					case DeviceType.IPad:
						return "InterfaceImages/IPad/";
					default:
						return "";
				}
			}
		}

		public DeviceType DeviceType {
			get {
				return GetDeviceType ();
			}
		}

	}
}

