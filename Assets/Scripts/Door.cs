using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Door : MonoBehaviour
{
    public List<GameObject> requiredSignals = new List<GameObject>();

    private bool opened;

    void Start() {
        if(requiredSignals.Count == 0) {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update() {
        if(opened && requiredSignals.Where(signal => signal.GetComponent<Signaler>().GetSignal() == false).ToList().Count > 0) {
            CloseDoor();
        }
        else if(requiredSignals.Where(signal => signal.GetComponent<Signaler>().GetSignal() == false).ToList().Count == 0) {
            OpenDoor();
        }
        DrawDebugLines();
    }

    private void OpenDoor() {
        if(!opened) {
            opened = true;
            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y + 2f, gameObject.transform.localPosition.z);
        }
    }

    private void CloseDoor() {
        if(opened) {
            opened = false;
            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y - 2f, gameObject.transform.localPosition.z);
        }
    }

    private void DrawDebugLines() {
        foreach(GameObject signal in requiredSignals) {
            Debug.DrawLine(gameObject.transform.position, signal.transform.position, Color.red);
        }
    }
}
