using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : Signaler {

    public AudioClip triggerSound;
    public List<string> allowedTriggerTags = new List<string>();

    private void OnTriggerEnter2D(Collider2D collision) {
        if(allowedTriggerTags.Contains(collision.gameObject.tag)) {
            AudioSource.PlayClipAtPoint(triggerSound, transform.position);
            Signal = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if(allowedTriggerTags.Contains(collision.gameObject.tag)) {
            Signal = false;
        }
    }

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
