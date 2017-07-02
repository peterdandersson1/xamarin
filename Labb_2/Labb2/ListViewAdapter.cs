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
    class ListViewAdapter : BaseAdapter
    {
        private Activity activity;
        private List<Entry> entries;

        public ListViewAdapter(Activity activity, List<Entry> entries)
        {
            this.activity = activity;
            this.entries = entries;
        }
        public override int Count
        {
            get { return entries.Count; }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {

            View view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.List_Item, parent, false);
			string inOut = "";
			if (BookkeeperManager.Instance.GetAccount (entries[position].AccountType).AccountType == 1)
				inOut = "+";
			else if (BookkeeperManager.Instance.GetAccount (entries[position].AccountType).AccountType == 2) 
				inOut = "-";

            view.FindViewById<TextView>(Resource.Id.itemDate).Text = entries[position].Date;
            view.FindViewById<TextView>(Resource.Id.itemDescription).Text = entries[position].Description;
			view.FindViewById<TextView>(Resource.Id.itemSum).Text = inOut + entries[position].Amount.ToString() + ":-";

            return view;
        }
    }
}