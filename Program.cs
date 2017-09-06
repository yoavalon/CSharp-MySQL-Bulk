using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace MySqlTest
{
    class Program
    {
        internal static void BulkInsert(List<TnuotDetails> TnuotItems)
        {            
            string ConnectionString = "Server=faasdbtst01.c20mfaqf3eyj.eu-west-1.rds.amazonaws.com; Port=1433; Database=loans_dev; Uid=loans_dev; Pwd=O6fFYdfBRtULmZ3VnJuo!&%;";
            using (MySqlConnection mConnection = new MySqlConnection(ConnectionString))
            {

                int NumTnuot = TnuotItems.Count();
                int Limit = 20000;
                int Steps = NumTnuot / Limit + 1;

                mConnection.Open();

                for (int i = 0; i < Steps; i++)
                {
                    StringBuilder sCommand = new StringBuilder("INSERT INTO TnuotItems (Status, TaxPercent, BankNumber, BranchNumber, AccountNumber, SalarySystem) values ");

                    List<string> Rows = new List<string>();
                    foreach (var item in TnuotItems.Skip(i * Limit).Take(Limit))
                    {
                        Rows.Add($" ('{item.Status.PadLeft(2, '0')}', {item.TaxPercent}, '{item.BankNumber.PadLeft(3, '0')}', '{item.BranchNumber.PadLeft(3, '0')}', '{item.AccountNumber.PadLeft(9, '0')}', {item.SalarySystem})");
                    }
                    sCommand.Append(string.Join(",", Rows));
                    sCommand.Append(";");

                    using (MySqlCommand myCmd = new MySqlCommand(sCommand.ToString(), mConnection))
                    {
                        myCmd.CommandType = CommandType.Text;
                        myCmd.ExecuteNonQuery();
                    }
                }
            }
        }


        static void Main(string[] args)
        {
            
            List<TnuotDetails> TnuotItems = new List<TnuotDetails>();

            for (int i = 0; i < 750000; i++)
            {
                TnuotItems.Add(new TnuotDetails() { AccountNumber = "329123", BankNumber = "10", BranchNumber = "972", SalarySystem = 34234, Status = "99", TaxPercent = 4.3 });
            }

            BulkInsert(TnuotItems);

        }
    }

    public class TnuotDetails
    {
        private string _Status;
        public string Status
        {
            get { return _Status; }
            set { _Status = value; }
        }

        private double _TaxPercent;
        public double TaxPercent
        {
            get { return _TaxPercent; }
            set { _TaxPercent = value; }
        }

        private string _BankNumber;
        public string BankNumber
        {
            get { return _BankNumber; }
            set { _BankNumber = value; }
        }

        private string _BranchNumber;
        public string BranchNumber
        {
            get { return _BranchNumber; }
            set { _BranchNumber = value; }
        }

        private string _AccountNumber;
        public string AccountNumber
        {
            get { return _AccountNumber; }
            set { _AccountNumber = value; }
        }

        private double _SalarySystem;
        public double SalarySystem
        {
            get { return _SalarySystem; }
            set { _SalarySystem = value; }
        }
    }
}
