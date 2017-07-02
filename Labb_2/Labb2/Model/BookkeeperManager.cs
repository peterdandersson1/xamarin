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
    class BookkeeperManager
    {
        private string dbPath;
        private static BookkeeperManager instance;
        public List<Account> IncomeAccounts 
        { 
            get
            {
                SQLiteConnection db = new SQLiteConnection(dbPath);
                List<Account> dbList = db.Table<Account>().Where(a => a.AccountType == 1).ToList();
                db.Close();
                return dbList; 
            }
        }
        public List<Account> ExpenseAccounts
        {
            get
            {
                SQLiteConnection db = new SQLiteConnection(dbPath);
                List<Account> dbList = db.Table<Account>().Where(a => a.AccountType == 2).ToList();
                db.Close();
                return dbList;
            }
        }
        public List<Account> MoneyAccounts
        {
            get
            {
                SQLiteConnection db = new SQLiteConnection(dbPath);
                List<Account> dbList = db.Table<Account>().Where(a => a.AccountType == 3).ToList();
                db.Close();
                return dbList;
            }
        }
        public List<TaxRate> TaxRates
        {
            get
            {
                SQLiteConnection db = new SQLiteConnection(dbPath);
                List<TaxRate> dbList = db.Table<TaxRate>().ToList();
                db.Close();
                return dbList;
            }
        }
        public List<Entry> Entries { get; private set; }
        public static BookkeeperManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BookkeeperManager();
                }
                return instance;
            }
        }
        public BookkeeperManager()
        {
            dbPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            dbPath += "\\database.db";
            SQLiteConnection db = new SQLiteConnection(dbPath);
            db.CreateTable<Entry>();
            db.CreateTable<Account>();
            db.CreateTable<TaxRate>();
            if (IncomeAccounts.Count == 0)
                CreateAccounts();
            db.Close();
 
        }

        private void CreateAccounts()
        {
            List<Account> NewAccounts = new List<Account>() {   new Account() { Name = "Försäljning", Number = 3000, AccountType = 1 },
                                                                new Account() { Name = "Försäljning av tjänst", Number = 3040, AccountType = 1 },
                                                                new Account() { Name = "Övriga egna utag", Number = 2013, AccountType = 2 },
                                                                new Account() { Name = "Förbrukningsinventarier och förbrukningsmaterial", Number = 0000, AccountType = 2 }, 
                                                                new Account() { Name = "Reklam och PR", Number = 5900, AccountType = 2 },
                                                                new Account() { Name = "Kassa", Number = 1910, AccountType = 3 },
                                                                new Account() { Name = "Företagskonto", Number = 1930, AccountType = 3 }, 
                                                                new Account() { Name = "Egna insättningar", Number = 2018, AccountType = 3 } };
            
            List<TaxRate> NewTaxRates = new List<TaxRate>() {   new TaxRate() { Tax = 0.06 }, 
                                                                new TaxRate() { Tax = 0.12 },
                                                                new TaxRate() { Tax = 0.25 } };
            
            foreach (Account a in NewAccounts) { AddAccount(a); }
            foreach (TaxRate t in NewTaxRates) { AddTaxRate(t); }

        }
        public void AddEntry(Entry e)
        {
            SQLiteConnection db = new SQLiteConnection(dbPath);
            db.Insert(e);
            db.Close();
        }
        public void AddAccount(Account e)
        {
            SQLiteConnection db = new SQLiteConnection(dbPath);
            db.Insert(e);
            db.Close();
        }
        public void AddTaxRate(TaxRate e)
        {
            SQLiteConnection db = new SQLiteConnection(dbPath);
            db.Insert(e);
            db.Close();
        }
        public List<Entry> GetEntries()
        {
            SQLiteConnection db = new SQLiteConnection(dbPath);
            IEnumerable<Entry> dbList = db.Table<Entry>();
            Entries = dbList.OrderBy(e => e.Id).ToList();
            db.Close();
            return Entries;
        }

		public Entry GetEntry(int id)
		{
			SQLiteConnection db = new SQLiteConnection(dbPath);
			IEnumerable<Entry> dbList = db.Table<Entry>();
			Entry entryId = dbList.Where (e => e.Id == id).Max ();
			db.Close();
			return entryId;
		}

		public Account GetAccount(int number){
			SQLiteConnection db = new SQLiteConnection(dbPath);
			Account theAccount = db.Table<Account>().Where(a => a.Number == number).Max();
			db.Close();
			return theAccount; 
		}
		public TaxRate GetTaxRate(int id)
		{
			SQLiteConnection db = new SQLiteConnection(dbPath);
			TaxRate tax = db.Table<TaxRate>().Max();
			db.Close();
			return tax;
		}

    }
}