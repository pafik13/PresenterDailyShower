using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace PresenterDailyShower.Lib
{
	public class Demonstration
	{
		public int doctorID;
		public List <Demo> demos;

		public Demonstration()
		{
			demos = new List <Demo> ();
		}

		public Demo GetDemo (string key)
		{
			for (int d = 0; d < demos.Count; d++) {
				if (demos[d].slideKey == key)
					return demos[d];
			}
			return new Demo() {slideKey = key, shows = new List<Show>()};
		}

		public int SaveDemo (Demo item)
		{
			Demo i = demos.Find (x => x.slideKey == item.slideKey);
			if (i == null) {
				demos.Add (item);
			} else {
				i = item; // replaces item in collection with updated value
			}
			return 0;
		}
	}

	public class Demo
	{
		public string slideKey;
		public List <Show> shows;
	}

	public class Show
	{
		public DateTime date;
		public int number;
		public double time;
		public Coord coord;
	}

	public struct Coord
	{
		public double longtitude;
		public double latitude;
	}
}

