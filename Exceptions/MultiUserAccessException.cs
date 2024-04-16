namespace ShopBase
{
    public class MultiUserAccessException : Exception
    {
        public MultiUserAccessException(string message) : base(message)
        { }

        public MultiUserAccessException()
        { }
    }
}
