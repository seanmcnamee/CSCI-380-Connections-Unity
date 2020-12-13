using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        [SerializeField]
        private Text response;
        [SerializeField]
        private string menuSwitch;

        public void Login() {
            //Login with the provided credentials
            string username = firstName.text + lastName.text;
            string userPassword = password.text;
            Debug.Log("User-pass: " + username + ", " + userPassword);
            SceneInstanceControl.User = Authenticator.Login(firstName.text, lastName.text, userPassword);

            if (SceneInstanceControl.User == null) {
                response.text = "Invalid Credentials, Try again";
            } else {
                response.text = "";
                SceneManager.LoadScene(menuSwitch);
            }
        }

        public void ClearResponse() {
            response.text = "";
        }
    }
}