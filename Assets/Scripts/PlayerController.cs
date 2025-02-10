using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool Docked => _docked;

    [SerializeField] private float _speed = 0.5f;
    private float _direction;
    private SpriteRenderer _flipFace;
    private bool _docked = false;

    private Transform _parent;
    private bool _isInDockZone = false;

    private void Start()
    {
        
        _parent = transform.parent;
        _flipFace = GetComponent<SpriteRenderer>();
    
    }

    private void Update()
    {
        HarvestInput();
        RotationView();
        MovePlayer();
    }

    private void HarvestInput()
    {
        _direction = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.E))
        {
         

            if (_isInDockZone)
            {
                if (_docked)
                {
                    
                    UnDock();
                }
                else
                {

                    Dock();
                }
            }
            else
            {
                
            }
        }
    }

    private void MovePlayer()
    {
        if (_docked)
        {
            
            transform.position += new Vector3(_direction * _speed * Time.deltaTime, 0, 0);
        }
        else
        {
            transform.localPosition = new Vector3(
                Mathf.Clamp(transform.localPosition.x + _direction * _speed * Time.deltaTime, -0.20f, 0.35f),
                transform.localPosition.y,
                transform.localPosition.z
            );
        }
    }

    private void RotationView()
    {
        if (_direction < 0)
        {
            _flipFace.flipX = true;
        }
        else if (_direction > 0)
        {
            _flipFace.flipX = false;
        }
    }

    private void Dock()
    {
        if (_docked) return;

        _docked = true;
        _speed = 3f;
        transform.SetParent(null);
        transform.rotation = Quaternion.identity;
        transform.position = new Vector2(transform.position.x, 0f);

        Debug.Log("Player docked.");
    }

    private void UnDock()
    {
        if (!_docked) return;

        _docked = false;
        _speed = 0.5f;

        if (_parent != null)
        {
            transform.SetParent(_parent);
            transform.localPosition = new Vector2(0, 0.08f);
        }
        else
        {
            Debug.LogError("Parent not set!");
        }

        Debug.Log("Player undocked.");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"OnTriggerEnter2D with {other.name}, tag: {other.tag}");
        if (other.CompareTag("DockZone"))
        {
            _isInDockZone = true;
            Debug.Log("Player entered DockZone.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log($"OnTriggerExit2D with {other.name}, tag: {other.tag}");
        if (other.CompareTag("DockZone"))
        {
            _isInDockZone = false;
            Debug.Log("Player left DockZone.");
        }
    }
}
