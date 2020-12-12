using UnityEngine;
using UnityEngine.UI;

using System.Collections.Generic;

using Data;

namespace SecAuth
{
    public class RegisterPage : MonoBehaviour
    {
        [SerializeField]
        private InputField firstName;
        [SerializeField]
        private InputField lastName;
        [SerializeField]
        private InputField password;
        [SerializeField]
        private InputField email;
        [SerializeField]
        private Dropdown userType;

        [SerializeField]
        private Dropdown homeSchool;



        [SerializeField]
        private Dropdown schoolDropDown;

        [SerializeField]
        private Text textShowingSchools;

        [SerializeField]
        private Button submit;
  
        private List<string> schools = new List<string>();


        public void UserTypeSet() {
            if (userType.value > 1) {
                //Moderator/Admin
                schoolDropDown.interactable = false;
                submit.interactable = true;
                schools.Clear();
                setTextForSchools();
            } else if (userType.value == 1) {
                //High School
                schoolDropDown.interactable = true;
                submit.interactable = false;
            } else {
                //Nothing selected
                submit.interactable = false;
            }
        }


        public void AddSchool() {
            if (schoolDropDown.value > 0) {
                string schoolToAdd = schoolDropDown.captionText.text;

                if (schools.Contains(schoolToAdd)) {
                    schools.Remove(schoolToAdd);
                } else {
                    schools.Add(schoolToAdd);
                }
                
                setTextForSchools();
            }

            submit.interactable = (schools.Count > 0);
            schoolDropDown.value = 0;
        }

        private void setTextForSchools() {
            string text = "";
            foreach (string school in schools) {
                text += school;
                if (school != schools[schools.Count-1]) {
                    text += ", ";
                }
            }
            textShowingSchools.text = text;
        }

        public void Register() {
            //Login with the provided credentials
            string strfirstName = firstName.text;
            string strlastName = lastName.text;
            string struserPassword = password.text;
            string strEmail = email.text;
            int intUserType = userType.value;
            string strhomeSchool = homeSchool.captionText.text;
            string[] lstSchools = schools.ToArray();

            //Make sure above information isn't null

            if ((strfirstName != null) && (strlastName != null) && (struserPassword != null) && (strEmail != null) && (intUserType > 0) && (intUserType < 4) && (strhomeSchool != null)) {
                Authenticator.Register(strfirstName, strlastName, struserPassword, ((User.UserType) intUserType), strEmail, strhomeSchool, lstSchools);
            }
            // firstName,  lastName,  password, userType,  email,  homeSchool, string[] schools
            

        }
    }
}