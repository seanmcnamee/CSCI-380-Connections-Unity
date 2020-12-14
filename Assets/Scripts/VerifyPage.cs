using UnityEngine;
using UnityEngine.UI;

using SecAuth;

namespace Page
{
    public class VerifyPage : MonoBehaviour
    {
        [SerializeField]
        private InputField firstName;
        [SerializeField]
        private InputField lastName;
        [SerializeField]
        private InputField verificationCode;
        [SerializeField]
        private Text response;

        public void Verify() {
            //Login with the provided credentials
            string strFirstName = firstName.text;
            string strLastName = lastName.text;
            string strVerificationCode = verificationCode.text;

            if (Authenticator.IsValidString(strFirstName) && Authenticator.IsValidString(strLastName) && Authenticator.IsValidString(strVerificationCode)) {
                bool success = Authenticator.VerifyAccount(strFirstName, strLastName, strVerificationCode);

                if (success) {
                    response.text = "Verified.";
                } else {
                    response.text = "Invalid Credentials, Try again";
                }
            } else {
                response.text = "Something isn't filled out correctly";
            }
        }

        public void ClearResponse() {
            response.text = "";
        }
    }
}