using System;
using System.Collections;
using System.Collections.Generic;
using OneLine;
using UnityEngine;
using UnityEngine.Experimental.U2D.TriangleNet.Meshing.Algorithm;
using UnityEngine.UI;

[Serializable]
public class Task {

    public string Template;
    public float Reward;
    public float TimeLeft;
    public float TimeAmount = 0;
    

    public float HumansDone = 0;
    
    [SerializeField, OneLine]
    public List<TaskHuman> Humans;

    public Task(string template = "", float reward = 0) {
        Template = template;
        Reward = reward;
        Humans = new List<TaskHuman>();
    }
    
    public string Text {
        get { return String.Format(Template, string.Join(", ", Humans)); }
    }

    public List<GameManager.HumanType> GetHumansList() {
        
        if (Humans == null) 
            return new List<GameManager.HumanType>();
        
        List<GameManager.HumanType> humans = new List<GameManager.HumanType>();

        foreach (TaskHuman taskHuman in Humans) {
            for (int i = 0; i < taskHuman.Count; i++) {
                humans.Add(taskHuman.Type);
            }
        }

        return humans;
    }
}

[Serializable]
public class TaskHuman {

    public TaskHuman(GameManager.HumanType type, int count) {
        Type = type;
        Count = count;
    }
    
    private string TranslatedType {
        get {
            switch (Type) {
                case GameManager.HumanType.Alpha: return "альфа";
                case GameManager.HumanType.Beta: return "бета";
                case GameManager.HumanType.Gamma: return "гамма";
                case GameManager.HumanType.Delta: return "дельта";
                case GameManager.HumanType.Epsilon: return "эпсилон";
            }

            return "собак";
        }
    }

    public int Count;
    public GameManager.HumanType Type;

    public override string ToString() {
        return Count + " " + TranslatedType;
    }

}