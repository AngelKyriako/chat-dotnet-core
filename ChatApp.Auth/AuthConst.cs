namespace ChatApp.Auth {

    /// <summary>
    /// Holds any constants related to the authentication of the app.
    /// 
    /// ATTENTION:
    /// We use our own clain names because dotnet core converts claim types from JwtRegisteredClaimNames to urls
    /// .eg JwtRegisteredClaimNames.UniqueName='unique_name' is converted to 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'
    /// </summary>
    public static class AuthConst {

        public static readonly string POLICY_REGULAR_USER = "RegularUser";
        public static readonly string CLAIM_ID = "Id";
        public static readonly string CLAIM_JTI = "Jti";
        public static readonly string CLAIM_IAT = "Iat";
        public static readonly string CLAIM_USERNAME = "Username";
        public static readonly string CLAIM_FIRSTNAME = "Firstname";
        public static readonly string CLAIM_LASTNAME = "Lastname";

        public static readonly string POLICY_PRO_USER = "ProUser";
        public static readonly string CLAIM_MEMBERSHIP = "Membership";
        public static readonly string CLAIM_MEMBERSHIP_VALUE_BASIC = "Basic";
        public static readonly string CLAIM_MEMBERSHIP_VALUE_MORE_GUNS = "MoreGuns";

        public static readonly string POLICY_ADMIN = "AdminOnly";
        public static readonly string CLAIM_IS_ADMIN = "IsAdmin";
    }
}
