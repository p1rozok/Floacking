using System;
using UnityEngine;

public class Hook : MonoBehaviour
{
    [SerializeField] private float waterLevel = 0f;
    [SerializeField] private float floatStrength = 2f;
    [SerializeField] private float maxSinkDepth = 0.1f;
    [SerializeField] private PresenterBait presenterBait;

    private float depthBelowWater;
    private Rigidbody2D rb;
    private bool isStopped = false;

   



    void Start()
    { 
        rb = GetComponent<Rigidbody2D>();
    }


    void FixedUpdate()
    {
        if (isStopped)
            return;

        depthBelowWater = waterLevel - transform.position.y;
        if (depthBelowWater > 0 && depthBelowWater <= maxSinkDepth)
        {
            float force = Mathf.Abs(Physics2D.gravity.y) * floatStrength * (depthBelowWater / maxSinkDepth);
            rb.AddForce(Vector2.up * force);
        }
        else if (depthBelowWater > maxSinkDepth)
        {
            Vector3 newPosition = transform.position;
            newPosition.y = waterLevel - maxSinkDepth;
            transform.position = newPosition;

            rb.velocity = Vector2.zero;
        }
    }
 
    public void SetBait(Bait bait)
    {
        presenterBait.PresenterImage(bait.Sprite);
    }

}
