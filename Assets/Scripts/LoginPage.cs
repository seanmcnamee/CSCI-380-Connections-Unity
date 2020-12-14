using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DB;
using SecAuth;

namespace Page
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
            string strFirstName = firstName.text;
            string strLastName = lastName.text;
            string strPassword = password.text;




            Debug.Log("User-pass: " + strFirstName + " " + strLastName + ", " + strPassword);

            if (Authenticator.IsValidString(strFirstName) && Authenticator.IsValidString(strLastName) && Authenticator.IsValidString(strPassword)) {
                SceneInstanceControl.User = Authenticator.Login(strFirstName, strLastName, strPassword);

                bool isLoggedInAndVerified = SceneInstanceControl.User != null && SceneInstanceControl.User.IsVerified();

                if (isLoggedInAndVerified) {
                    //ChangeToEncrpted();
                    response.text = "";
                    SceneManager.LoadScene(menuSwitch);
                } else {
                    if (SceneInstanceControl.User == null) {
                        response.text = "Invalid Credentials, Try again";
                    } else {
                        response.text = "Not yet verified. Contact your advisor";
                    }
                }
            } else {
                response.text = "Something isn't filled out correctly";
            }
        }

        public void ClearResponse() {
            response.text = "";
        }


        //Only used when transitioning to encryption
        private void ChangeToEncrpted() {
            response.text = Authenticator.PasswordEncryption(password.text);

            if (Authenticator.PasswordEncryption(password.text).Equals(response.text)) {
                response.text = "Same!";
                Queries conn = new Queries();
                conn.setPassword(firstName.text, lastName.text, Authenticator.PasswordEncryption(password.text));
                conn.closeConenction();
            } else {
                response.text = "different :(";
            }
        }
    }
}