using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections.Generic;

using Data;
using UnityEngine.SceneManagement;
using DB;

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
        private InputField advisorSchool;
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
  
        private List<string> schools = new List<string>();

        void Start() {
            Queries conn = new Queries();
            schoolDropDown.ClearOptions();
            List<string> collegeRooms = conn.getAllRooms(true);
            collegeRooms.Insert(0, chatrooms);
            schoolDropDown.AddOptions(collegeRooms);
            conn.closeConenction();
        }

        public void UserTypeSet() {
            Queries conn;
            List<string> homeSchoolsList;
            switch (userType.value) {
                case 1:
                    //High School
                    advisorSchool.text = "";
                    schoolDropDown.interactable = true;
                    submit.interactable = false;
                    advisorSchool.interactable = false;
                    homeSchool.interactable = true;
                    conn = new Queries();
                    homeSchool.ClearOptions();
                    homeSchoolsList = conn.getAllRooms(false);
                    homeSchoolsList.Insert(0, homeSchoolText);
                    homeSchool.AddOptions(homeSchoolsList);
                    conn.closeConenction();
                    break;
                case 2:
                    //Moderator
                    schools.Clear();
                    setTextForSchools();
                    advisorSchool.text = "";
                    schoolDropDown.interactable = false;
                    submit.interactable = true;
                    advisorSchool.interactable = false;
                    homeSchool.interactable = true;
                    conn = new Queries();
                    homeSchool.ClearOptions();
                    homeSchoolsList = conn.getAllRooms(true);
                    homeSchoolsList.Insert(0, homeSchoolText);
                    homeSchool.AddOptions(homeSchoolsList);
                    conn.closeConenction();
                    break;
                case 3:
                    //Admin
                    schools.Clear();
                    setTextForSchools();

                    schoolDropDown.interactable = false;
                    submit.interactable = true;
                    advisorSchool.interactable = true;
                    homeSchool.interactable = false;
                    break;
                default:
                    schools.Clear();
                    setTextForSchools();
                    advisorSchool.text = "";

                    submit.interactable = false;
                    advisorSchool.interactable = false;
                    homeSchool.interactable = false;
                    schoolDropDown.interactable = false;
                    break;
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

            string strhomeSchool = "";
            if (intUserType == 2) {
                //Only advisors input their own school
                strhomeSchool = advisorSchool.text;
            } else {
                //All other users choose from the dropdown
                if (homeSchool.value > 0) {
                    strhomeSchool = homeSchool.captionText.text;
                }
            }
             
            string[] lstSchools = schools.ToArray();

            //Make sure above information isn't null

            if (!String.IsNullOrEmpty(strfirstName) && !String.IsNullOrEmpty(strlastName) && !String.IsNullOrEmpty(struserPassword) && !String.IsNullOrEmpty(strEmail) && (intUserType > 0) && (intUserType < 4) && !String.IsNullOrEmpty(strhomeSchool)) {
                Authenticator.Register(strfirstName, strlastName, struserPassword, ((User.UserType) intUserType), strEmail, strhomeSchool, lstSchools);
                SceneManager.LoadScene(menuSwitch);
            }
            // firstName,  lastName,  password, userType,  email,  homeSchool, string[] schools
            

        }
    }
}