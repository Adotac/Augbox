using UnityEngine;

namespace Augbox
{
    public class UIPanelGameSettings : MonoBehaviour
    { 
        public static UIPanelGameSettings Instance { get; private set; }

        private void Awake() {
            Instance = this;
            Hide();
        }

        public void Back()
        {
            // UIPanelManager.Instance.HidePanel<UIPanelGameSettings>(false);
            Hide();
        }

        private void Hide() {
            this.gameObject.SetActive(false);
        }

        public void Show() {
            this.gameObject.SetActive(true);
        }
    }
}