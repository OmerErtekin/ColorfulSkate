using DG.Tweening;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Components
    [SerializeField] private Animator playerAnimator;
    private Skateboard currentSkateboard,startSkateboard;
    #endregion

    #region Variables
    private ColorFromPalette currentColor = ColorFromPalette.White;
    private bool canPassToNextSkate = true;
    #endregion

    private void OnEnable()
    {
        EventManager.StartListening(EventKeys.CubeCollected, ScaleUpTheSkate);
        EventManager.StartListening(EventKeys.OnColorChanged, ChangePlayerColor);
        EventManager.StartListening(EventKeys.OnCameToDoor, SetPlayerOnDoor);
        EventManager.StartListening(EventKeys.OnLevelCreated, SetPlayerAtLevelCreate);
        EventManager.StartListening(EventKeys.OnLevelStarted, JumpToFirstSkate);
        EventManager.StartListening(EventKeys.OnHomeReturned, ResetPlayerAtHomeScreen);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventKeys.CubeCollected, ScaleUpTheSkate);
        EventManager.StopListening(EventKeys.OnColorChanged, ChangePlayerColor);
        EventManager.StopListening(EventKeys.OnCameToDoor, SetPlayerOnDoor);
        EventManager.StopListening(EventKeys.OnLevelCreated,SetPlayerAtLevelCreate);
        EventManager.StopListening(EventKeys.OnLevelStarted, JumpToFirstSkate);
        EventManager.StopListening(EventKeys.OnHomeReturned, ResetPlayerAtHomeScreen);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ICollectible collectible))
        {
            collectible.OnCollect();
        }
    }

    private void PassToNextSkate(Skateboard skateboard)
    {
        if (!canPassToNextSkate) return;

        if (currentSkateboard != null)
        {
            EventManager.TriggerEvent(EventKeys.GetOffSkate, new object[] { currentSkateboard });
        }
        canPassToNextSkate = false;
        currentSkateboard = skateboard;
        playerAnimator.SetInteger("state", 2);
        JumpToNextSkate();
    }

    private void JumpToNextSkate()
    {
        transform.DOJump(currentSkateboard.GetPlayerPos.position, 2, 1, 1f).SetTarget(this).OnComplete(() =>
        {
            playerAnimator.SetInteger("state", 1);
            currentSkateboard.transform.parent = transform;
            canPassToNextSkate = true;
            currentColor = ColorFromPalette.White;
            EventManager.TriggerEvent(EventKeys.JumpedToSkate, new object[] { currentSkateboard });
        });
    }

    private void JumpToFinish(Vector3 finishPos)
    {
        if (currentSkateboard != null)
        {
            EventManager.TriggerEvent(EventKeys.GetOffSkate, new object[] { currentSkateboard });
        }
        playerAnimator.SetInteger("state", 2);
        transform.DOJump(finishPos, 3, 1, 1f).OnComplete(() =>
        {
            canPassToNextSkate = true;
            EventManager.TriggerEvent(EventKeys.OnLevelCompleted, new object[] { });
            playerAnimator.SetInteger("state", 4);
        }).SetTarget(this);
    }

    private void FallFromSkate()
    {
        if (currentSkateboard != null)
        {
            EventManager.TriggerEvent(EventKeys.GetOffSkate, new object[] { currentSkateboard });
        }

        currentSkateboard = null;
        playerAnimator.SetInteger("state", 3);
        transform.DOJump(new Vector3(transform.position.x, 0.25f, transform.position.z - 5), 3, 1, 0.75f).OnComplete(() =>
        {
            canPassToNextSkate = true;
            EventManager.TriggerEvent(EventKeys.OnLevelFailed, new object[] {});
        });
    }

    private void SetPlayerAtLevelCreate(object[] obj = null)
    {
        currentColor = ColorFromPalette.White;
        playerAnimator.SetInteger("state", 0);
        startSkateboard = (Skateboard)obj[0];
        transform.position = ((Transform)obj[1]).position;
    }

    private void ResetPlayerAtHomeScreen(object[] obj =  null)
    {
        transform.DOKill();
        canPassToNextSkate = true;
        if (currentSkateboard != null)
        {
            Destroy(currentSkateboard.gameObject);
        }
        playerAnimator.SetInteger("state", 0);
    }

    private void JumpToFirstSkate(object[] obj = null)
    {
        PassToNextSkate(startSkateboard);
    }

    private void SetPlayerOnDoor(object[] obj = null)
    {
        if (!canPassToNextSkate) return;

        canPassToNextSkate = false;
        if ((ColorFromPalette)obj[0] == currentColor && (float)obj[1] <= currentSkateboard.GetCurrentYScale)
        {
            if (((Transform)obj[2]).TryGetComponent(out Skateboard board))
            {
                canPassToNextSkate = true;
                PassToNextSkate(board);
            }
            else
            {
                JumpToFinish(((Transform)obj[2]).position);
            }
        }
        else
        {
            FallFromSkate();
        }
    }

    private void ScaleUpTheSkate(object[] obj = null)
    {
        currentSkateboard.ShrinkAndScaleSkateboard((float)obj[0]);
    }

    private void ChangePlayerColor(object[] obj = null)
    {
        currentColor = (ColorFromPalette)obj[0];
    }
}
