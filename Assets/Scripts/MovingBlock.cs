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
                if(CharacterController.Instance.isInteracting) {
                    MoveObject(direction, 1.25f);
                    return;
                }

                MoveObject(-direction, 1f);
            }
        }
    }

    private void MoveObject(float direction, float distance) {
        StartCoroutine(SmoothMovement(direction, distance));
    }

    private IEnumerator SmoothMovement(float direction, float distance, float duration=0.3f) {
        float elapsedTime = 0;
        Vector3 startingPos = transform.position;
        Vector3 endPos = transform.position + new Vector3(direction * distance, 0f, 0f);

        while(elapsedTime < duration) {

             RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(direction, 0), distance, groundLayer);

            // If the ray hits something on the ground layer, stop movement
            if (hit.collider != null) {
                Renderer renderer = gameObject.GetComponent<Renderer>();
                Vector3 size = renderer.bounds.size;
                distance = hit.distance - size.x / 2f;
                StartCoroutine(SmoothMovement(direction, distance));
                endPos = transform.position;
                break;
            }

            transform.position = Vector3.Lerp(startingPos, endPos, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = endPos;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
