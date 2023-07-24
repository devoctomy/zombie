namespace Zombie.Api.Exceptions
{
    public class ZombieApiException : Exception
    {
        public ZombieApiException(string message)
            : base(message)
        {}

        public ZombieApiException(
            string message,
            Exception innerException)
            : base(message, innerException)
        {}
    }
}
