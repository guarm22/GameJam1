using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : Signaler
{
    private GameObject player;
    public bool Toggle = true;

    public float buttonSignalDuration = 1.5f;

    // Start is called before the first frame update
    void Start() {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update() {
        if(!Toggle && Signal) {
            return;
        }

        if(isPlayerNear()) {

            if(Input.GetKeyDown(KeyCode.E)) {
                
                if(!Toggle) {
                    StartCoroutine(ToggleButton());
                }
                else {
                    Signal = !Signal;
                }

            }

        }
        
        
    }

    public IEnumerator ToggleButton() {
        Signal = true;
        yield return new WaitForSeconds(buttonSignalDuration);
        Signal = false;
    }

    private bool isPlayerNear() {
        return Vector3.Distance(player.transform.position, transform.position) < 1.5f;
    }
}
