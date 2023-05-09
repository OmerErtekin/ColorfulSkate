using System.Collections;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    #region Components
    [SerializeField] private Transform objectToFollow;
    #endregion
    #region Variables
    [SerializeField] private Vector3 offset;
    private Vector3 velocity;
    #endregion
    private void OnEnable()
    {
        EventManager.StartListening(EventKeys.OnLevelCreated, ResetCamera);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventKeys.OnLevelCreated, ResetCamera);
    }

    private void LateUpdate()
    {
        if (!objectToFollow) return;
        Vector3 targetVector = new(transform.position.x, transform.position.y, objectToFollow.transform.position.z + offset.z);
        transform.position = Vector3.SmoothDamp(transform.position, targetVector, ref velocity,0.25f);
    }

    private void ResetCamera(object[] obj = null)
    {
        StartCoroutine(ResetRoutine());
    }

    private IEnumerator ResetRoutine()
    {
        yield return new WaitForFixedUpdate();
        transform.position = new Vector3(objectToFollow.position.x, transform.position.y, objectToFollow.transform.position.z + offset.z);
    }
}
