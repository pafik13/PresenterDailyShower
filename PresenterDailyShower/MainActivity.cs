using System;
using System.Threading;
using System.Globalization;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

using PresenterPlanner.Lib;
using PresenterPlanner.Lib.Doctors;
using PresenterPlanner.Lib.Hospitals;

using PresenterDailyShower.Adapters;

namespace PresenterDailyShower
{
	[Activity (Label = "План на сегодня ", MainLauncher = true, Icon = "@drawable/Icon_presents_72")]
	public class MainActivity : Activity
	{
		protected MainListAdpter adapter = null;
		protected List <Presentation> presents;
//		protected Spinner spn = null;

		protected override void OnCreate (Bundle bundle)
		{

			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			Title = Title + "(" + DateTime.Today.ToString ("D") + ")";
			Setts sett = Common.GetSettings ();

			DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
			Calendar cal = dfi.Calendar;
			int week = (cal.GetWeekOfYear (DateTime.Today, dfi.CalendarWeekRule, dfi.FirstDayOfWeek) - sett.weekOfStart) % 3;

			var choosenHospitals = (List<Hospital>)HospitalManager.GetChoosenHospitals (week, DateTime.Today.DayOfWeek);

//			var doctors = DoctorManager.GetDoctors ();
//
//			var docs = new List<string> ();
//			var chdocs = new List<Doctor> ();

//			for (int d = 0; d < doctors.Count; d++) {
//				for (int h = 0; h < choosenHospitals.Count; h++) {
//					if (doctors [d].HospitalID == choosenHospitals [h].ID) {
//						chdocs.Add (doctors [d]);
//						docs.Add(doctors [d].SecondName + ' ' + doctors [d].FirstName + ' ' + doctors [d].ThirdName);
//					}
//				}
//			}
			FindViewById <Button> (Resource.Id.btnShow).Clickable = false;
			adapter = new MainListAdpter (this, choosenHospitals);
			var lvList = FindViewById <ListView> (Resource.Id.lvList);
			lvList.Adapter = adapter; //ArrayAdapter<String> (this, Android.Resource.Layout.SimpleListItemChecked, docs);
			lvList.ChoiceMode = ChoiceMode.Single;
			lvList.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
				//presentations
				if (presents == null) {
					var lblPresents = FindViewById<TextView> (Resource.Id.PresentLabel);
					lblPresents.Visibility = ViewStates.Visible;
					var btnShow = FindViewById <Button> (Resource.Id.btnShow); 
					btnShow.Visibility = ViewStates.Visible;
					var spn = FindViewById <Spinner> (Resource.Id.spnPresents);
					spn.Visibility = ViewStates.Visible;

					string[] load = {"Загружается список презентаций..."};
					spn.Adapter = new ArrayAdapter<String> (this, Android.Resource.Layout.SimpleListItem1, load);
					new Thread(new ThreadStart(delegate {
							//LOAD METHOD TO GET ACCOUNT INFO
							RunOnUiThread(() => {

								presents = Presentations.GetPresentations ();
								List<String> presentsTitle = new List<String> ();
								for (int i = 0; i < presents.Count; i++) {
									for (int j = 0; j < presents [i].parts.Count; j++) {
										presentsTitle.Add (presents [i].name + "." + presents [i].parts [j].name);
									}
								}

								spn.Adapter = new ArrayAdapter<String> (this, Android.Resource.Layout.SimpleListItem1, presentsTitle.ToArray ());
						});
					})).Start ();
				}
				FindViewById <TextView> (Resource.Id.SecondNameLabel).Text = "Фамилия: " + adapter [e.Position].SecondName;
				FindViewById <TextView> (Resource.Id.FirstNameLabel).Text = "Имя: " + adapter [e.Position].FirstName;
				FindViewById <TextView> (Resource.Id.ThirdNameLabel).Text = "Отчество: " + adapter [e.Position].ThirdName;
				FindViewById <TextView> (Resource.Id.TelLabel).Text = "Телефон: " + adapter [e.Position].Tel;
				FindViewById <TextView> (Resource.Id.EmailLabel).Text = "E-mail: " + adapter [e.Position].Email;
				FindViewById <TextView> (Resource.Id.SpecialityLabel).Text = "Специалность: " + adapter [e.Position].Speciality;
				FindViewById <TextView> (Resource.Id.PositionLabel).Text = "Должность: " + adapter [e.Position].Position;
				FindViewById <TextView> (Resource.Id.HospitalLabel).Text = "Поликлиника: " + HospitalManager.GetHospital (adapter [e.Position].HospitalID).Name;
			};


//			var spn = FindViewById <Spinner> (Resource.Id.spnPresents);
//
//			presents = Presentations.GetPresentations ();
//			List<String> presentsTitle = new List<String> ();
//			for (int i = 0; i < presents.Count; i++) {
//				for (int j = 0; j < presents [i].parts.Count; j++) {
//					presentsTitle.Add (presents [i].name + "." + presents [i].parts [j].name);
//				}
//			}
//			FindViewById <Button> (Resource.Id.btnShow).Clickable = false;
//
//			spn.Adapter = new ArrayAdapter<String> (this, Android.Resource.Layout.SimpleListItem1, presentsTitle.ToArray ());
			FindViewById <Button> (Resource.Id.btnShow).Click += (object sender, EventArgs e) => {
				var slides = new Intent (this, typeof(PresentationView));
				int presentationID = 0;
				int partID = FindViewById <Spinner> (Resource.Id.spnPresents).SelectedItemPosition;
				for (int i = 0; (i <= presents.Count - 1) && (partID > presents [i].parts.Count - 1); i++) {
					presentationID = i + 1;
					partID = partID - presents [i].parts.Count;
				}
				slides.PutExtra ("presentationID", presentationID);
				slides.PutExtra ("partID", partID);
				slides.PutExtra ("doctorID", adapter [lvList.CheckedItemPosition].ID);
				StartActivity (slides);
			};
		}
	}
}


