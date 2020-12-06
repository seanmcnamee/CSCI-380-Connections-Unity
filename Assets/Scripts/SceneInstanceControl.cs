using UnityEngine;
using UnityEngine.UI;

namespace SecAuth {
    public class SceneInstanceControl : MonoBehaviour {
        public static User User = null;

        [SerializeField]
        private Button loggedInOnly = null;

        public void updateButtonStatus() {
            Debug.Log("loggedInOnlyButton: " + loggedInOnly + ", User: " + User);
            if (User != null) {
                Debug.Log("User verified?: " + User.userName + ", " + User.IsVerified());
            }
            loggedInOnly.interactable = User != null && User.IsVerified();
        }
    }
}