using System.Threading;
using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;


    public class ToastUI : MonoBehaviour
    {
        private const int DISPLAY_DURATION = 1000;
        private const float FADE_IN_DURATION = 0.2f;
        private const float FADE_OUT_DURATION = 0.5f;
        private const float MOVE_Y_END_VALUE = 150f;
        private const float MOVE_DURATION = 0.5f;

        [SerializeField] private TextMeshProUGUI _toastText;
        [SerializeField] private CanvasGroup _toastCanvasGroup;

        private CancellationTokenSource _toastCancellation;

        public void OnShow(string message)
        {
            _toastText.text = message;

            if (_toastCancellation != null)
            {
                _toastCancellation.Cancel();
                _toastCancellation.Dispose();
                _toastCancellation = null;
            }
            
            DisplayToastThenFade();
        }

        private void ResetBeforeShowing()
        {
            _toastCanvasGroup.alpha = 0;
            _toastCanvasGroup.transform.DOLocalMoveY(0, 0);
        }

        private async Task DisplayToastThenFade()
        {
            // Wait 1 frame for previous thread to be cancelled completely
            await Task.Yield();
            ResetBeforeShowing();

            _toastCanvasGroup.alpha = 0;
            _toastCanvasGroup.transform.SetAsLastSibling();
            _toastCanvasGroup.transform.DOLocalMoveY(MOVE_Y_END_VALUE, MOVE_DURATION);
            _toastCanvasGroup.DOFade(1, FADE_IN_DURATION);
            
            await Task.Delay(DISPLAY_DURATION,CancellationToken.None);
            
            
            _toastCanvasGroup.DOFade(0, FADE_OUT_DURATION).OnComplete(() =>  _toastCanvasGroup.alpha = 0);
        }
    }