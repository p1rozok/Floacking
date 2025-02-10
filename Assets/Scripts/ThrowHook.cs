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
    [SerializeField] private FishingTrigger fishingTrigger;
    
    private bool isHookDeployed = false;
    private GameObject currentHook;
    private bool _isStartFishing=false;

    public event UnityAction OnTrowHook;
    public event UnityAction OnTrowRetrieve;

    
    private void OnEnable()
    {
        fishingTrigger.OnCollectebls += Retrieve;
    }
    private void OnDisable()
    {
        fishingTrigger.OnCollectebls -= Retrieve;
    }

    private void Start()
    {
        ToggleMiniGame.OnCath.AddListener(() =>
        {
            _isStartFishing = true;

        });

        fishingRod.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(throwKey) && !isHookDeployed)
        {
            Throw();
        }
        else if (Input.GetKeyDown(throwKey) && isHookDeployed&& !_isStartFishing)
        {
            Retrieve();
        }

       
                
    }

      private void Throw()
    {   OnTrowHook?.Invoke();
       
        fishingRod.SetActive(true);
       
        currentHook = Instantiate(hookPrefab, spawnPoint.position, Quaternion.identity);
        currentHook.GetComponent<Hook>().SetBait(Inventory.Instance.CurrentBait());
     

        Rigidbody2D rb = currentHook.GetComponent<Rigidbody2D>();


        if (rb != null)
        {
            Vector2 throwDirection = transform.right;
            rb.velocity = throwDirection * throwForce;
        }

        isHookDeployed = true;
        
        var child = currentHook.transform.GetChild(0);
 
        cameraSwitcher.SwitchToHookCamera(child.transform);
      
        fishingLine.SetPoint(currentHook);
        fishingLine.SetPosition(currentHook.transform.position);
    }

    private void Retrieve()
    {
        OnTrowRetrieve?.Invoke();

        _isStartFishing = false;
        fishingRod.SetActive(false);
        
        if (currentHook != null)
        {
            Destroy(currentHook);
        }

        isHookDeployed = false;

        cameraSwitcher.SwitchToMainCamera();
    }
}
