using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Labb2.Model;

namespace Labb2
{
    [Activity(Label = "Labb2", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            BookkeeperManager bk = BookkeeperManager.Instance;

            Button buttonNewEntry = FindViewById<Button>(Resource.Id.NewEntryButton);
            Button buttonAllEntries = FindViewById<Button>(Resource.Id.AllEntriesButton);
            Button buttonReport = FindViewById<Button>(Resource.Id.ReportButton);

            Intent newEntryIntent = new Intent(this, typeof(EntryActivity));
            Intent allEntriesIntent = new Intent(this, typeof(AllEntriesActivity));
            Intent reportIntent = new Intent(this, typeof(ReportActivity));

            buttonNewEntry.Click += delegate 
            {
                StartActivity(newEntryIntent);
            };
            buttonAllEntries.Click += delegate
            {
                StartActivity(allEntriesIntent);
            };
            buttonReport.Click += delegate
            {
                StartActivity(reportIntent);
            };
        }
    }
}

