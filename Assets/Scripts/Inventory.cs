using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    public static Inventory Instance;

    public void addItem(Item item) {
        if(items.Contains(item)) {
            items.Find(i => i == item).amount++;
        }
        else {
            items.Add(item);
        }
    }
    
    public void removeItem(Item item) {
        if(items.Contains(item)) {
            items.Find(i => i == item).amount--;
            if(items.Find(i => i == item).amount == 0) {
                items.Remove(item);
            }
        }
    }
    
    public bool hasItem(string item) {
        return items.Exists(i => i.itemName == item);
        
    }

    public int getAmountOfItem(string item) {
        if(!hasItem(item)) {
            return 0;
        }

        return items.Find(i => i.itemName == item).amount;
    }

    private void displayItems() {
        foreach(Item item in items) {
            Debug.Log(item.itemName + " " + item.amount);
        }
    }
    
    void Start() {
        Instance = this;
    }

    // Update is called once per frame
    void Update() {
        if(Input.GetKeyDown(KeyCode.I)) {
            displayItems();
        }
    }
}
