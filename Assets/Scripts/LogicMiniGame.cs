
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LogicMiniGame : MonoBehaviour
{
    [SerializeField] private Transform topPivot;
    [SerializeField] private Transform bottomPivot;
    [SerializeField] private Transform fish;
    [SerializeField] private float timerMultiplyer=3f;
    [SerializeField] private float smoothMotion = 1f;
    
    [SerializeField] private Transform hook;
    [SerializeField] private float hookSize = 0.1f;
    [SerializeField] private float hookPower = 0.5f;
    [SerializeField] private float hookPullPower=0.01f;
    [SerializeField] private float hookGravityPower = 0.005f;
    [SerializeField] private float hookDegrationPullPower = 0.1f;

    [SerializeField] private RectTransform progressBarConteiner;
    
    [SerializeField] private FishingTrigger fishingTrigger;
    [SerializeField] private MoveHook moveHook;

    private float _fishPosition;
    private float _fishDestination;
    private float _fishSpeed;
    private float _fishTimer;

    private float _hookPosition;
    private float _hookProgress;
    private float _hookPullVelocity;

    private bool _isStartMiniGame;
    


    private void Start()
    {
        gameObject.SetActive(false);

        ToggleMiniGame.OnWin.AddListener(() =>
        {
            gameObject.SetActive(false);
        });

        ToggleMiniGame.OnCath.AddListener(() =>
        {
            gameObject.SetActive(true);

        });
    }

    private void Update()
    {

            FishMover();
            HookMover();
            ProgressBarCheck();
        
    }

    private void ProgressBarCheck()
    {
        Vector3 localScale=progressBarConteiner.localScale;
  
        localScale.y =_hookProgress;
        progressBarConteiner.localScale = localScale;

        float min = _hookPosition - hookSize / 2;
        float max = _hookPosition + hookSize / 2;

        if (min < _fishPosition && _fishPosition < max)
        {
            
            _hookProgress += hookPower * Time.deltaTime;
        }
        else 
        {
            
            _hookProgress-=hookDegrationPullPower * Time.deltaTime;
        }
      //  _hookProgress = Utils.Map(_hookProgress, 1, 0, moveHook.DistanseToHook,0);
       
        _hookProgress = Mathf.Clamp(_hookProgress, 0, 1);
       
        if (_hookProgress >= 1)
            ToggleMiniGame.EndFish();


    }

    private void HookMover()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            _hookPullVelocity += hookPullPower * Time.deltaTime;
        }
        _hookPullVelocity -= hookGravityPower * Time.deltaTime;
        _hookPosition += _hookPullVelocity;

        if (_hookPosition-hookSize/2<=0 &&_hookPullVelocity<0f)
        {
            _hookPullVelocity = 0;

        }
        if (_hookPosition+hookSize/2 >=1 && _hookPullVelocity > 0f)
        {
            _hookPullVelocity = 0;

        }
        _hookPosition=Mathf.Clamp(_hookPosition, hookSize/2, 1-hookSize/2);
        hook.position=Vector3.Lerp(bottomPivot.position, topPivot.position, _hookPosition);

    }

    private void FishMover()
    {
        _fishTimer-=Time.deltaTime;
        if (_fishTimer < 0)
        {
            _fishTimer = Random.value * timerMultiplyer;
            _fishDestination = Random.value;
        }
        _fishPosition=Mathf.SmoothDamp(_fishPosition, _fishDestination,ref _fishSpeed,smoothMotion);
        fish.position = Vector3.Lerp(bottomPivot.position  , topPivot.position, _fishPosition);
    }


  
}
