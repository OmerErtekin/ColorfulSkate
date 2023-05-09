using UnityEngine;

public class LevelModel : MonoBehaviour
{
    #region Components
    [SerializeField] private Skateboard startSkateboard;
    [SerializeField] private Transform startPos;
    #endregion
    #region Properties
    public Skateboard GetStartSkateBoard => startSkateboard;
    public Transform GetStartPos => startPos;
    #endregion
}
