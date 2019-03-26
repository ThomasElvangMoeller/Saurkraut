using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed = 3.0f;
    public int facingDirection;
    private Rigidbody2D rg2D;

    // Start is called before the first frame update
    void Start()
    {
        rg2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");
        Movement(horizontalMovement, verticalMovement);
        
    }

    public void Movement(float horizontal, float vertical)
    {
        rg2D.velocity = new Vector2(horizontal * Speed, vertical * Speed);

        
    }

    public void FlipOnVertival()
    {

    }

    public void FlipOnHorizontal()
    {

    }
}
