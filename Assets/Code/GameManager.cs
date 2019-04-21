using System;
using System.Collections;
using System.Collections.Generic;
using Code;
using OneLine;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[Serializable]
public class HumanPrecision {

    [SerializeField]
    public GameManager.HumanType Type;
    [SerializeField]
    public float Precision;
}

[Serializable]
public class HumanGrowTime {
    [SerializeField]
    public GameManager.HumanType Type;
    [SerializeField]
    public float Time;
}

[Serializable]
public class HumanGrowPrice {
    [SerializeField]
    public GameManager.HumanType Type;
    [SerializeField]
    public int Price;
}


[Serializable]
public class HumanColor {
    [SerializeField]
    public GameManager.HumanType Type;
    [SerializeField]
    public Color Color;
}

public class GameManager : MonoBehaviour {

    public static GameManager Instance;
    
    public enum HumanType {
        None, Alpha, Beta, Gamma, Delta, Epsilon
    }

    public enum StateType {
        Idle, Task
    }

    public class TaskEvent : UnityEvent<Task> { }
    public class IncubatorEvent : UnityEvent<Incubator, HumanType> { }
    
    [Header("Events")]
    public TaskEvent TaskStartEvent = new TaskEvent();
    public TaskEvent TaskDoneEvent = new TaskEvent();
    public IncubatorEvent IncubatorDoneEvent = new IncubatorEvent();

    [Header("Elements")]
    public TubeController TubeController;
    public List<Incubator> incubators;
    private List<HumanTaskUI> humanTasksUI;

    [Header("UI"), SerializeField, OneLine]
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI DnaText;
    public TextMeshProUGUI TaskText;
    public Image TaskTimeLeftBar;
    public GameObject HumanTaskPrefab;
    public GameObject HumanTasksPanel;

    [Header("Settings")]
    public float PressureLossSpeed = 0.01f;
    public float IncubatorFillingRate = 0.02f;
    [SerializeField, OneLine]
    public List<HumanPrecision> Precisions;
    [SerializeField, OneLine]
    public List<HumanGrowTime> GrowTimes;
    [SerializeField, OneLine]
    public List<HumanColor> HumanColors;

    [Header("State")]
    public float Dna;
    public float Score;
    public float DifficultyLevel;
    public StateType State = StateType.Idle;
    public Task CurrentTask = null;


    private void Start() {
        Instance = this;
        TaskStartEvent.AddListener(OnTaskStart);
        TaskDoneEvent.AddListener(OnTaskDone);
        IncubatorDoneEvent.AddListener(OnIncubatorDone);   
    }
    

    public void Run() {
        Task task = TaskGenerator.Generate(10);
        TaskStartEvent.Invoke(task);
    }

    private void Update() {
        
        ScoreText.text = Score.ToString();
        DnaText.text = Dna.ToString();

        if (CurrentTask != null && State == StateType.Task) {
            CurrentTask.TimeLeft -= Time.deltaTime;
            
            float percentComplete = 100 / CurrentTask.TimeAmount * CurrentTask.TimeLeft;
            TaskTimeLeftBar.fillAmount = percentComplete / 100;
            if (CurrentTask.TimeLeft <= 0) TaskDoneEvent.Invoke(CurrentTask);
        }
           
    }

    public void OnTaskStart(Task task) {
        CurrentTask = task;
        State = StateType.Task;

        TaskText.text = task.Text;
        //TODO Draw other UI regarding to tasks 

        humanTasksUI = new List<HumanTaskUI>();

        foreach (TaskHuman taskHuman in task.Humans) {
            GameObject taskHumanGameObject = Instantiate(HumanTaskPrefab, HumanTasksPanel.transform);
            HumanTaskUI taskHumanUI = taskHumanGameObject.GetComponent<HumanTaskUI>();
            taskHumanUI.Task = taskHuman;
            humanTasksUI.Add(taskHumanUI);
        }
    }

    private void OnIncubatorDone(Incubator incubator, HumanType humanType) {
        
        if (CurrentTask == null) return;

        if (humanTasksUI != null) {
            foreach (HumanTaskUI humanTaskUi in humanTasksUI) {
                if (humanTaskUi.Task.Type == humanType)
                    humanTaskUi.Done++;
            }
        }

        int humanCountsInTask = CurrentTask.GetHumansList().Count;
        CurrentTask.HumansDone++;
        
        if (CurrentTask.HumansDone >= humanCountsInTask) {
            TaskDoneEvent.Invoke(CurrentTask);
        }

    }
    
    public void OnTaskDone(Task task) {
     
        State = StateType.Idle;
        
        int humanCountsInTask = CurrentTask.GetHumansList().Count;
        
        if (CurrentTask.HumansDone >= humanCountsInTask ) {
            Dna += task.Reward;
            Debug.Log("task done");
        }
        else {
            Debug.Log("FAILED");
        }

        CurrentTask = null;

        TaskText.text = "Задача завершена!";

    }

    public float GetGrowTime(HumanType type) {

        foreach (HumanGrowTime item in GrowTimes) {
            if (item.Type == type) {
                return item.Time;
            }
        }

        return 0;
    }
    
    public float GetPrecision(HumanType type) {

        foreach (HumanPrecision item in Precisions) {
            if (item.Type == type) {
                return item.Precision;
            }
        }

        return 0.5f;
    }

    public Color GetHumanColor(HumanType type) {
        foreach (HumanColor item in HumanColors) {
            if (item.Type == type) {
                return item.Color;
            }
        }

        return Color.blue;
    }
    
}