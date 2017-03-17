using Newtonsoft.Json;

namespace ChatApp.Model {

    public class UserModel : BaseModel {

        public string Username { get; set; }

        public string Password { get; set; }

        [JsonIgnore]
        public string PasswordHash { get; set; }
        [JsonIgnore]
        public byte[] PasswordSalt { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public void Copy(UserModel other) {
            if (other.Username != null && other.Username.Length > 0) {
                Username = other.Username;
            }
            if (other.Firstname != null) {
                Firstname = other.Firstname;
            }
            if (other.Lastname != null) {
                Lastname = other.Lastname;
            }
        }
    }
}
