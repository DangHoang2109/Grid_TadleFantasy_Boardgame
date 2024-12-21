using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameTaskManager : MonoBehaviour
{
    public static InGameTaskManager Instance;
    void Awake() => Instance = this;

    private Queue<ITaskSchedule> taskSchedules = new Queue<ITaskSchedule>();
    private Coroutine coRunning;
    public void ScheduleNewTask(ITaskSchedule task, bool autoRun = true)
    {
        taskSchedules.Enqueue(task);
        if (autoRun)
            RunSchedule();
    }
    public void RunSchedule()
    {
        if (coRunning != null)
            return;

        coRunning = StartCoroutine(CoroutineSchedule());
    }
    private IEnumerator CoroutineSchedule()
    {
        while (this.taskSchedules.Count > 0)
        {
            var act = this.taskSchedules.Dequeue();
            //lastAction = act.GetType().Name;
            yield return act.DoTask();
        }

        this.coRunning = null;
    }
}

public class ITaskSchedule
{
    public virtual IEnumerator DoTask() { yield return null; }
}