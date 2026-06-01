using System.Windows.Controls;

namespace CoinParkingSystem.Views
{
    public partial class EntryView : UserControl
    {
        public EntryView()
        {
            InitializeComponent();
        }

        // 🎯 စာလုံး ၂ လုံးပြည့်တာနဲ့ TxtCarNoPart2 ဆီသို့ Cursor အလိုအလျောက် ရွှေ့ပေးမည့် Event
        private void CarNoPart1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Text.Length == 2)
            {
                TxtCarNoPart2.Focus();
            }
        }
    }
}