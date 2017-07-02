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

using SQLite;

namespace Labb2.Model
{
    public class Entry
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; private set; }
        public int Account { get; set; }
        public int AccountType { get; set; }
        public double Tax { get; set; }
        public string Date { get; set; }
        public string Description { get; set; }
        public int Amount { get; set; }
		public string Image { get; set; }

    }
}