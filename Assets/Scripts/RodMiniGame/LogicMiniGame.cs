using UnityEngine;
using UnityEngine.UI;

public class LogicMiniGame : MonoBehaviour
{
    [Header("Настройки движения рыбы")]
    [SerializeField] private Transform topPivot;
    [SerializeField] private Transform bottomPivot;
    [SerializeField] private Transform fish;
    [SerializeField] private float timerMultiplier = 3f;
    [SerializeField] private float smoothMotion = 1f;

    [Header("Настройки движения крючка")]
    [SerializeField] private Transform hook;
    [SerializeField] private float hookSize = 0.1f;
    [SerializeField] private float hookPower = 0.5f;
    [SerializeField] private float hookPullPower = 0.01f;
    [SerializeField] private float hookGravityPower = 0.005f;
    [SerializeField] private float hookDegradationPullPower = 0.1f;

    [Header("UI элементы")]
    [SerializeField] private RectTransform progressBarContainer;
    [SerializeField] private Button pullButton;

    public bool IsActive { get; private set; }

    private float _fishPosition;
    private float _fishDestination;
    private float _fishSpeed;
    private float _fishTimer;

    private float _hookPosition;
    private float _hookProgress;
    private float _hookPullVelocity;
public static float Translater;

    private void Start()
    {
        gameObject.SetActive(false);
        
        if (pullButton != null)
        {
            pullButton.gameObject.SetActive(false);
            pullButton.onClick.AddListener(OnPullButtonPressed);
        }
       

        ToggleMiniGame.OnWin.AddListener(() =>
        {
            _hookProgress = 0;
            EndMiniGame();
            gameObject.SetActive(false);
        });

        ToggleMiniGame.OnCath.AddListener(() =>
        {
            StartMiniGame();
            gameObject.SetActive(true);
        });
    }

    private void Update()
    {
        if (IsActive)
        {
            FishMover();
            HookMover();
            ProgressBarCheck();
            
        }
    }

    public void StartMiniGame()
    {
        IsActive = true;

        if (pullButton != null)
        {
            pullButton.gameObject.SetActive(true);
        }
    }

    public void EndMiniGame()
    {
        IsActive = false;

        if (pullButton != null)
        {
            pullButton.gameObject.SetActive(false);
        }
    }

    private void ProgressBarCheck()
    {
        
        if (progressBarContainer != null)
        {
            float progressBarHeight = Mathf.Clamp01(_hookProgress);
            Vector3 localScale = progressBarContainer.localScale;
            localScale.y = progressBarHeight;
            progressBarContainer.localScale = localScale;
        }
      

        float min = _hookPosition - hookSize / 2;
        float max = _hookPosition + hookSize / 2;

        if (min < _fishPosition && _fishPosition < max)
        {
            _hookProgress += hookPower * Time.deltaTime;
        }
        else
        {
            _hookProgress -= hookDegradationPullPower * Time.deltaTime;
        }

        _hookProgress = Mathf.Clamp(_hookProgress, 0f, 1f);

        Translater = _hookProgress;

        if (_hookProgress >= 1f)
        {
            ToggleMiniGame.EndFish();
        }
    }

    private void HookMover()
    {

        _hookPullVelocity -= hookGravityPower * Time.deltaTime;
        _hookPosition += _hookPullVelocity;


        if (_hookPosition - hookSize / 2 <= 0f && _hookPullVelocity < 0f)
        {
            _hookPullVelocity = 0f;
        }
        if (_hookPosition + hookSize / 2 >= 1f && _hookPullVelocity > 0f)
        {
            _hookPullVelocity = 0f;
        }

        _hookPosition = Mathf.Clamp(_hookPosition, hookSize / 2, 1f - hookSize / 2);
        hook.position = Vector3.Lerp(bottomPivot.position, topPivot.position, _hookPosition);
    }

    private void FishMover()
    {
        _fishTimer -= Time.deltaTime;
        if (_fishTimer < 0f)
        {
            _fishTimer = Random.value * timerMultiplier;
            _fishDestination = Random.value;
        }
        _fishPosition = Mathf.SmoothDamp(_fishPosition, _fishDestination, ref _fishSpeed, smoothMotion);
        fish.position = Vector3.Lerp(bottomPivot.position, topPivot.position, _fishPosition);
    }

  
    public void OnPullButtonPressed()
    {
        if (IsActive)
        {
            _hookPullVelocity += hookPullPower;
        }
    }
public static float GetHookProgress()
    {

        return Translater;
    }
}
