using System.Threading;
using DG.Tweening;
using TMPro;
using UnityEngine;


    public class ToastMessage : MonoSingleton<ToastMessage>
    {
        // Localization keys
        public const string TOAST_NOT_ENOUGH_ENERGY = "!!TOAST_NOT_ENOUGH_ENERGY";
        public const string TOAST_NO_MORE_CAMPAIGN = "!!TOAST_NO_MORE_CAMPAIGN";
        public const string TOAST_CANNOT_SELECT_HERO = "!!TOAST_CANNOT_SELECT_HERO";
        public const string TOAST_COMING_SOON = "!!TOAST_COMING_SOON";
        public const string TOAST_SKILL_MAX_LEVEL = "!!TOAST_SKILL_MAX_LEVEL";
        public const string TOAST_HERO_MAX_LEVEL = "!!TOAST_HERO_MAX_LEVEL";
        public const string TOAST_SKILLPOINT_IS_NOT_ENOUGH = "!!TOAST_SKILLPOINT_IS_NOT_ENOUGH";
        public const string TOAST_CASH_IS_NOT_ENOUGH = "!!TOAST_CASH_IS_NOT_ENOUGH";
        public const string TOAST_UNLOCK_AFK = "!!TOAST_UNLOCK_AFK";
        public const string TOAST_NO_IDLE_REWARD = "!!TOAST_NO_IDLE_REWARD";

        [SerializeField] private ToastUI[] _toastUIElements;
        [SerializeField] private int _toastIndex = 0;
        
        void OnValidate()
        {
            if (Application.isPlaying) return;
            _toastUIElements = GetComponentsInChildren<ToastUI>();
        }

        public void Awake()
        {
            if (_toastUIElements == null || _toastUIElements.Length == 0)
                _toastUIElements = GetComponentsInChildren<ToastUI>();
        }

        public void Show(string message)
        {

            var next = GetNextToastUI();
            if (next != null) next.OnShow(message);

        }

        private ToastUI GetNextToastUI()
        {
            _toastIndex++;
            if (_toastIndex >= _toastUIElements.Length)
            {
                _toastIndex = 0;
            }
            return _toastUIElements[_toastIndex];
        }
    }
