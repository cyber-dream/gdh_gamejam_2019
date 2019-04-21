using UnityEngine;
using UnityEngine.Events;

public class TubeController : MonoBehaviour {
    
    public class IncubatorEvent : UnityEvent<Incubator> {}
    public UnityEvent TargetDownEvent = new UnityEvent();
    public UnityEvent TargetUpEvent = new UnityEvent();
    public IncubatorEvent  incubatorConnectedEvent = new IncubatorEvent ();
    public IncubatorEvent  incubatorDisconnectedEvent = new IncubatorEvent ();
    
    public enum TargetStateType {
        Move,
        Idle
    }
    
    public GameObject Target;
    public TargetStateType TargetState = TargetStateType.Idle;
    public float ConnectorBreakForce = 100f;
    public float ConnectorMaxForce = 5000f;
    
    
    // Components
    private TargetJoint2D TargetTargetJoint;
    private Camera mainCamera;

    void Start() {
        TargetTargetJoint = Target.GetComponent<TargetJoint2D>();
        
        TargetDownEvent.AddListener(OnTargetDown);
        TargetUpEvent.AddListener(OnTargetUp);
        incubatorConnectedEvent.AddListener(OnTubeConnected);
        incubatorDisconnectedEvent.AddListener(OnTubeDisconnected);
        
        mainCamera = Camera.main;
    }

    private void OnTargetDown() {
        TargetState = TargetStateType.Move;
        TargetTargetJoint.enabled = true;
    }

    private void OnTargetUp() {
        TargetState = TargetStateType.Idle;
        TargetTargetJoint.enabled = false;
    }

    private void OnTubeConnected(Incubator target) {

        Connector connector = target.Connector;
        TargetJoint2D joint = Target.AddComponent<TargetJoint2D>();
        joint.breakForce = ConnectorBreakForce;
        joint.maxForce = ConnectorMaxForce;
        joint.target = connector.transform.position;
        target.TubeConnectedEvent.Invoke();
    }

    private void OnTubeDisconnected(Incubator target) {
        target.TubeDisconnectedEvent.Invoke();
    }

    void Update() {
        if (TargetState == TargetStateType.Move) {
            TargetTargetJoint.target = CalculateMousePosition();
        }
    }

    private Vector2 CalculateMousePosition() {
        return mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }
    
}