using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using SecAuth;

namespace Page
{
    public class EmailPage : MonoBehaviour
    {

        public void SendEmail() {
            SceneInstanceControl.User.SendMessageHistory();
            ClearMessages();
        }

        public void ClearMessages() {
            SceneInstanceControl.User.messages.Clear();
        }
    }
}