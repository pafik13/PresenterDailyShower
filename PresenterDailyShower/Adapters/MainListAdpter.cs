using System;
using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using Android.Content;
using PresenterPlanner.Lib.Doctors;
using PresenterPlanner.Lib.Hospitals;

namespace PresenterDailyShower.Adapters
{
	public class MainListAdpter: BaseAdapter<Doctor> {
		Activity context = null;
		Dictionary<string, List<Doctor>> samples;
		private readonly IList<object> rows;
	

		public MainListAdpter(Activity context, List<Hospital> chHospitals): base() {
			this.context = context;
			samples = new Dictionary<string, List<Doctor>>();

			for (int h = 0; h < chHospitals.Count; h++) {
				var chDoctors = (List<Doctor>)DoctorManager.GetDoctors (chHospitals [h].ID);
				chDoctors.Sort (DoctorManager.DoctorCompare);
				samples.Add (chHospitals [h].Name, chDoctors);
			}

			// flatten groups into single 'list'
			rows = new List<object>();
			foreach (var section in samples.Keys) {
				rows.Add(section);
				foreach (var session in samples[section]) {
					rows.Add(session);
				}
			}
		}

		public override Doctor this[int position]
		{
			get
			{ // this'll break if called with a 'header' position
				return (Doctor)rows[position];
			}
		}

		public override long GetItemId(int position)
		{
			return position;
		}

		public override int Count
		{
			get { return rows.Count; }
		}
		public override bool AreAllItemsEnabled()
		{
			return true;
		}
		public override bool IsEnabled(int position)
		{
			return !(rows[position] is string);
		}

		/// <summary>
		/// Grouped list: view could be a 'section heading' or a 'data row'
		/// </summary>
		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			// Get our object for this position
			var item = this.rows[position];
			View view = null;

			if (item is string) {   // header
				view = context.LayoutInflater.Inflate(Resource.Layout.MainListHeader, null);
				view.Clickable = false;
				view.LongClickable = false;
				view.SetOnClickListener(null);
				view.FindViewById<TextView>(Resource.Id.txtHospital).Text = (string)item;
			} else {   //session
				view = context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItemChecked, null);
				view.LongClickable = false;
				Doctor doc = (Doctor)item;
				view.FindViewById<TextView> (Android.Resource.Id.Text1).Text = doc.FIO (); //SecondName + ' ' + doc.FirstName + ' ' + doc.ThirdName;
			}
			//Finally return the view
			return view;
		}
	}
}

