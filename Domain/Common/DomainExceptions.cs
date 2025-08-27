namespace ClientDirectory.Domain.Common
{
    /// <summary>
    /// Exception thrown when a client is not found.
    /// </summary>
    public class ClientNotFoundException(string message) : Exception(message);

    /// <summary>
    /// Exception thrown when an account is not found.
    /// </summary>
    public class AccountNotFoundException(string message) : Exception(message);

    /// <summary>
    /// Exception thrown when there are insufficient funds.
    /// </summary>
    public class InsufficientFundsException(string message) : Exception(message);

    /// <summary>
    /// Exception thrown when daily limit is exceeded.
    /// </summary>
    public class DailyLimitExceededException(string message) : Exception(message);

    /// <summary>
    /// Exception thrown when a generic entity is not found.
    /// </summary>
    public class EntityNotFoundException(string message) : Exception(message);
}

