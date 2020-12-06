using UnityEngine;
using UnityEngine.UI;

namespace SecAuth
{
    public class LoginPage : MonoBehaviour
    {
        [SerializeField]
        private InputField firstName;
        [SerializeField]
        private InputField lastName;
        [SerializeField]
        private InputField password;

        public void Login() {
            string username = firstName.text + lastName.text;
            string userPassword = password.text;
            Debug.Log("User-pass: " + username + ", " + userPassword);
            SceneInstanceControl.User = Authenticator.Login(username, userPassword);
        }
    }
}