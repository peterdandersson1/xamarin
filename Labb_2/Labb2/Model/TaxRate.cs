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
    class TaxRate
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public double Tax { get; set; }

        public override string ToString()
        {
            return Tax * 100 + "%";
        }
    }
}