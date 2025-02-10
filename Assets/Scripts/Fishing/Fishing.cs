using Unity.Mathematics;
using UnityEngine;

public class Fishing : MonoBehaviour
{
    private GameObject call;
    private FlockingFish flockingFish;
   

    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private MoveHook moveHook;
    [SerializeField] private BoxCollider2D boxCollider;
    private void Start()
    {
        ToggleMiniGame.OnCath.AddListener(() =>
        {
            moveHook.enabled = false;
            boxCollider.enabled = false;
            
            
        });

        ToggleMiniGame.OnWin.AddListener(() =>
        {
            if (flockingFish != null)
            {
                flockingFish.RemoveFish();
               
            }
            boxCollider.enabled = true;
        });
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<FlockingFish>(out FlockingFish collidedFish))
        {
            if (ToggleMiniGame.OnCath != null)
            {
                call = collision.gameObject;
                flockingFish = collidedFish;

                if (transform.childCount<1)
                {
                    collision.gameObject.transform.SetParent(gameObject.transform);
                    collision.gameObject.transform.localPosition = Vector3.zero;
                    collision.gameObject.transform.rotation = quaternion.identity;
                    collision.gameObject.transform.Rotate(0, 0, 90);
                    collidedFish.enabled = false;
                    ToggleMiniGame.StartFish();
                    sprite.enabled = false;
                    collision.gameObject.GetComponent<Collider2D>().enabled=false;
                }
                
            }
        }
        else
        {
            sprite.enabled = true;
        }
    }
}