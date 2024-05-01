using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MovingBlock : MonoBehaviour {

    public LayerMask groundLayer;

    public bool Signal = false;

    private void OnCollisionEnter2D(Collision2D collision) {
        //check if close to wall
        if(collision.gameObject.CompareTag("Player")) {
            //get collision direction
            float direction = collision.transform.position.x - transform.position.x;
            if(Math.Abs(direction) > 1) {

            }
        }
    }

    private void AttachedMovement() {
        //attach the block to the character as long as the interaction button is pressed
        //we will simulate the attachment by moving the block the same as the player moves
        //this will make the block follow the player

        //get the direction of the player movement and store it into a variable called 'isPlayerMovingLeft'
        bool isPlayerMovingLeft = CharacterController.Instance.GetVelocity().x < 0;

        //get the direction of the rock position in relation to the player and store it into a variable called 'isRockLeft'
        bool isRockLeft = transform.position.x < CharacterController.Instance.transform.position.x;

        //if both are true or both are false, return
        if(isPlayerMovingLeft == isRockLeft) {
            CharacterController.Instance.carrying = false;
            return;
        }

        GetComponent<Rigidbody2D>().velocity = CharacterController.Instance.GetVelocity() * new Vector2(1.15f, 0);
        CharacterController.Instance.carrying = true;

        //give the rock some extra velocity towards the player if it is too far away
        if(distanceFromPlayer() > 1.3f) {
            GetComponent<Rigidbody2D>().velocity = CharacterController.Instance.GetVelocity() * new Vector2(1.15f, 0) * 1.5f;
        }
    }

    private float distanceFromPlayer() {
        return Vector2.Distance(CharacterController.Instance.transform.position, this.transform.position);
    }

    void Start() {
        
    }

    void Update() {
        if(distanceFromPlayer() < 1.5f || (CharacterController.Instance.carrying && distanceFromPlayer() < 2.0f)) {
            if(CharacterController.Instance.isInteracting) {

                float direction = GameObject.Find("Player").transform.position.x - transform.position.x;
                if(Math.Abs(direction) > 1) {
                    AttachedMovement();
                }
            }
        }
    }
}
