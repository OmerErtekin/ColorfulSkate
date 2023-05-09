using DG.Tweening;
using UnityEngine;

public class Skateboard : MonoBehaviour
{
    #region Components
    [SerializeField] private MeshRenderer skateModel;
    private Material skateMaterial;
    #endregion
    #region Variables
    [SerializeField] private ColorPalette palette;
    [SerializeField] private Transform playerPos;
    private Transform startParent;
    private float targetYScale = 1, currentYScale = 1;
    private bool isUsingByPlayer = false;
    #endregion

    #region Properties
    public bool IsUsingByPlayer => isUsingByPlayer;
    public Transform GetPlayerPos => playerPos;
    public float GetCurrentYScale => currentYScale;
    #endregion

    private void Awake()
    {
        startParent = transform.parent;
        skateMaterial = skateModel.GetComponent<MeshRenderer>().material;
    }

    private void OnEnable()
    {
        EventManager.StartListening(EventKeys.JumpedToSkate, StartRiding);
        EventManager.StartListening(EventKeys.GetOffSkate, StopRiding);
        EventManager.StartListening(EventKeys.OnColorChanged, ChangeSkateColor);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventKeys.JumpedToSkate, StartRiding);
        EventManager.StopListening(EventKeys.GetOffSkate, StopRiding);
        EventManager.StopListening(EventKeys.OnColorChanged, ChangeSkateColor);
    }

    private void LateUpdate()
    {
        if (!isUsingByPlayer) return;
        SetSkateScale();
    }

    public void ShrinkAndScaleSkateboard(float amount)
    {
        targetYScale += amount;
    }

    private void SetSkateScale()
    {
        targetYScale = Mathf.Clamp(targetYScale, 0.1f, Mathf.Infinity);
        currentYScale = Mathf.Lerp(currentYScale, targetYScale, 25 * Time.deltaTime);
        transform.localScale = new Vector3(transform.localScale.x, currentYScale, transform.localScale.z);
    }

    private void StartRiding(object[] obj = null)
    {
        if ((Skateboard)obj[0] != this) return;
        isUsingByPlayer = true;
    }

    private void StopRiding(object[] obj = null)
    {
        if ((Skateboard)obj[0] != this) return;
        transform.parent = startParent;
        isUsingByPlayer = false;
        transform.DOScaleY(0, 0.5f).OnComplete(()=>gameObject.SetActive(false));
    }

    private void ChangeSkateColor(object[] obj = null)
    {
        if (!isUsingByPlayer) return;
        skateMaterial.DOKill();
        skateMaterial.DOColor(palette.colors[(int)obj[0]], 0.5f).SetTarget(this);
    }
}
