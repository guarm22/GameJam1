using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraContoller : MonoBehaviour
{
    private Transform player;
    private float smoothSpeed = 0.2f;
    private Vector3 offset = new Vector3(0, 0, -30);
    private Vector3 velocity = Vector3.zero;
    void Start() {
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update() {
        Vector3 playerPosition = player.position + offset;
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, playerPosition, ref velocity, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
