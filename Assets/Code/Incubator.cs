using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class Incubator : MonoBehaviour {
    
    // Service data
    public class HumanEvent : UnityEvent<GameManager.HumanType> { }
  
    public enum IncubatorState {Idle, Working, Filling}

    [Header("Elements")] 
    public SpriteRenderer OxygenLevelSprite;
    public Connector Connector;
    public GameObject TruckPrefab;
    public Truck CurrentTruck;
    
    [Header("State")]
    public IncubatorState State = IncubatorState.Working;
    public float TargetPrecision = 10;
    public float GrowTime = 0;
    public float GrowLevel = 0;
    public GameManager.HumanType CurrentHuman = GameManager.HumanType.None;

    [Header("Settings")] 
    public Vector2 TruckStartPosition;
    
    public Vector2 TruckEndPosition;
    
    [Header("Events")]
    public HumanEvent StartTaskEvent = new HumanEvent();
    public HumanEvent StopTaskEvent = new HumanEvent();
    public UnityEvent TubeConnectedEvent = new UnityEvent();
    public UnityEvent TubeDisconnectedEvent = new UnityEvent();
    public HumanEvent GrowButtonEvent = new HumanEvent();

    [SerializeField] 
    private float _oxygenLevel = 0;
    
    void Start() {
        StartTaskEvent.AddListener(OnTaskStart);
        StopTaskEvent.AddListener(OnTaskStop);
        TubeConnectedEvent.AddListener(OnConnectedTube);
        TubeDisconnectedEvent.AddListener(OnDisconnectedTube);
        GrowButtonEvent.AddListener(OnGrowButtonClick);

        OxygenLevel = 0.2f;
    }

    private void OnGrowButtonClick(GameManager.HumanType type) {
        
       OnTaskStart(type);
    }
    
    private void OnConnectedTube() {
        State = IncubatorState.Filling;
    }

    private void OnDisconnectedTube() {

        if (CurrentHuman != GameManager.HumanType.None) {
            State = IncubatorState.Working;
        } else {
            State = IncubatorState.Idle;
        }
    }
    
    private void OnTaskStart(GameManager.HumanType type) {
        if (State == IncubatorState.Working) return;
  
        CurrentHuman = type;

        State = IncubatorState.Working;
        GrowTime = GameManager.Instance.GetGrowTime(type);
        GrowLevel = 0;
        TargetPrecision = GameManager.Instance.GetPrecision(type);
        OxygenLevel = 0.5f;
        
        GameObject newTruck = Instantiate(TruckPrefab);
        newTruck.transform.position = TruckStartPosition;
        CurrentTruck = newTruck.GetComponent<Truck>();

        CurrentTruck.SetTargetPosition(new Vector2(transform.position.x, TruckStartPosition.y));
    }
    
    
    private void OnTaskStop(GameManager.HumanType type) {
        //TODO implement
    }

    private void OnHumanDone() {
        State = IncubatorState.Idle;
        CurrentTruck.SetTargetPosition(TruckEndPosition);
        GameManager.Instance.IncubatorDoneEvent.Invoke(this, CurrentHuman);
    }

    public float OxygenLevel {
        get { return _oxygenLevel; }
        set {
            _oxygenLevel = value;
            Vector3 scale = OxygenLevelSprite.transform.localScale;
            OxygenLevelSprite.transform.localScale = new Vector3(scale.x, _oxygenLevel, scale.z);
        }
    }

    void Update() {
        
        switch (State) {
            case IncubatorState.Working: 
                GrowLevel += Time.deltaTime;
                
                if (GrowLevel >= GrowTime) 
                    OnHumanDone();

                if (OxygenLevel > 0) {
                    OxygenLevel -= GameManager.Instance.PressureLossSpeed * Time.deltaTime;
                }
                
                if (Mathf.Abs(0.5f - OxygenLevel) > TargetPrecision) {
                    State = IncubatorState.Idle;
                }

                break;
            
            case IncubatorState.Filling: 
                OxygenLevel += GameManager.Instance.IncubatorFillingRate * Time.deltaTime;
                break;
            
        }
    }
}