using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepController : MonoBehaviour {
    private float latestDirectionChangeTime;
    private readonly float directionChangeTime = 2f;
    private float characterVelocity = 0.2f;

    private Animator animator;
    private float random;
    float rnd1;
    float rnd2;


    void Start() {
        latestDirectionChangeTime = 0f;
        calcuateNewMovementVector();
        animator = GetComponent<Animator>();
    }

    void calcuateNewMovementVector() {
        //create a random direction vector with the magnitude of 1, later multiply it with the velocity of the enemy

        // Decides if the sheep should move or stand still
        random = Random.Range(0f, 1f);
        // Decides where the sheep needs to move
        rnd1 = Random.value > 0.5 ? Random.value : -Random.value;
        rnd2 = Random.value > 0.5 ? Random.value : -Random.value;

    }

    void Update() {
        //if the changeTime was reached, calculate a new movement vector
        if (Time.time - latestDirectionChangeTime > directionChangeTime) {
            latestDirectionChangeTime = Time.time;
            calcuateNewMovementVector();
        }

        // move enemy: 
        // 60% chance of standing still
        if (random > 0.6) {
            if (rnd1 < 0 && rnd1 < rnd2) {
                animator.SetBool("WalkingLeft", true);
                animator.SetBool("WalkingRight", false);
                animator.SetBool("WalkingUp", false);
                animator.SetBool("WalkingDown", false);
            } else if (rnd1 > 0 && rnd1 > rnd2) {
                animator.SetBool("WalkingRight", true);
                animator.SetBool("WalkingUp", false);
                animator.SetBool("WalkingDown", false);
                animator.SetBool("WalkingLeft", false);
            } else if(rnd2 < 0 && rnd2 < rnd1) {
                animator.SetBool("WalkingDown", true);
                animator.SetBool("WalkingRight", false);
                animator.SetBool("WalkingUp", false);
                animator.SetBool("WalkingLeft", false);
            } else if (rnd2 > 0 && rnd2 > rnd1) {
                animator.SetBool("WalkingUp", true);
                animator.SetBool("WalkingDown", false);
                animator.SetBool("WalkingRight", false);
                animator.SetBool("WalkingLeft", false);
            }

            transform.position = Vector2.MoveTowards(transform.position,
                new Vector2(transform.position.x + rnd1, transform.position.y + rnd2),
                Time.deltaTime * characterVelocity);



        } else {
            animator.SetBool("WalkingLeft", false);
            animator.SetBool("WalkingRight", false);
            animator.SetBool("WalkingUp", false);
            animator.SetBool("WalkingDown", false);
        }
    }
}


