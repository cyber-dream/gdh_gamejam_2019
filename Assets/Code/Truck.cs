using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Truck : MonoBehaviour {
    private TargetJoint2D target;
    public Vector2 EndPosition;

    public Vector2 TargetPosition; 


    // Start is called before the first frame update
    void Start() {
        target = GetComponent<TargetJoint2D>();
        target.target = TargetPosition;
    }

    public void SetTargetPosition(Vector2 position) {
        TargetPosition = position;
        
        if (target) target.target = TargetPosition;
    }

}