namespace TBC.ROPP.Shared
{
    public abstract class SystemDate
    {
        public static DateTimeOffset Now
        {
            get
            {
                var date = DateTimeOffset.Now.ToUniversalTime();
                return date;
            }
        }
    }
}