namespace AngularSPA.Auth.Helpers
{
    public class SignInResult
    {
        public bool Succeeded { get; private set; }
        public static SignInResult Success { get; } = new SignInResult { Succeeded = true };
        public static SignInResult Failed()
        {
            return new SignInResult { Succeeded = false };
        }

        public override string ToString() => Succeeded ? "Succeeded" : "Failed";
    }
}
