using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MoveHook : MonoBehaviour
{
    [SerializeField] private float distance;
    [SerializeField] private Hook hook;
    [SerializeField] private float speed = 1;

    private float _time = 4f;
    private float _directionHorizontal;
    private float _directionVertical;
    private float _angle;
    private Vector2 _newPosition;
    private Rigidbody2D _rb;
    private bool _enabled = false;
    private List<Vector3> _transformList = new List<Vector3>();
    private float _distansToHook;

    private Joystick _joystick;

    public float DistanseToHook => _distansToHook;
    public Hook Hook => hook;

    public void Initialize(Joystick joystick)
    {
        _joystick = joystick;
        InitializeHook();
    }

    private void InitializeHook()
    {
        if (_joystick == null)
        {
           
            enabled = false;
            return;
        }

        ToggleMiniGame.OnCath.AddListener(() =>
        {
            _distansToHook = Vector2.Distance(hook.transform.position, _newPosition);
            _distansToHook = Mathf.Clamp01(_distansToHook);
        });

        _rb = GetComponent<Rigidbody2D>();
        StartCoroutine(WaitTime(_time));
    }

    private void Update()
    {
        if (!_enabled)
            return;

        HarvestInput();
        CalculatedAngle();
    }

    private void FixedUpdate()
    {
        if (_enabled)
            MoverHook();
        else
            FollowParents();
    }

    private void FollowParents()
    {
        _rb.MovePosition(transform.parent.position);
    }
    private void HarvestInput()
    {
        _directionHorizontal = _joystick.Horizontal;
        _directionVertical = _joystick.Vertical;
    }

    private void MoverHook()
    {
        Vector2 movement = new Vector2(_directionHorizontal, _directionVertical).normalized;
        _newPosition = _rb.position + movement * speed * Time.fixedDeltaTime;

        var currentDistance = Vector2.Distance(hook.transform.position, _newPosition);
        if (currentDistance <= distance && _angle >= 90)
        {
            _rb.MovePosition(_newPosition);
            _transformList.Add(transform.position);
        }
    }

    private void CalculatedAngle()
    {
        var midlleVector = _newPosition - (Vector2)hook.transform.position;
        _angle = Vector2.Angle(midlleVector, hook.transform.up);
    }
    private IEnumerator WaitTime(float time)
    {
        yield return new WaitForSeconds(time);
        _enabled = true;
    }
}