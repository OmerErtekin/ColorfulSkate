using DG.Tweening;
using UnityEngine;

public class CollectibleCube : MonoBehaviour,ICollectible
{
    #region Variables
    [SerializeField] private ColorPalette palette;
    [SerializeField] private float iceAmount = 1;
    private Material cubeMaterial;
    #endregion

    #region Components
    [SerializeField] private GameObject cubeModel;
    [SerializeField] private ParticleSystem collectParticle;
    #endregion

    private void OnEnable()
    {
        cubeMaterial = cubeModel.GetComponent<MeshRenderer>().material;
        EventManager.StartListening(EventKeys.OnColorChanged, ChangeCubeColor);
        EventManager.StartListening(EventKeys.GetOffSkate, ResetCubeToWhite);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventKeys.OnColorChanged, ChangeCubeColor);
        EventManager.StopListening(EventKeys.GetOffSkate, ResetCubeToWhite);
    }

    public void OnCollect()
    {
        EventManager.TriggerEvent(EventKeys.CubeCollected,new object[] {iceAmount});
        cubeModel.transform.DOScale(0, 0.5f).SetTarget(this).OnComplete(() => cubeModel.SetActive(false));
        cubeModel.SetActive(false);
        collectParticle.Play();
    }

    private void ChangeCubeColor(object[] obj = null)
    {
        cubeMaterial.DOKill();
        cubeMaterial.DOColor(palette.colors[(int)obj[0]], 0.5f).SetTarget(this);
    }

    private void ResetCubeToWhite(object[] obj = null)
    {
        cubeMaterial.DOKill();
        cubeMaterial.DOColor(palette.colors[(int)ColorFromPalette.White], 0.5f).SetTarget(this);
    }
}
