using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickupable : MonoBehaviour
{
    public string itemName;
    public string itemDescription;
    public int amount = 1;

    public AudioClip pickupSound;

    public void PickUp() {
        Item item = new Item();
        item.itemName = itemName;
        item.itemDescription = itemDescription;
        item.amount = amount;
        Inventory.Instance.addItem(item);
        if(pickupSound != null) {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        }

        Destroy(gameObject);
    }

    private bool isPlayerNear() {
        GameObject player = GameObject.Find("Player");
        return Vector3.Distance(player.transform.position, transform.position) < 1.5f;
    }
    
    void Start() {  
        if(pickupSound == null) {
            Resources.Load<AudioClip>("Sounds/item_collect");
        }
    }

    // Update is called once per frame
    void Update() {
        if(isPlayerNear() && Input.GetKeyDown(KeyCode.E)) {
            PickUp();
        }
    }
}
