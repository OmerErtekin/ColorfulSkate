using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class ColorDoor : MonoBehaviour
{
    #region Components
    [SerializeField] private MeshRenderer boxRenderer;
    [SerializeField] private Transform nextTarget;
    #endregion
    #region Variables
    [SerializeField] private Transform doorModel;
    [SerializeField] private ColorPalette palette;
    [SerializeField] private ColorFromPalette requiredColor;
    [SerializeField] private float requiredIceLength = 3;
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        //Will activate only if the player is closer to this door (rather than the other door)
        if (!other.TryGetComponent(out Skateboard board) || !board.IsUsingByPlayer || Mathf.Sign(transform.position.x) != Mathf.Sign(other.transform.position.x))
            return;

        EventManager.TriggerEvent(EventKeys.OnCameToDoor, new object[] { requiredColor, requiredIceLength,nextTarget });
    }

#if UNITY_EDITOR
    public void SetDoorModel()
    {
        doorModel.transform.localScale = new Vector3(1, requiredIceLength, 1);
        boxRenderer.material = palette.materials[(int)requiredColor];
        EditorUtility.SetDirty(gameObject);
    }
#endif
}


