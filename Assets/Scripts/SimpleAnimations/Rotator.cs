using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private Vector3 rotateVector;
    private void FixedUpdate()
    {
        transform.Rotate(rotateVector * Time.deltaTime);
    }
}
