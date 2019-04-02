using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float Speed = 3.0f;
    public int faceDirection;
    public Sprite[] PlayerSprites = new Sprite[3];

    private Rigidbody2D rg2D;
    private SpriteRenderer mRenderer;

    // Start is called before the first frame update
    void Start()
    {
        rg2D = GetComponent<Rigidbody2D>();
       mRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Movement and facing direction
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");
        Movement(horizontalMovement, verticalMovement);
        HandleSprite(new Vector2(horizontalMovement, verticalMovement));
        
        // Inventory
        //if(Input.GetKeyDown("Inventory"))
        //{

        //}

    }


    public void Movement(float horizontal, float vertical)
    {
        rg2D.velocity = new Vector2(horizontal * Speed, vertical * Speed);

        
    }

    public void HandleSprite(Vector2 direction) {
        if(direction.y > 0) {
            mRenderer.sprite = PlayerSprites[2];
            mRenderer.flipX = false;
        }else if(direction.x > 0) {
            mRenderer.sprite = PlayerSprites[1];
            mRenderer.flipX = false;
        }else if(direction.x < 0) {
            mRenderer.sprite = PlayerSprites[1];
            mRenderer.flipX = true;
        } else if(direction.y < 0) {
            mRenderer.sprite = PlayerSprites[0];
            mRenderer.flipX = false;
        }
    }

    public void FlipOnVertival(float horizontal, float vertical)
    {
        if(vertical > 0)
        {
            faceDirection = 0;
        }
        else if (vertical < 0)
        {
            faceDirection = 2;
        }
        else if (horizontal > 0)
        {
            faceDirection = 1;
        }
        else
        {
            faceDirection = 3;
        }

        //Vector3 theScale = transform.localScale;
        //transform.localScale = theScale;
    }

    public void FlipOnHorizontal()
    {


        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
