namespace Karting.Domain.Exceptions;

/// <summary>
/// Represents the DomainRuleException class.
/// </summary>
public sealed class DomainRuleException(string message) : Exception(message);
