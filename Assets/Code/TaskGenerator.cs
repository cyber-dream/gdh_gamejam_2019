using UnityEngine;

namespace Code {
    public class TaskGenerator {
        
        static Random rnd = new Random();


        public static string[] Templates = {
            "На полигон срочно нужно {0:s} для полетов в космос!",
            "Для постройки пункта выдачи сомы нужны {0:s} ",
            "Предоставьте {0:s} в отдел кадров! ",
        };
        
        public static Task Generate(int difficulty) {
            Task task = new Task();

            task.TimeLeft = 25.0f;
            task.TimeAmount = 25.0f;
            
            //TODO calculate task reward based on the difficulty
            task.Reward = difficulty * 10;
            
            //TODO store templates for tasks somewhere
            task.Template = Templates[Random.Range(0, Templates.Length)];
            
            //TODO add humans to the task based on the difficulty
            task.Humans.Add(new TaskHuman(GameManager.HumanType.Delta, 1));
            task.Humans.Add(new TaskHuman(GameManager.HumanType.Gamma, 2));
            
            return task;
        }
        
    }
}