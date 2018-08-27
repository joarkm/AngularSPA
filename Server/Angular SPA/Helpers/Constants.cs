namespace AngularSPA.Helpers
{
    public static class Constants
    {

        public static class Strings
        {
            // Taken from https://github.com/mmacneil/AngularASPNETCore2WebApiAuth/blob/master/src/Helpers/Constants.cs
            public static class JwtClaimIdentifiers
            {
                public const string Rol = "rol", Id = "id";
            }
            
        }
        public enum LoginStates
        {
            OK,
            InvalidCredentials,
            LockedOut,
            Requires2FA //To be implemented?
        }
    }
}
