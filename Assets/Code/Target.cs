using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour {
    public TubeController Controller;


    private void OnMouseDown() {
        Controller.TargetDownEvent.Invoke();
    }

    private void OnMouseUp() {
        Controller.TargetUpEvent.Invoke();
    }
}