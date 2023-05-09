using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Components
    private Rigidbody characterRb;
    #endregion

    #region Variables
    [SerializeField] private float defaultZSpeed = 5, maxSwerveAmount = 5,swerveSpeed = 5, defaultXWidth = 5;
    private bool canMove = false;
    private float inputX, zSpeed, lerpedX;
    private Vector3 lastMousePos;
    #endregion

    private void Awake()
    {
        zSpeed = 0;
        characterRb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        EventManager.StartListening(EventKeys.JumpedToSkate, StartMovement);
        EventManager.StartListening(EventKeys.GetOffSkate, StopMovement);
        EventManager.StartListening(EventKeys.OnLevelCreated, StopMovement);
        EventManager.StartListening(EventKeys.OnHomeReturned, StopMovement);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventKeys.JumpedToSkate, StartMovement);
        EventManager.StopListening(EventKeys.GetOffSkate, StopMovement);
        EventManager.StopListening(EventKeys.OnLevelCreated, StopMovement);
        EventManager.StopListening(EventKeys.OnHomeReturned, StopMovement);
    }

    private void Update()
    {
        HandleMovement();
        if (!canMove) return;
        HandleInput();
    }

    private void HandleMovement()
    {
        inputX = Mathf.Clamp(inputX, -maxSwerveAmount, maxSwerveAmount) * swerveSpeed;
        lerpedX = Mathf.Lerp(lerpedX, inputX, 25 * Time.deltaTime);
        if ((transform.position.x > defaultXWidth / 2 && lerpedX > 0) || (transform.position.x < -defaultXWidth / 2 && lerpedX < 0))
        {
            characterRb.velocity = new Vector3(0, characterRb.velocity.y, zSpeed);
        }
        else
        {
            characterRb.velocity = new Vector3(lerpedX, characterRb.velocity.y, zSpeed);
        }
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePos = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            inputX = Input.mousePosition.x - lastMousePos.x;
            lastMousePos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            inputX = 0;
        }
    }

    private void StartMovement(object[] obj = null)
    {
        characterRb.constraints = RigidbodyConstraints.FreezeRotation;
        zSpeed = defaultZSpeed;
        canMove = true;
    }

    private void StopMovement(object[] obj = null)
    {
        characterRb.constraints = RigidbodyConstraints.FreezeAll;
        canMove = false;
        inputX = 0;
        zSpeed = 0;
    }
}
