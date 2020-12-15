using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections.Generic;

using UnityEngine.SceneManagement;
using Data;
using DB;
using SecAuth;

namespace Page
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
        private InputField advisorSchool;
        [SerializeField]
        private Toggle IsCollege;
        [SerializeField]
        private Dropdown schoolDropDown;
        [SerializeField]
        private Text textShowingSchools;

        [SerializeField]
        private Button submit;
        [SerializeField]
        private string menuSwitch;

        private string chatrooms = "ChatRooms?";
        private string homeSchoolText = "HomeSchool?";
  
        public static List<string> schools = new List<string>();

        public void UserTypeSet() {
            showAdvisorInput(userType.value == 3);
            showHomeSchools(userType.value == 1 || userType.value == 2);
            showChatSchools(userType.value == 1);
            showSubmit(userType.value == 2 || userType.value == 3);

            setTextForSchools();
        }

        private void showAdvisorInput(bool show) {
            //advisorSchool.interactable = show;
            if (!show) {
                advisorSchool.text = "";
            }
            advisorSchool.gameObject.SetActive(show);
            IsCollege.gameObject.SetActive(show);
        }

        private void showChatSchools(bool show) {
            if (!show) {
                schools.Clear();
            }
            schoolDropDown.gameObject.SetActive(show);
        }

        private void showHomeSchools(bool show) {
            if (show) {
                Queries conn = new Queries();
                homeSchool.ClearOptions();
                List<string> homeSchoolsList = conn.getAllSchools(userType.value == 2);
                homeSchoolsList.Insert(0, homeSchoolText);
                homeSchool.AddOptions(homeSchoolsList);
                conn.closeConenction();
            }
            //homeSchool.interactable = true;

            homeSchool.gameObject.SetActive(show);
        }

        private void showSubmit(bool show) {
            submit.gameObject.SetActive(show);
        }

        void Start() {
            Queries conn = new Queries();
            schoolDropDown.ClearOptions();
            List<string> collegeRooms = conn.getAllSchools(true);
            collegeRooms.Insert(0, chatrooms);
            schoolDropDown.AddOptions(collegeRooms);
            conn.closeConenction();
            UserTypeSet();
        }

        public void AddSchool() {
            Debug.Log(schools.Count);

            if (schoolDropDown.value > 0) {
                string schoolToAdd = schoolDropDown.captionText.text;

                if (schools.Contains(schoolToAdd)) {
                    schools.Remove(schoolToAdd);
                } else {
                    schools.Add(schoolToAdd);
                }
                
                setTextForSchools();
            }

            showSubmit(schools.Count > 0);
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

            string strhomeSchool = "";
            if (intUserType == 3) {
                //Only advisors input their own school
                strhomeSchool = advisorSchool.text;
            } else if (homeSchool.value > 0) { //All other users choose from the dropdown (0 isn't allowed)
                    strhomeSchool = homeSchool.captionText.text;
            }
            
            //Make sure above information isn't bad
            if (Authenticator.IsValidString(strfirstName) && Authenticator.IsValidString(strlastName) && Authenticator.IsValidString(struserPassword) && 
                                            Authenticator.IsValidString(strEmail) && Authenticator.IsValidEmail(strEmail) && (intUserType > 0) && (intUserType < 4)) {
                Authenticator.Register(strfirstName, strlastName, struserPassword, ((User.UserType) intUserType), strEmail, strhomeSchool, schools, IsCollege.isOn);
                SceneManager.LoadScene(menuSwitch);
            } else {
                textShowingSchools.text = "Something isn't filled out correctly";
            }
            
        }
    }
}