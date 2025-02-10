using UnityEngine;

public class DistanceChecker : MonoBehaviour
{
    [SerializeField] private Hook hook;
    [SerializeField] private LogicMiniGame logicMiniGame;

    private Vector2 _transformFish;
    private float _distansToHook;
    private float _distansHookNormalized;
    private bool _isCatch=false;
    
    void Start()
    {
        ToggleMiniGame.OnCath.AddListener(() =>
        {
            
            _transformFish = transform.position;
            _isCatch = true;
           
       

        });

        ToggleMiniGame.OnWin.AddListener(() =>
        {
            transform.position = Vector3.zero;
            _isCatch =false ;
            

        });
    }

    void Update()
    {   
        if (_isCatch)
        {
            MoverBaitOnMiniGame();
        }

    }

   

    private void MoverBaitOnMiniGame()
    {
        transform.position = Vector3.Lerp(_transformFish, hook.transform.position, LogicMiniGame.GetHookProgress());
    }
}
