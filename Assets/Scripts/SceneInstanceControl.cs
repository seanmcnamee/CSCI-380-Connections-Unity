using UnityEngine;
using UnityEngine.UI;

namespace SecAuth {
    public class SceneInstanceControl : MonoBehaviour {
        public static User User = null;

        [SerializeField]
        private Button loggedInOnly = null;

        public void updateButtonStatus() {
            loggedInOnly.interactable = User != null && User.IsVerified();
        }
    }
}