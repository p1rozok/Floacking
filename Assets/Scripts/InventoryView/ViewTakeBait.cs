using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent (typeof(BoxCollider2D))]
public class ViewTakeBait : MonoBehaviour
{
    [SerializeField] private Bait bait;
    [SerializeField] private Inventory inventory;

    private  SpriteRenderer spriteRenderer;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite=bait.Sprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<MoveBoat>(out MoveBoat controller))
        {
            inventory.AddSlot(bait);
            Destroy(gameObject);
        }
    }
}
