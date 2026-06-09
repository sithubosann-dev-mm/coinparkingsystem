/*using System; 
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
*/

/*

using System;
using System.IO;

namespace CoinParkingSystem.Services
{
    public class ReceiptService
    {
        private readonly string customerFolder;
        private readonly string ownerFolder;

        public ReceiptService()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;

            customerFolder = Path.Combine(
                baseDir,
                "Data",
                "Customer_Receipt");

            ownerFolder = Path.Combine(
                baseDir,
                "Data",
                "Owner_Receipt");

            Directory.CreateDirectory(customerFolder);
            Directory.CreateDirectory(ownerFolder);
        }

        public void GenerateCustomerReceipt(
            int slotNumber,
            decimal amount)
        {
            string fileName =
                $"Receipt_{DateTime.Now:yyyyMMdd_HHmmss}.txt";

            string filePath =
                Path.Combine(customerFolder, fileName);

            string text =
$@"==================================
      Coin Parking Receipt
==================================

Slot Number : {slotNumber}

Payment Time : {DateTime.Now}

Amount : {amount} 円

==================================
Thank You
==================================";

            File.WriteAllText(filePath, text);
        }

        public void SaveDailyIncome(
            int slotNumber,
            decimal amount)
        {
            string filePath =
                Path.Combine(
                    ownerFolder,
                    "daily_income_report.txt");

            string record =
                $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}," +
                $"Slot:{slotNumber}," +
                $"Amount:{amount}円";

            File.AppendAllText(
                filePath,
                record + Environment.NewLine);
        }
    }
}
*/
using System;
using System.IO;
using System.Text;

namespace CoinParkingSystem.Services
{
    public class ReceiptService
    {
        private readonly string customerFolder;
        private readonly string ownerFolder;

        public ReceiptService()
        {
          
            string binDir = AppDomain.CurrentDomain.BaseDirectory;

          
            customerFolder = Path.Combine(binDir, "Data", "Customer_Receipt");
            ownerFolder = Path.Combine(binDir, "Data", "Owner_Receipt");

          
            if (!Directory.Exists(customerFolder)) Directory.CreateDirectory(customerFolder);
            if (!Directory.Exists(ownerFolder)) Directory.CreateDirectory(ownerFolder);
        }

        public void GenerateCustomerReceipt(int slotNumber, decimal amount)
        {
           
            string fileName = $"Receipt_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
            string filePath = Path.Combine(customerFolder, fileName);

            string text =
$@"==================================
      Coin Parking Receipt
==================================

Slot Number  : No. {slotNumber}
Payment Time : {DateTime.Now:yyyy/MM/dd HH:mm:ss}
Amount       : {amount:N0} 円

==================================
            Thank You
==================================";

          
            File.WriteAllText(filePath, text, Encoding.UTF8);
        }

        public void SaveDailyIncome(int slotNumber, decimal amount)
        {
            string filePath = Path.Combine(ownerFolder, "daily_income_report.txt");

            string record = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss},Slot:{slotNumber},Amount:{amount:N0}円";

          
            File.AppendAllText(filePath, record + Environment.NewLine, Encoding.UTF8);
        }
    }
}