using UnityEngine;
using UnityEngine.UI;

namespace ShadowMatch
{
    [RequireComponent(typeof(Button))]
    public class ThemeButtonUI : MonoBehaviour
    {
        [SerializeField]
        private ThemeData themeData;

        [SerializeField]
        private Button button;

        private void Reset()
        {
            if (button == null)
                button = GetComponent<Button>();
        }

        private void Awake()
        {
            if (button == null)
                button = GetComponent<Button>();
        }

        private void OnEnable()
        {
#if UNITY_EDITOR
            UnityEditor.Events.UnityEventTools.RemovePersistentListener(button.onClick, OnButtonClicked);
            UnityEditor.Events.UnityEventTools.AddPersistentListener(button.onClick, OnButtonClicked);
#else
            button.onClick.RemoveListener(OnButtonClicked);
            button.onClick.AddListener(OnButtonClicked);
#endif
        }

        private void OnDisable()
        {
#if UNITY_EDITOR
            UnityEditor.Events.UnityEventTools.RemovePersistentListener(button.onClick, OnButtonClicked);
#else
            button.onClick.RemoveListener(OnButtonClicked);
#endif
        }

        public void OnButtonClicked()
        {
            if (themeData == null)
            {
                Debug.LogWarning("ThemeData is not assigned for this ThemeButtonUI.", gameObject);
                return;
            }

            ShadowMatchGameManager.Instance.SelectTheme(themeData);
        }
    }
}
