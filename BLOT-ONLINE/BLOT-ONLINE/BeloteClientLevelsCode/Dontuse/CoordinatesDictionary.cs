using System;
using System.Collections.Generic;
using BeloteClient;
using CoreGraphics;

namespace BLOTONLINE
{
	public class CoordinatesDictionary
	{
		private Dictionary<string, Dictionary<DeviceType, CGPoint>> dict;

		public CoordinatesDictionary ()
		{
			dict = new Dictionary<string, Dictionary<DeviceType, CGPoint>> ();
		}

		public void AddValue(string Key, DeviceType Device, CGPoint Point)
		{
			if (dict.ContainsKey (Key)) {
				dict [Key].Add (Device, Point);
			} else {
				dict.Add (Key, new Dictionary<DeviceType, CGPoint> ());
				AddValue (Key, Device, Point);
			}
		}

		public CGPoint GetValue(string Key, DeviceType Device)
		{
			return dict [Key] [Device];
		}
	}
}

