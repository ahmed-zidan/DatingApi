namespace Core.Extensions
{
    public static class DateTimeExtensions
    {
        public static int getAge(this DateTime value) { 
            DateTime now = DateTime.Now;
            var age = now.Year - value.Year;
            return age;
        }
    }
}
