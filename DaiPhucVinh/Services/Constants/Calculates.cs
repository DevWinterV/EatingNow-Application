namespace DaiPhucVinh.Services.Constants
{
    public class Calculates
    {
        public static double ParseToBasicValue(double basicValue, double value, double quantity)
        {
            return (value / basicValue) * quantity;
        }
    }
}
