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
	public class DemonstrationManager
	{
		static DemonstrationManager ()
		{
		}

		public static Demonstration GetDemonstration(int docID)
		{
			return DemonstrationRepository.GetDemonstration(docID);
		}

		public static int SaveDemonstration (Demonstration item)
		{
			return DemonstrationRepository.SaveDemonstration(item);
		}
	}
}

