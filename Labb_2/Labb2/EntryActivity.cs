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
using AFile = Java.IO.File;
using AEnvironment = Android.OS.Environment;
using AUri = Android.Net.Uri;

namespace Labb2
{
    [Activity(Label = "EntryActivity")]
    public class EntryActivity : Activity
    {
        private RadioButton buttonIncom, buttonSpending;
        private EditText editDate, editAmounts, editDescription;
        private Spinner SpinnerType, SpinnerAccount, SpinnerVat;
        private Button buttonAdd, buttonPhoto;
        private ArrayAdapter adapterTypeIncome, adapterTypeSpending;
        private Bitmap bitmap;
        private ImageView photoView;
        private AFile bkDir, picDir, photoFile;
        private Entry newEntry;
        private AUri photoUri;
		private bool photo;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Entry_Activity);

            buttonIncom = FindViewById<RadioButton>(Resource.Id.incomeButton);
            buttonSpending = FindViewById<RadioButton>(Resource.Id.spendingButton);
            editDate = FindViewById<EditText>(Resource.Id.editDate);
            editDescription = FindViewById<EditText>(Resource.Id.editDescription);
            editAmounts = FindViewById<EditText>(Resource.Id.editAmounts);
            SpinnerType = FindViewById<Spinner>(Resource.Id.typeSpinner);
            SpinnerAccount = FindViewById<Spinner>(Resource.Id.accountSpinner);
            SpinnerVat = FindViewById<Spinner>(Resource.Id.vatSpinner);
            buttonAdd = FindViewById<Button>(Resource.Id.addEntryButton);
            buttonPhoto = FindViewById<Button>(Resource.Id.photoButton);
            photoView = FindViewById<ImageView>(Resource.Id.imageView); 

            SetSpinners();
            buttonIncom.Checked = true;
            buttonAdd.Click += AddEntry;
            buttonPhoto.Click += TakePhoto;
            buttonIncom.Click += ChangeAccounts;
            buttonSpending.Click += ChangeAccounts;

			photo = false;
        }

        private void TakePhoto(object sender, EventArgs e)
        {
            picDir = AEnvironment.GetExternalStoragePublicDirectory(AEnvironment.DirectoryPictures);
            bkDir = new AFile(picDir, "Bookkeeper");
            if (!bkDir.Exists())
            {
                bkDir.Mkdirs();
            }
            int id;
            if(BookkeeperManager.Instance.GetEntries().Count > 0)
            {
                id = BookkeeperManager.Instance.GetEntries().Select(b => b.Id).Max() + 1;
            }
            else
            {
                id = 0;
            }

            photoFile = new AFile(bkDir, "entry" + id + ".jpg");
            photoUri = AUri.FromFile(photoFile);

            Intent intent = new Intent(MediaStore.ActionImageCapture);
            intent.PutExtra(MediaStore.ExtraOutput, photoUri);
            StartActivityForResult(intent, 0);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (requestCode == 0 && resultCode == Result.Ok)
            {
                LoadAndScaleBitmap();
                photoView.SetImageBitmap(bitmap);
            }
            else
                base.OnActivityResult(requestCode, resultCode, data);
}
        private void ChangeAccounts(object sender, EventArgs e)
        {
            if (buttonIncom.Checked)
                SpinnerType.Adapter = adapterTypeIncome;
            else
                SpinnerType.Adapter = adapterTypeSpending;
        }

        private void SetSpinners()
        {
            ArrayAdapter adapterVat = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, BookkeeperManager.Instance.TaxRates);
            adapterVat.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            SpinnerVat.Adapter = adapterVat;
            
            ArrayAdapter adapterAccount = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, BookkeeperManager.Instance.MoneyAccounts);
            adapterAccount.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            SpinnerAccount.Adapter = adapterAccount;
            
            adapterTypeIncome = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, BookkeeperManager.Instance.IncomeAccounts);
            adapterTypeSpending = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, BookkeeperManager.Instance.ExpenseAccounts);
            adapterTypeIncome.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            adapterTypeSpending.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            SpinnerType.Adapter = adapterTypeIncome;
        }
        private void AddEntry(object sender, EventArgs e)
        {
            List<Account> acounts = new List<Account>();
            acounts.AddRange(BookkeeperManager.Instance.IncomeAccounts);    
            acounts.AddRange(BookkeeperManager.Instance.ExpenseAccounts);

            newEntry = new Entry() {  	Date = editDate.Text,
                                        Description = editDescription.Text,
                                        Amount = int.Parse(editAmounts.Text),
                                        AccountType = acounts.Where(a => a.ToString() == SpinnerType.SelectedItem.ToString()).ToList()[0].Number,
                                        Account = BookkeeperManager.Instance.MoneyAccounts.Where(a => a.ToString() == SpinnerAccount.SelectedItem.ToString()).ToList()[0].Number,
                                        Tax = BookkeeperManager.Instance.TaxRates.Where(v => v.ToString() == SpinnerVat.SelectedItem.ToString()).ToList()[0].Tax};
            
			if (photo) 
			{
				newEntry.Image = photoFile.Path;
			}

            BookkeeperManager.Instance.AddEntry(newEntry);
            Finish();
        }

        private void LoadAndScaleBitmap()
        {
			photo = true;
            int height = Resources.DisplayMetrics.HeightPixels;
            int width = photoView.Width;

            BitmapFactory.Options options = new BitmapFactory.Options { InJustDecodeBounds = true };
            BitmapFactory.DecodeFile(photoFile.Path, options);

            
            int outHeight = options.OutHeight;
            int outWidth = options.OutWidth;
            int inSampleSize = 1;

            if (outHeight > height || outWidth > width)
            {
                inSampleSize = outWidth > outHeight
                                   ? outHeight / height
                                   : outWidth / width;
            }

            options.InSampleSize = inSampleSize;
            options.InJustDecodeBounds = false;
            bitmap = BitmapFactory.DecodeFile(photoFile.Path, options);
        }
    }
}