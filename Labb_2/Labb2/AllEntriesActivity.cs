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

using Labb2.Model;

namespace Labb2
{
    [Activity(Label = "AllEntriesActivity")]
    public class AllEntriesActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.All_Entries_Activity);
            ListView listView = FindViewById<ListView>(Resource.Id.listView);
			listView.Adapter = new ListViewAdapter(this, BookkeeperManager.Instance.GetEntries());
			listView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => 
			{

				List<Entry> list = BookkeeperManager.Instance.GetEntries();
				//var entry = listView.Adapter.GetItem(arg).JavaCast<Message>();
				//entry.ToString();
				Entry entry = list[(int)e.Id];
				Intent intent = new Intent(this, typeof(ViewEntryActivity));
				intent.PutExtra("ID",entry.Id);
				intent.PutExtra("t",e.ToString());
				StartActivity(intent);
			};
        }
    }
}