using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointSystem : MonoBehaviour {
    private List<GameObject> checkpoints = new List<GameObject>();
    private GameObject player;
    [HideInInspector]
    public GameObject currentCheckpoint;
    
    public static CheckpointSystem Instance;
    void Start() {
        Instance = this;
        checkpoints.AddRange(GameObject.FindGameObjectsWithTag("Checkpoint"));
        player = GameObject.FindGameObjectWithTag("Player");
    }

    
    void Update() {
        if(Input.GetKeyDown(KeyCode.R)) {
            if(currentCheckpoint != null) {
                player.transform.position = currentCheckpoint.transform.position;
            }
        }
    }
}
