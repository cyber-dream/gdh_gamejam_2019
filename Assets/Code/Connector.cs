using System;
using UnityEngine;

public class Connector : MonoBehaviour {
    
    public TubeController Controller;
    public Incubator Incubator;
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject && other.gameObject == Controller.Target) {
            Controller.incubatorConnectedEvent.Invoke(Incubator);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        
        if (other.gameObject && other.gameObject == Controller.Target) {
            Controller.incubatorDisconnectedEvent.Invoke(Incubator);
        }
        
    }
}