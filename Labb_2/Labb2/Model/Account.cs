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
    class Account
    {
        [PrimaryKey]
        public int Number { get; set; }
        public string Name { get; set; }

        //AccountType => 1 = income, 2 = Expense, 3 = Money 
        public int AccountType { get; set; }

        public override string ToString()
        {
            return this.Name + " (" + this.Number + ")";
        }
    }
}