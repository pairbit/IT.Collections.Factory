namespace IT.Collections.Factory.Internal;

internal static class xRegistrationBehavior
{
    public static bool IsValid(this RegistrationBehavior behavior)
        => behavior == RegistrationBehavior.None ||
           behavior == RegistrationBehavior.OverwriteExisting ||
           behavior == RegistrationBehavior.ThrowOnExisting;
}