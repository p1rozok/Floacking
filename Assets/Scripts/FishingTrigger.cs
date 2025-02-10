using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class FishingTrigger : MonoBehaviour
{
    public event UnityAction OnCollectebls;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<FlockingFish>(out FlockingFish FlockingFish))
        {
            OnCollectebls?.Invoke();
            Destroy(collision.gameObject);
        }
    }
}
