using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections.Generic;


using UnityEngine.SceneManagement;
using Data;
using SecAuth;

namespace Page
{
    public class ChatSelector : MonoBehaviour
    {
        [SerializeField]
        private Dropdown chatRoomDropDown;
        

        private string chatrooms = "Select chat room";
  

        void Start() {
            chatRoomDropDown.ClearOptions();
            List<string> chatRoomList = SceneInstanceControl.User.schoolNames;
            chatRoomList.Insert(0, chatrooms);
            chatRoomDropDown.AddOptions(chatRoomList);
        }
    }
}