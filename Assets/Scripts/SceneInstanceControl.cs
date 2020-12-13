using UnityEngine;
using UnityEngine.UI;
using Data;

namespace SecAuth {
    public class SceneInstanceControl : MonoBehaviour {
        public static User User = null;

        [SerializeField]
        private Button[] loggedInOnly = null;
        [SerializeField]
        private Button[] advisorOnly = null;
        [SerializeField]
        private Button[] loggedOutOnly = null;

        void Start() {
            updateButtonStatus();
        }

        public void Logout() {
            User = null;
            updateButtonStatus();
        }

        public void updateButtonStatus() {
            bool isLoggedInAndVerified = User != null && User.IsVerified();
            bool isAdvisorOrDev = isLoggedInAndVerified && (User.IsAdvisor() || User.IsDeveloper());
            if (isLoggedInAndVerified) {
                Debug.Log("Verified: " + User.userName);
            }
            foreach (Button b in loggedInOnly) {
                b.interactable = isLoggedInAndVerified;
            }
            foreach (Button b in advisorOnly) {
                b.interactable = isAdvisorOrDev;
            }
            foreach (Button b in loggedOutOnly) {
                b.interactable = !isLoggedInAndVerified;
            }
        }
    }
}