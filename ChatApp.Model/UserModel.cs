namespace ChatApp.Model {

    public class UserModel<K> : BaseModel<K> {

        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }

        public void Copy(UserModel<K> other) {
            Username = other.Username;
            Firstname = other.Firstname;
            Lastname = other.Lastname;
        }
    }
}
