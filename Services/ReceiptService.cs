using System; 
using System.IO;

namespace CoinParkingSystem.Services
{  
    public class ReceiptService
    {
        // Receipt can view in folder path in ur bin file >>bin/Debug/Data/....

        // the folder path for customer receipt,owner .diki
        private string customerReceipt= Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data/Customer_Receipt/receipt_history.txt");
        private string dailyOwnerReport = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data/Owner_Receipt/daily_income_report.txt");

        // if the folder does not exit.diki
        public ReceiptService()
        {
            if (!Directory.Exists("Data/Customer_Receipt"))
                Directory.CreateDirectory("Data/Customer_Receipt");

            if (!Directory.Exists("Data/Owner_Receipt"))
                Directory.CreateDirectory("Data/Owner_Receipt");
        }

        // Customer Receipt.diki
        public void GenerateCustomerReceipt(string plateNumber, decimal amount)
        {
            string content = $"--- Receipt ---\nPlate: {plateNumber}\nAmount: ¥{amount}\nTime: {DateTime.Now}\n\n";
            File.AppendAllText(customerReceipt, content);
        }

        // Owner Report.diki
        public void SaveDailyIncome(decimal amount, int parkingFeeValue)
        {
            string reportLine = $"{DateTime.Now.ToShortDateString()} | Income: ¥{amount}\n";
            File.AppendAllText(dailyOwnerReport, reportLine);
        }

        internal void GenerateCustomerReceipt(int slotNumber, int parkingFeeValue)
        {
            throw new NotImplementedException();
        }
    }


    
}