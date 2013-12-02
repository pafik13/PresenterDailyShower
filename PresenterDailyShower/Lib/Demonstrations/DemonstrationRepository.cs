using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PresenterPlanner.Lib;

namespace PresenterDailyShower.Lib
{
	public class DemonstrationRepository
	{
		static string storeLocation;	
		static List<Demonstration> demonstrations;

		static DemonstrationRepository ()
		{
			// set the db location
			storeLocation = DatabaseFilePath;
			demonstrations = new List<Demonstration> ();

			// deserialize XML from file at dbLocation
			ReadXml ();
		}

		static void ReadXml ()
		{
			if (File.Exists (storeLocation)) {
				var serializer = new XmlSerializer (typeof(List<Demonstration>));
				using (var stream = new FileStream (storeLocation, FileMode.Open)) {
					demonstrations = (List<Demonstration>)serializer.Deserialize (stream);
				}
			}
		}

		static void WriteXml ()
		{
			var serializer = new XmlSerializer (typeof(List<Demonstration>));
			using (var writer = new StreamWriter (storeLocation)) {
				serializer.Serialize (writer, demonstrations);
			}
		}

		public static string DatabaseFilePath {
			get { 		
				return Path.Combine (Common.DatabaseFileDir(), "Demonstrations.xml");		
			}
		}

		public static Demonstration GetDemonstration(int docID)
		{
			for (int d = 0; d < demonstrations.Count; d++) {
				if (demonstrations[d].doctorID == docID)
					return demonstrations[d];
			}
			return new Demonstration() {doctorID = docID, demos = new List<Demo>()};
		}

		public static int SaveDemonstration (Demonstration item)
		{ 
			var i = demonstrations.Find (x => x.doctorID == item.doctorID);
			if ( i == null ) {
				demonstrations.Add (item);
			} else {
				i = item; // replaces item in collection with updated value
			}

			WriteXml ();
			return item.doctorID;
		}
	}
}

