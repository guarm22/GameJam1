using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Signaler : MonoBehaviour {
    [HideInInspector]
    private bool Signal;    
    private bool InvertSignal;

    public bool GetSignal() {
        if(InvertSignal) {
            return !Signal;
        }
        return Signal;
    }

    public void SetSignal(bool signal) {
        Signal = signal;
    }
}
