
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
using Android.Provider;
using Android.Graphics;

namespace Labb2
{
	[Activity (Label = "ViewEntryActivity")]			
	public class ViewEntryActivity : Activity
	{
		private TextView textView1, textView2, textView3, textView4, textView5;
		private ImageView image;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView(Resource.Layout.View_Entry_Activity);

			textView1 = FindViewById<TextView> (Resource.Id.textView1);
			textView2 = FindViewById<TextView> (Resource.Id.textView2);
			textView3 = FindViewById<TextView> (Resource.Id.textView3);
			textView4 = FindViewById<TextView> (Resource.Id.textView4);
			textView5 = FindViewById<TextView> (Resource.Id.textView5);
			image = FindViewById<ImageView> (Resource.Id.imageView1);

			int id = Intent.GetIntExtra ("ID", 666);

			Entry entry = BookkeeperManager.Instance.GetEntry (id);
			string inOut = "";
			if (BookkeeperManager.Instance.GetAccount (entry.AccountType).AccountType == 1)
				inOut = ":-   " + GetString(Resource.String.to) + "   ";
			else if (BookkeeperManager.Instance.GetAccount (entry.AccountType).AccountType == 2)
				inOut = ":-   " + GetString(Resource.String.from) + "   ";

			textView1.Text = entry.Date;
			textView2.Text = entry.Amount + inOut + BookkeeperManager.Instance.GetAccount (entry.Account).ToString ();
			textView3.Text = GetString(Resource.String.vat) + "  "+ (entry.Tax * 100) + "%";
			textView4.Text = entry.Description;
			textView5.Text = BookkeeperManager.Instance.GetAccount (entry.AccountType).ToString ();
			image.SetImageBitmap(LoadAndScaleBitmap(entry.Image));
		}

		private Bitmap LoadAndScaleBitmap(string path)
		{
			int width = image.Width;

			BitmapFactory.Options options = new BitmapFactory.Options { InJustDecodeBounds = true };
			BitmapFactory.DecodeFile(path, options);

			int outWidth = options.OutWidth;
			int inSampleSize = outWidth < width 
								? outWidth / width
								: 1;

			options.InSampleSize = inSampleSize;
			options.InJustDecodeBounds = false;
			return BitmapFactory.DecodeFile(path, options);
		}
	}
}

