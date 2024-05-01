using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SignalLight : MonoBehaviour
{
    public List<GameObject> requiredSignals = new List<GameObject>();

    private bool on;

    public Color onColor = Color.green;
    public Color offColor = Color.red;

    void Start() {
        
        if(requiredSignals.Count == 0  || requiredSignals == null) {
            gameObject.GetComponent<Renderer>().material.color = onColor;
        }
    }

    private void TurnOn() {
        on = true;
        gameObject.GetComponent<SpriteRenderer>().color = onColor;
    }

    private void TurnOff() {
        on = false;
        gameObject.GetComponent<SpriteRenderer>().color = offColor;
    }

    void Update() {
        if(requiredSignals.Count == 0 || requiredSignals == null) {
            return;
        }

        if(on && requiredSignals.Where(signal => signal.GetComponent<Signaler>().GetSignal() == false).ToList().Count > 0) {
            TurnOff();
        }
        else if(requiredSignals.Where(signal => signal.GetComponent<Signaler>().GetSignal() == false).ToList().Count == 0) {
            TurnOn();
        }
    }
}
