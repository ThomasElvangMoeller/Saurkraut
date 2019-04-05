using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float Speed = 3.0f;
    public int faceDirection;
    public Sprite[] PlayerSprites = new Sprite[3];

    private Rigidbody2D rg2D;
    private SpriteRenderer mRenderer;
    private bool showingInventory = false;
    private List<ItemStack> Inventory = new List<ItemStack>(GameController.Config.DEFAULT_PLAYER_INVENTORY_SIZE);
    private Transform InteractionPoint;
    private Vector2 oldDirection = Vector2.zero;

    public delegate void InventoryChangedEventHandler(object sender, EventArgs args);
    public event InventoryChangedEventHandler InventoryChanged;

    private void NotifyInventoryChanged() {
        if(InventoryChanged != null) {
            InventoryChanged(this, new EventArgs());
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rg2D = GetComponent<Rigidbody2D>();
        mRenderer = GetComponent<SpriteRenderer>();
        InteractionPoint = new GameObject().transform;
        InteractionPoint.parent = transform;
        InteractionPoint.position = new Vector2(0, -.2f);
        InteractionPoint.name = "InteractionPoint";

        Filter2D.layerMask = LayerMask.GetMask("Nature");

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Movement and facing direction
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");

        Vector2 dir = new Vector2(horizontalMovement, verticalMovement);

        Movement(horizontalMovement, verticalMovement);
        HandleSprite(dir);
        HandleInteractionPoint(dir);
        
        // Inventory
        if(Input.GetButtonDown("Inventory"))
        {
            foreach (var item in Inventory) {
                Debug.Log(item);
            }
            if(showingInventory == false)
            {
                showingInventory = true;
                SceneManager.LoadScene(0, LoadSceneMode.Additive);
                //Application.LoadLevelAdditive(0);
                
            } else
            {
                showingInventory = false;
                SceneManager.UnloadSceneAsync(0);
                //Application.UnloadLevel(0);
                
            }
        }

        // Interact
        if (Input.GetButtonDown("Jump")) {
            Interact(dir);
        }

    }


    public bool MoveItem(int itemToMoveIndex, int newItemIndex, List<ItemStack> inventory) {
        return MoveItem(itemToMoveIndex, newItemIndex, inventory, inventory);
    }

    public bool MoveItem(int itemToMoveIndex, int newItemIndex, List<ItemStack> invFrom, List<ItemStack> invTo) {
        if (itemToMoveIndex >= 0 && itemToMoveIndex < invFrom.Capacity) {
            ItemStack item1 = invFrom[itemToMoveIndex];

            if (newItemIndex >= 0 && newItemIndex < invTo.Capacity) {
                ItemStack item2 = invTo[newItemIndex];

                invFrom[itemToMoveIndex] = item2;
                invTo[newItemIndex] = item1;
                NotifyInventoryChanged();
                return true;
            }
        }
        return false;
    }

    public bool AddToInventory(ItemStack itemStack) {

        if(Inventory.Count < GameController.Config.DEFAULT_PLAYER_INVENTORY_SIZE) {
            foreach (ItemStack item in Inventory) {
                if(item.item == itemStack.item) {
                    item.itemAmount += itemStack.itemAmount;
                    return true;
                }
            }
            Inventory.Add(itemStack);
            NotifyInventoryChanged();
            return true;
        }

        return false;
    }


    public void Movement(float horizontal, float vertical)
    {
        rg2D.velocity = new Vector2(horizontal * Speed, vertical * Speed);
    }



    public void HandleInteractionPoint(Vector2 direction) {

        if (direction != oldDirection) {

            if (direction.y > 0) {
                InteractionPoint.localPosition = new Vector2(0, .2f);
            } else if (direction.x > 0) {
                InteractionPoint.localPosition = new Vector2(.2f, 0);
            } else if (direction.x < 0) {
                InteractionPoint.localPosition = new Vector2(-.2f, 0);
            } else if (direction.y < 0) {
                InteractionPoint.localPosition = new Vector2(0, -.2f);
            }
            oldDirection = direction;
        }
    }

    private ContactFilter2D Filter2D = new ContactFilter2D();

    public void Interact(Vector2 direction) {
        RaycastHit2D[] hit2D = new RaycastHit2D[1];
        Physics2D.CircleCast(InteractionPoint.position, 0.05f, direction, Filter2D, hit2D);

        ResourcePoint resource = hit2D[0].collider.gameObject.GetComponent<ResourcePoint>();
        //TODO find måde at checke om collided har component ResourcePoint 
        if(resource != null) {
            resource.OnHit(this);
        }
    }

    public void HandleSprite(Vector2 direction) {
        if(direction.y > 0) {
            mRenderer.sprite = PlayerSprites[2];
            mRenderer.flipX = false;
        }else if(direction.x > 0) {
            mRenderer.sprite = PlayerSprites[1];
            mRenderer.flipX = false;
        } else if(direction.x < 0) {
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
