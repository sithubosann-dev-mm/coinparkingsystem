using System;
using System.Globalization;
using System.Windows.Data;

namespace CoinParkingSystem.Converters
{
    public class SlotSelectionConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // values[0] = Mini Map အကွက်၏ SlotNumber (int)
            // values[1] = ညာဘက် Number Pad မှ ရိုက်ထားသော TypedSlotNumber (string)

            if (values != null && values.Length >= 2)
            {
                if (values[0] is int slotNumber && values[1] is string typedText)
                {
                    if (int.TryParse(typedText, out int typedNumber))
                    {
                        // နံပါတ်ချင်း ကိုက်ညီပါက True ပြန်ပေးမည်
                        return slotNumber == typedNumber;
                    }
                }
            }
            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}