using UnityEngine;
using UnityEngine.Events;

public class ThrowHook : MonoBehaviour
{
    [SerializeField] private GameObject hookPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float throwForce = 5f;
    [SerializeField] private KeyCode throwKey = KeyCode.F;
    [SerializeField] private CameraSwitcher cameraSwitcher;
    [SerializeField] private GameObject fishingRod;
    [SerializeField] private FishingLine fishingLine;
    [SerializeField] private returnPosition returnPositionScript;
    [SerializeField] private Joystick joystick;
    [SerializeField] private Inventory inventory;
  
    private GameObject currentHook;
    private bool isHookDeployed = false;
    private bool _isStartFishing = false;

    public event UnityAction OnTrowHook;
    public event UnityAction OnTrowRetrieve;

    private void Start()
    {
        ToggleMiniGame.OnWin.AddListener(() => Retrieve());
        ToggleMiniGame.OnCath.AddListener(() => _isStartFishing = true);
        fishingRod.SetActive(false);
    }

    private void Update()
    {
        if (!inventory.PosibleTrowh())
        {
            return;
        }
        if (Input.GetKeyDown(throwKey) && !isHookDeployed)
        {
            Throw();
        }
        else if (Input.GetKeyDown(throwKey) && isHookDeployed && !_isStartFishing)
        {
            Retrieve();
        }
    }

    public void Throw()
    {
        OnTrowHook?.Invoke();

        fishingRod.SetActive(true);


        currentHook = Instantiate(hookPrefab, spawnPoint.position, Quaternion.identity);


        MoveHook moveHook = currentHook.GetComponentInChildren<MoveHook>();
        if (moveHook != null)
        {
            moveHook.Initialize(joystick); 
        }
      

        Hook hookComponent = currentHook.GetComponent<Hook>();
        if (hookComponent != null)
        {
            hookComponent.SetBait(Inventory.Instance.CurrentBait());
        }
      

        Transform baitTransform = currentHook.transform.GetChild(0);
        returnPositionScript.SetBaitTransform(baitTransform);

        Rigidbody2D rb = currentHook.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 throwDirection = transform.right;
            rb.velocity = throwDirection * throwForce;
        }

        isHookDeployed = true;


        var child = currentHook.transform.GetChild(0); 
        if (child != null)
        {
            cameraSwitcher.SwitchToHookCamera(child.transform);
          
        }
        fishingLine.SetPoint(currentHook);
        fishingLine.SetPosition(currentHook.transform.position);
    }

    public void Retrieve()
    {
        OnTrowRetrieve?.Invoke();

        _isStartFishing = false;
        fishingRod.SetActive(false);

        if (currentHook != null)
        {
            Destroy(currentHook);
        }

        returnPositionScript.SetBaitTransform(null);
        isHookDeployed = false;
        cameraSwitcher.SwitchToMainCamera();
    }
}
