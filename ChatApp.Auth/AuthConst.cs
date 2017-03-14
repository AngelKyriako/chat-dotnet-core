namespace ChatApp.Auth {

    public static class AuthConst {

        public static readonly string POLICY_REGULAR_USER = "RegularUser";

        public static readonly string POLICY_PRO_USER = "ProUser";
        public static readonly string CLAIM_MEMBERSHIP = "Membership";
        public static readonly string CLAIM_MEMBERSHIP_VALUE_BASIC = "Basic";
        public static readonly string CLAIM_MEMBERSHIP_VALUE_MORE_GUNS = "MoreGuns";

        public static readonly string POLICY_ADMIN = "AdminOnly";
        public static readonly string CLAIM_IS_ADMIN = "IsAdmin";
    }
}
