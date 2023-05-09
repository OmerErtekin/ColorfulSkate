using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class IceTank : MonoBehaviour
{
    #region Components
    [SerializeField] private Image fillBar;
    [SerializeField] private Animator tankAnimator;
    private Skateboard currentBoard;
    #endregion

    #region Variables
    [SerializeField] private ColorPalette palette;
    [SerializeField] private float startAmount = 2.5f, maxAmount = 5, inputTreshold = 3, tankUseSpeed = 7.5f;
    private float currentAmount, inputY;
    private Vector3 mousePos;
    private bool canUseTank = false;
    #endregion

    private void OnEnable()
    {
        EventManager.StartListening(EventKeys.JumpedToSkate, SetTankUseable);
        EventManager.StartListening(EventKeys.GetOffSkate, SetTankUnuseable);
        EventManager.StartListening(EventKeys.OnColorChanged, SetTankUIColor);
        EventManager.StartListening(EventKeys.OnLevelCreated, ResetTank);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventKeys.JumpedToSkate, SetTankUseable);
        EventManager.StopListening(EventKeys.GetOffSkate, SetTankUnuseable);
        EventManager.StopListening(EventKeys.OnColorChanged, SetTankUIColor);
        EventManager.StopListening(EventKeys.OnLevelCreated, ResetTank);
    }

    private void Update()
    {
        if (!canUseTank) return;
        HandleInput();
        HandleScale();
    }

    public void ModifyTankAmount(float usedAmount)
    {
        if (currentBoard == null || (usedAmount > 0 && currentAmount <= 0) || (usedAmount < 0 && currentBoard.GetCurrentYScale < 0.25f))
        {
            tankAnimator.SetInteger("state", 0);
            return;
        }
        tankAnimator.SetInteger("state", 1);
        currentAmount -= usedAmount;
        currentAmount = Mathf.Clamp(currentAmount, 0, maxAmount);
        currentBoard.ShrinkAndScaleSkateboard(usedAmount);
        fillBar.fillAmount = currentAmount / maxAmount;
    }

    private void SetTankUseable(object[] obj = null)
    {
        currentBoard = (Skateboard)obj[0];
        canUseTank = true;
    }

    private void SetTankUnuseable(object[] obj = null)
    {
        inputY = 0;
        tankAnimator.SetInteger("state", 0);
        canUseTank = false;
        ResetColor();
    }

    private void ResetTank(object[] obj = null)
    {
        SetTankUnuseable();
        currentAmount = startAmount;
        fillBar.fillAmount = currentAmount / maxAmount;
    }

    private void ResetColor()
    {
        fillBar.DOColor(palette.colors[(int)ColorFromPalette.White], 0.5f).SetTarget(this).SetUpdate(true);
    }

    private void SetTankUIColor(object[] obj = null)
    {
        fillBar.DOColor(palette.colors[(int)obj[0]], 0.5f).SetTarget(this);
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mousePos = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            inputY = Input.mousePosition.y - mousePos.y;
            mousePos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            inputY = 0;
        }
    }

    private void HandleScale()
    {
        if (Mathf.Abs(inputY) < inputTreshold || currentAmount < 0)
        {
            tankAnimator.SetInteger("state", 0);
            return;
        }
        if (inputY > 0)
        {
            ModifyTankAmount(Time.deltaTime * tankUseSpeed);
        }
        else
        {
            ModifyTankAmount(-Time.deltaTime * tankUseSpeed);
        }
    }
}
