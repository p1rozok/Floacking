/*using UnityEngine;

public class DockZone : MonoBehaviour
{
    [SerializeField] private MoveBoat moveBoat;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private KeyCode switchKey = KeyCode.E;
    [SerializeField] private CameraSwitcher cameraSwitcher;
    [SerializeField] private Collider2D boatCollider;
    private bool isPlayerInZone = false;

    void Start()
    {
        if (moveBoat == null)
            Debug.LogError("MoveBoat не назначен в DockZone!");
        if (playerController == null)
            Debug.LogError("PlayerController не назначен в DockZone!");
        if (cameraSwitcher == null)
            Debug.LogError("CameraSwitcher не назначен в DockZone!");
        if (boatCollider == null)
            Debug.LogError("BoatCollider не назначен в DockZone!");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == moveBoat.gameObject)
        {
            isPlayerInZone = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == moveBoat.gameObject)
        {
            isPlayerInZone = false;
        }
    }

    void Update()
    {
        if (isPlayerInZone && Input.GetKeyDown(switchKey))
        {
            cameraSwitcher.SwitchToFixedCamera();
            moveBoat.SetIsActive(false);
            boatCollider.enabled = false;
            playerController.transform.SetParent(null);
            playerController.transform.position = new Vector3(
                moveBoat.transform.position.x,
                moveBoat.transform.position.y,
                playerController.transform.position.z
            );
            playerController.SetIsOnBoat(false);
            cameraSwitcher.SwitchToPlayerCamera();
        }
    }
}
*/