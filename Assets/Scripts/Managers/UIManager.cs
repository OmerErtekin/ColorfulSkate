using DG.Tweening;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region Components
    [SerializeField] private CanvasGroup startGroup,tapToPlayGroup, gameplayGroup, failGroup, completeGroup;
    [SerializeField] private TMP_Text levelText;
    #endregion

    private void OnEnable()
    {
        startGroup.gameObject.SetActive(true);
        startGroup.alpha = 1;
        EventManager.StartListening(EventKeys.OnLevelCreated, ShowTapToPlayUI);
        EventManager.StartListening(EventKeys.OnLevelStarted, ShowGameplayUI);
        EventManager.StartListening(EventKeys.OnLevelCompleted, ShowLevelCompletedUI);
        EventManager.StartListening(EventKeys.OnLevelFailed, ShowLevelFailedUI);
        EventManager.StartListening(EventKeys.OnPlayButtonClicked, HideStartScreenUI);
        EventManager.StartListening(EventKeys.OnLevelCreated, UpdateLevelText);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventKeys.OnLevelCreated, ShowTapToPlayUI);
        EventManager.StopListening(EventKeys.OnLevelStarted, ShowGameplayUI);
        EventManager.StopListening(EventKeys.OnLevelCompleted, ShowLevelCompletedUI);
        EventManager.StopListening(EventKeys.OnLevelFailed, ShowLevelFailedUI);
        EventManager.StopListening(EventKeys.OnPlayButtonClicked, HideStartScreenUI);
        EventManager.StopListening(EventKeys.OnLevelCreated, UpdateLevelText);
    }

    public void PlayButtonClick()
    {
        Time.timeScale = 1;
        EventManager.TriggerEvent(EventKeys.OnPlayButtonClicked, new object[] { });
    }

    public void TapToPlayButtonClick()
    {
        EventManager.TriggerEvent(EventKeys.OnLevelStarted, new object[] { });
    }

    public void HomeButtonClick()
    {
        Time.timeScale = 0;
        startGroup.gameObject.SetActive(true);
        startGroup.DOFade(1, 0.5f).From(0).SetTarget(this).SetUpdate(true).OnComplete(() =>
        {
            Time.timeScale = 1;
            EventManager.TriggerEvent(EventKeys.OnHomeReturned, new object[] { });
        });
        gameplayGroup.DOFade(0, 0.5f).SetTarget(this).SetUpdate(true).OnComplete(() => gameplayGroup.gameObject.SetActive(false));
    }

    private void HideStartScreenUI(object[] obj = null)
    {
        startGroup.DOFade(0, 0.5f).SetTarget(this).SetUpdate(true).OnComplete(() => startGroup.gameObject.SetActive(false));
    }

    private void ShowTapToPlayUI(object[] obj = null)
    {
        failGroup.gameObject.SetActive(false);
        completeGroup.gameObject.SetActive(false);
        tapToPlayGroup.gameObject.SetActive(true);
    }

    private void ShowGameplayUI(object[] obj = null)
    {
        tapToPlayGroup.gameObject.SetActive(false);
        gameplayGroup.alpha = 1;
        gameplayGroup.gameObject.SetActive(true);
    }

    private void ShowLevelCompletedUI(object[] obj = null)
    {
        completeGroup.gameObject.SetActive(true);
        completeGroup.DOFade(1, 0.5f).From(0).SetTarget(this).SetUpdate(true);
        gameplayGroup.DOFade(0, 0.5f).SetTarget(this).SetUpdate(true).OnComplete(() => gameplayGroup.gameObject.SetActive(false));
    }

    private void ShowLevelFailedUI(object[] obj = null)
    {
        failGroup.gameObject.SetActive(true);
        failGroup.DOFade(1, 0.5f).From(0).SetTarget(this).SetUpdate(true);
        gameplayGroup.DOFade(0, 0.5f).SetTarget(this).SetUpdate(true).OnComplete(() => gameplayGroup.gameObject.SetActive(false));
    }

    private void UpdateLevelText(object[] obj = null)
    {
        levelText.text = $"LEVEL {(int)obj[2]+1}";
    }
}
