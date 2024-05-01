using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JellyfishGroup : MonoBehaviour
{
    public List<GameObject> jellyfishes = new List<GameObject>();
    public float floatDuration = 5f;
    public float yMove = 2f;
    private bool isFloating = false;

    // Start is called before the first frame update
    void Start() {
        foreach (Transform child in transform) {
            jellyfishes.Add(child.gameObject);
        }  
        StartCoroutine(SimulateWaterMovement());
    }

    // Update is called once per frame
    void Update() {
        if(Input.GetKeyDown(KeyCode.X) && !isFloating && Inventory.Instance.getAmountOfItem("Orb")>0) {
            //Stop simulating water movement and start jellyfish movement
            StopAllCoroutines();
            StartCoroutine(JellyfishMove());
        }
    }

    private IEnumerator SimulateWaterMovement() {

        while(true) {
            foreach(GameObject jellyfish in jellyfishes) {
                StartCoroutine(SmoothMove(jellyfish, 0.5f, 1.5f));
            }

            yield return new WaitForSeconds(1.5f);

            foreach(GameObject jellyfish in jellyfishes) {
                StartCoroutine(SmoothMove(jellyfish, -0.5f, 1.5f));
            }

            yield return new WaitForSeconds(1.5f);
        }
    }

    private IEnumerator JellyfishMove() {
        isFloating = true;
        foreach(GameObject jellyfish in jellyfishes) {
            StartCoroutine(SmoothMove(jellyfish, yMove, 1f));
        }

        yield return new WaitForSeconds(floatDuration);

        foreach(GameObject jellyfish in jellyfishes) {
            StartCoroutine(SmoothMove(jellyfish, -yMove, 1f));
        }

        yield return new WaitForSeconds(1f);

        StartCoroutine(SimulateWaterMovement());
        isFloating = false;
    }

    private IEnumerator SmoothMove(GameObject obj, float dist, float duration) {
        float elapsedTime = 0;
        Vector3 startingPos = obj.transform.position;
        Vector3 targetPos = obj.transform.position + new Vector3(0, dist, 0);

        while (elapsedTime < duration) {
            obj.transform.position = Vector3.Lerp(startingPos, targetPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        obj.transform.position = targetPos;
    }
}
