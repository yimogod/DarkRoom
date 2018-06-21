using System;
using System.Collections.Generic;
using UnityEngine;

namespace DarkRoom.Game
{
    /// <summary>
    /// 处理task的组件, 外部调用添加或者删除接口
    /// 内部 update 处理缓存的时间列表
    /// </summary>
    public class UGameplayTasksComponent : MonoBehaviour, IGameplayTaskOwnerInterface
    {
        /// <summary>
        /// 锁定的资源变化了发起通知
        /// </summary>
        public Action OnClaimedResourcesChange;

        protected static List<UGameplayTask> LocalTickingTasks = new List<UGameplayTask>();

        /// <summary>
        /// 权重重新排列的task
        /// </summary>
        protected List<UGameplayTask> TaskPriorityQueue;

        /// <summary>
        /// 当前要处理的任务事件列表
        /// </summary>
        protected List<FGameplayTaskEventData> TaskEvents;

        /// <summary>
        /// 当前正在跑的task
        /// </summary>
        protected List<UGameplayTask> TickingTasks;

        /// <summary>
        /// 所有的被本组件处理的task
        /// </summary>
        protected List<UGameplayTask> KnownTasks;

        /// <summary>
        /// 正在运行中的task中最高的优先级
        /// </summary>
        protected int TopActivePriority;

        private int EventLockCounter;
        private bool bInEventProcessingInProgress = true;

        /// <summary>
        /// 缓存当前正在处理的任务的资源
        /// </summary>
        protected FGameplayResourceSet CurrentlyClaimedResources;


        public UGameplayTasksComponent()
        {
            TopActivePriority = 0;
            bInEventProcessingInProgress = false;
        }

        /// <summary>
        /// TODO 这里其实是处理任务的核心地方。 但感觉方法非常不舒服--可能C++只能这么干
        /// 回头改成自己喜欢的方式
        /// </summary>
        public virtual void Update()
        {
            // Because we have no control over what a task may do when it ticks, we must be careful.
            // Ticking a task may kill the task right here. It could also potentially kill another task
            // which was waiting on the original task to do something. Since when a tasks is killed, it removes
            // itself from the TickingTask list, we will make a copy of the tasks we want to service before ticking any
            int NumTickingTasks = TickingTasks.Count;
            int NumActuallyTicked = 0;
            switch (NumTickingTasks)
            {
                case 0:
                    break;
                case 1:
                    TickingTasks[0].TickTask(Time.deltaTime);
                    NumActuallyTicked++;
                    break;
                default:
                    LocalTickingTasks.Clear();
                    LocalTickingTasks.AddRange(TickingTasks);
                    foreach (var TickingTask in LocalTickingTasks)
                    {
                        TickingTask.TickTask(Time.deltaTime);
                        NumActuallyTicked++;
                    }

                    break;
            }


            // Stop ticking if no more active tasks
            if (NumActuallyTicked == 0)
            {
                TickingTasks.Clear();
                UpdateShouldTick();
            }
        }

        public void UpdateShouldTick()
        {
            bool bShouldTick = GetShouldTick();
            //if (bIsActive != bShouldTick)
            //{
            //    SetActive(bShouldTick);
            //}
        }

        /// <summary>
        /// 组件现在是否有正在处理的task
        /// </summary>
        public virtual bool GetShouldTick()
        {
            return TickingTasks.Count > 0;
        }


        //处理 优先级 和 资源
        /// <summary>
        /// 处理传入的task, 如果本task没有优先级或者资源依赖就不处理
        /// </summary>
        public void AddTaskReadyForActivation(UGameplayTask NewTask)
        {
            if (!NewTask.RequiresPriorityOrResourceManagement()) return;

            var e = new FGameplayTaskEventData(EGameplayTaskEvent.Add, NewTask);
            TaskEvents.Add(e);

            // trigger the actual processing only if it was the first event added to the list
            if (TaskEvents.Count == 1 && CanProcessEvents())
            {
                ProcessTaskEvents();
            }
        }

        /// <summary>
        /// 删除传入的task, 本是是放在任务队列中
        /// </summary>
        public void RemoveResourceConsumingTask(UGameplayTask Task)
        {
            var e = new FGameplayTaskEventData(EGameplayTaskEvent.Remove, Task);
            TaskEvents.Add(e);
            // trigger the actual processing only if it was the first event added to the list
            if (TaskEvents.Count == 1 && CanProcessEvents())
            {
                ProcessTaskEvents();
            }
        }

        /// <summary>
        /// 结束所有的相关task
        /// </summary>
        public void EndAllResourceConsumingTasksOwnedBy(IGameplayTaskOwnerInterface TaskOwner)
        {
            for (int Idx = 0; Idx < TaskPriorityQueue.Count; Idx++)
            {
                if (TaskPriorityQueue[Idx].GetTaskOwner() == TaskOwner)
                {
                    ///完成任务, task event会在后面统一删除
                    TaskPriorityQueue[Idx].TaskOwnerEnded();
                }
            }
        }

        /// <summary>
        /// 结束所有的相关task
        /// </summary>
        public bool FindAllResourceConsumingTasksOwnedBy(IGameplayTaskOwnerInterface TaskOwner,
            List<UGameplayTask> FoundTasks)
        {
            int NumFound = 0;
            for (int TaskIndex = 0; TaskIndex < TaskPriorityQueue.Count; TaskIndex++)
            {
                if (TaskPriorityQueue[TaskIndex].GetTaskOwner() == TaskOwner)
                {
                    FoundTasks.Add(TaskPriorityQueue[TaskIndex]);
                    NumFound++;
                }
            }

            return NumFound > 0;
        }

        /// <summary>
        /// 查找第一个 跟名字相关的任务
        /// </summary>
        public UGameplayTask FindResourceConsumingTaskByName(string TaskInstanceName)
        {
            for (int TaskIndex = 0; TaskIndex < TaskPriorityQueue.Count; TaskIndex++)
            {
                if (TaskPriorityQueue[TaskIndex].GetInstanceName() == TaskInstanceName)
                {
                    return TaskPriorityQueue[TaskIndex];
                }
            }

            return null;
        }

        public bool HasActiveTasks(Type TaskClass)
        {
            for (int Idx = 0; Idx < KnownTasks.Count; Idx++)
            {
                if (KnownTasks[Idx].GetType() == TaskClass)
                {
                    return true;
                }
            }

            return false;
        }

        public FGameplayResourceSet GetCurrentlyUsedResources()
        {
            return CurrentlyClaimedResources;
        }

        // BEGIN IGameplayTaskOwnerInterface
        public virtual UGameplayTasksComponent GetGameplayTasksComponent(UGameplayTask Task)
        {
            return this;
        }

        public virtual CUnitEntity GetGameplayTaskOwner(UGameplayTask Task)
        {
            return Task.GetOwnerActor();
        }

        public virtual CUnitEntity GetGameplayTaskAvatar(UGameplayTask Task)
        {
            return Task.GetAvatarActor();
        }

        public int GetGameplayTaskDefaultPriority()
        {
            return 1;
        }

        public virtual void OnGameplayTaskInitialized(UGameplayTask Task)
        {
        }

        public virtual void OnGameplayTaskActivated(UGameplayTask Task)
        {
            KnownTasks.Add(Task);

            if (Task.IsTickingTask())
            {
                if (!TickingTasks.Contains(Task)) TickingTasks.Add(Task);
                // If this is our first ticking task, set this component as active so it begins ticking
                if (TickingTasks.Count == 1) UpdateShouldTick();
            }

            if (!Task.IsOwnedByTasksComponent())
            {
                var TaskOwner = Task.GetTaskOwner();
                if (TaskOwner != null) TaskOwner.OnGameplayTaskActivated(Task);
            }
        }

        public virtual void OnGameplayTaskDeactivated(UGameplayTask Task)
        {
            bool bIsFinished = Task.IsFinished();

            var childTask = Task.GetChildTask();
            if (childTask != null && bIsFinished)
            {
                if (Task.HasOwnerFinished())
                {
                    childTask.TaskOwnerEnded();
                }
                else
                {
                    childTask.EndTask();
                }
            }

            if (Task.IsTickingTask())
            {
                // If we are removing our last ticking task, set this component as inactive so it stops ticking
                TickingTasks.Remove(Task);
            }

            if (bIsFinished)
            {
                // using RemoveSwap rather than RemoveSingleSwap since a Task can be added
                // to KnownTasks both when activating as well as unpausing
                // while removal happens only once. It's cheaper to handle it here.
                KnownTasks.Remove(Task);
            }

            // Resource-using task
            if (Task.RequiresPriorityOrResourceManagement() && bIsFinished)
            {
                OnTaskEnded(Task);
            }

            if (!Task.IsOwnedByTasksComponent() && !Task.HasOwnerFinished())
            {
                var TaskOwner = Task.GetTaskOwner();
                TaskOwner.OnGameplayTaskDeactivated(Task);
            }

            UpdateShouldTick();
        }
        // END IGameplayTaskOwnerInterface


        public string GetTickingTasksDescription()
        {
            return "";
        }

        public string GetKnownTasksDescription()
        {
            return "";
        }

        public string GetTasksPriorityQueueDescription()
        {
            return "";
        }

        public static string GetTaskStateName(EGameplayTaskState Value)
        {
            return "";
        }

        /// <summary>
        /// 处理任务事件
        /// </summary>
        private const int MaxIterations = 16;
        protected void ProcessTaskEvents()
        {
            bInEventProcessingInProgress = true;

            int IterCounter = 0;
            while (TaskEvents.Count > 0)
            {
                IterCounter++;
                if (IterCounter > MaxIterations)
                {
                    Debug.LogWarning(
                        "UGameplayTasksComponent.ProcessTaskEvents has exceeded allowes number of iterations. Check your GameplayTasks for logic loops!");
                    TaskEvents.Clear();
                    break;
                }

                for (int EventIndex = 0; EventIndex < TaskEvents.Count; ++EventIndex)
                {
                    //if (TaskEvents[EventIndex].RelatedTask.IsPendingKill())
                    //{
                        // we should ignore it, but just in case run the removal code.
                    //    RemoveTaskFromPriorityQueue(TaskEvents[EventIndex].RelatedTask);
                     //   continue;
                    //}

                    switch (TaskEvents[EventIndex].Event)
                    {
                        case EGameplayTaskEvent.Add:
                            if (!TaskEvents[EventIndex].RelatedTask.IsFinished())
                            {
                                AddTaskToPriorityQueue(TaskEvents[EventIndex].RelatedTask);
                            }
                            else
                            {
                                Debug.LogWarning(
                                    "UGameplayTasksComponent.ProcessTaskEvents trying to add a finished task to priority queue!");
                            }

                            break;
                        case EGameplayTaskEvent.Remove:
                            RemoveTaskFromPriorityQueue(TaskEvents[EventIndex].RelatedTask);
                            break;
                    }
                }

                TaskEvents.Clear();
                UpdateTaskActivations();

                // task activation changes may create new events, loop over to check it
            }

            bInEventProcessingInProgress = false;
        }

        public void UpdateTaskActivations()
        {
            FGameplayResourceSet ResourcesClaimed = new FGameplayResourceSet(0);

            if (TaskPriorityQueue.Count > 0)
            {
                List<UGameplayTask> ActivationList = new List<UGameplayTask>();

                FGameplayResourceSet ResourcesBlocked = new FGameplayResourceSet(0);
                for (int TaskIndex = 0; TaskIndex < TaskPriorityQueue.Count; ++TaskIndex)
                {
                    FGameplayResourceSet RequiredResources = TaskPriorityQueue[TaskIndex].GetRequiredResources();
                    FGameplayResourceSet ClaimedResources = TaskPriorityQueue[TaskIndex].GetClaimedResources();
                    if (RequiredResources.GetOverlap(ResourcesBlocked).IsEmpty())
                    {
                        // postpone activations, it's some tasks (like MoveTo) require pausing old ones first
                        ActivationList.Add(TaskPriorityQueue[TaskIndex]);
                        ResourcesClaimed.AddSet(ClaimedResources);
                    }
                    else
                    {
                        TaskPriorityQueue[TaskIndex].PauseInTaskQueue();
                    }

                    ResourcesBlocked.AddSet(ClaimedResources);
                }

                for (int Idx = 0; Idx < ActivationList.Count; Idx++)
                {
                    // check if task wasn't already finished as a result of activating previous elements of this list
                    if (ActivationList[Idx] != null
                        && ActivationList[Idx].IsFinished() == false)
                    {
                        ActivationList[Idx].ActivateInTaskQueue();
                    }
                }
            }

            SetCurrentlyClaimedResources(ResourcesClaimed);

         
        }

        public void SetCurrentlyClaimedResources(FGameplayResourceSet NewClaimedSet)
        {
            if (CurrentlyClaimedResources != NewClaimedSet)
            {
                //FGameplayResourceSet ReleasedResources = new FGameplayResourceSet(CurrentlyClaimedResources).RemoveSet(NewClaimedSet);
                //FGameplayResourceSet ClaimedResources = new FGameplayResourceSet(NewClaimedSet).RemoveSet(CurrentlyClaimedResources);
                CurrentlyClaimedResources = NewClaimedSet;
                //OnClaimedResourcesChange.Broadcast(ClaimedResources, ReleasedResources);
            }
        }

        /** called when a task gets ended with an external call, meaning not coming from UGameplayTasksComponent mechanics */
        private void OnTaskEnded(UGameplayTask Task)
        {
            if (Task.RequiresPriorityOrResourceManagement())
                RemoveResourceConsumingTask(Task);
        }

        private void AddTaskToPriorityQueue(UGameplayTask NewTask)
        {
            bool bStartOnTopOfSamePriority = NewTask.GetResourceOverlapPolicy() == ETaskResourceOverlapPolicy.StartOnTop;
            int InsertionPoint = -1;

            for (int Idx = 0; Idx < TaskPriorityQueue.Count; ++Idx)
            {
                if (TaskPriorityQueue[Idx] == null)
                {
                    continue;
                }

                if ((bStartOnTopOfSamePriority && TaskPriorityQueue[Idx].GetPriority() <= NewTask.GetPriority())
                    || (!bStartOnTopOfSamePriority && TaskPriorityQueue[Idx].GetPriority() < NewTask.GetPriority()))
                {
                    TaskPriorityQueue.Insert(Idx, NewTask);
                    InsertionPoint = Idx;
                    break;
                }
            }

            if (InsertionPoint == -1)
            {
                TaskPriorityQueue.Add(NewTask);
            }
        }

        private void RemoveTaskFromPriorityQueue(UGameplayTask Task)
        {
            int RemovedTaskIndex = TaskPriorityQueue.IndexOf(Task);
            if (RemovedTaskIndex != -1)
            {
                TaskPriorityQueue.RemoveAt(RemovedTaskIndex);
            }
            else
            {
                Debug.LogWarning("RemoveTaskFromPriorityQueue for %s called, but it's not in the queue. Might have been already removed");
            }
        }


        private bool CanProcessEvents()
        {
            return !bInEventProcessingInProgress && (EventLockCounter == 0);
        }
    }
}