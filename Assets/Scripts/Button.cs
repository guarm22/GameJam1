using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : Signaler
{
    private GameObject player;
    public bool Toggle = true;
    public AudioClip buttonOn;
    public AudioClip buttonOff;

    public Sprite onSprite;
    public Sprite offSprite;

    public float buttonSignalDuration = 1.5f;

    // Start is called before the first frame update
    void Start() {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update() {
        if(!Toggle && GetSignal()) {
            return;
        }

        if(isPlayerNear()) {

            if(Input.GetKeyDown(KeyCode.E)) {
                
                if(!Toggle) {
                    StartCoroutine(ToggleButton());
                }
                else {
                    ChangeSignal();
                }
            }
        }
    }

    private void ChangeSignal() {
        SetSignal(!GetSignal());

        if(GetSignal()) {
            GetComponent<SpriteRenderer>().sprite = onSprite;
            AudioSource.PlayClipAtPoint(buttonOn, transform.position);
        }
        else {
            GetComponent<SpriteRenderer>().sprite = offSprite;
            AudioSource.PlayClipAtPoint(buttonOff, transform.position);
        }
    }

    public IEnumerator ToggleButton() {
        ChangeSignal();
        yield return new WaitForSeconds(buttonSignalDuration);
        ChangeSignal();
    }

    private bool isPlayerNear() {
        return Vector3.Distance(player.transform.position, transform.position) < 1.5f;
    }
}
