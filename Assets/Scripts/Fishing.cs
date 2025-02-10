using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Fishing : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private MoveHook moveHook;
    
    private void Start()
    {
        ToggleMiniGame.OnCath.AddListener(() =>
        {
            moveHook.enabled = false;


        });
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.TryGetComponent<FlockingFish>(out FlockingFish FlockingFish))
        {
            ToggleMiniGame.StartFish();
            sprite.enabled = false;
            collision.gameObject.transform.SetParent(gameObject.transform);
            Debug.Log("1");

        }
        else
        {
           sprite.enabled=true;

        }
    }
}
