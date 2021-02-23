using UnityEngine;

namespace CGOL {
    public class UIText : MonoBehaviour {


        void Update() {
            if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Return))
            {
                Hide();
                this.enabled = false;
            }
        }
        public void Hide() {
            gameObject.SetActive(false);
        }
    }
}