using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public bool Docked => _docked;

    [SerializeField] private float _speed = 0.5f;
    public Joystick _joystick;
    

    private float _direction;
    private SpriteRenderer _flipFace;
    private bool _docked = false;

    private Transform _parent;
    private bool _isInDockZone = false;

    private Transform _baitTransform;

    private float mover; 
    

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
        mover = _joystick.Horizontal;
        
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
        }

        
    }

    
    private void MovePlayer()
    {
        transform.localPosition = new Vector3(
            Mathf.Clamp(transform.localPosition.x + mover * _speed * Time.deltaTime, -0.20f, 0.35f),
            transform.localPosition.y,
            transform.localPosition.z
        );
        
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
        if (_direction < 0 || mover < 0)
        {
            _flipFace.flipX = true;
        }
        else if (_direction > 0 || mover > 0)
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
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DockZone"))
        {
            _isInDockZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("DockZone"))
        {
            _isInDockZone = false;
        }
    }
}
