using System;
using System.Collections.Generic;
using UnityEngine;

namespace DarkRoom.Game
{
public enum EGameplayTaskEvent
    {
        Add,
        Remove,
    };

    public enum EGameplayTaskRunResult
    {
        /** When tried running a null-task*/
        Error,
        Failed,
        /** Successfully registered for running, but currently paused due to higher priority tasks running */
        Success_Paused,
        /** Successfully activated */
        Success_Active,
        /** Successfully activated, but finished instantly */
        Success_Finished,
    };

    public enum EGameplayTaskState
    {
        Uninitialized,
        AwaitingActivation,
        Paused,
        Active,
        Finished
    };

    public enum ETaskResourceOverlapPolicy
    {
        /// <summary>
        /// 暂停同样优先级的任务
        /// </summary>
        StartOnTop,

        /// <summary>
        /// 等待其他优先级的任务完成
        /// </summary>
        StartAtEnd,
    };

    public enum FGameplayTaskPriority
    {
        DefaultPriority,
        ScriptedPriority,
    }

    public struct FGameplayTaskEventData
    {
        public EGameplayTaskEvent Event;
        public UGameplayTask RelatedTask;

        public FGameplayTaskEventData(EGameplayTaskEvent InEvent, UGameplayTask InRelatedTask)
        {
            Event = InEvent;
            RelatedTask = InRelatedTask;
        }
    }

    public class UGameplayTask : IGameplayTaskOwnerInterface
    {
        /// <summary>
        /// task名称
        /// </summary>
        protected string InstanceName;

        /// <summary>
        /// 任务优先级
        /// TODO 改为可配置的三种优先级, Important, Normal, Minor
        /// </summary>
        protected FGameplayTaskPriority Priority;

        /// <summary>
        /// 任务状态, 不要直接访问它
        /// TODO 改为private, 如果找到合理的方案
        /// </summary>
        protected EGameplayTaskState TaskState;

        /// <summary>
        /// 任务覆盖策略
        /// </summary>
        protected ETaskResourceOverlapPolicy ResourceOverlapPolicy;

        /* If true, this task will receive TickTask calls from TasksComponent */
        protected bool bTickingTask = true;

        protected bool bIsPausable = true;

        /// <summary>
        /// 是否关心优先级
        /// </summary>
        protected bool bCaresAboutPriority = true;

        /* this is set to avoid duplicate calls to task's owner and TasksComponent when both are the same object */
        protected bool bOwnedByTasksComponent = true;

        protected bool bClaimRequiredResources = true;

        protected bool bOwnerFinished = true;

        /// <summary>
        /// 此资源需要可用的, 任务才会被激活
        /// </summary>
        protected FGameplayResourceSet RequiredResources;

        /// <summary>
        /// 此任务激活, 这个资源会被锁死
        /// </summary>
        protected FGameplayResourceSet ClaimedResources;

        /// <summary>
        /// 谁创建了这个任务
        /// </summary>
        protected IGameplayTaskOwnerInterface TaskOwner;

        /// <summary>
        /// 持有本任务的组件, 一般也是taskowner
        /// </summary>
        protected UGameplayTasksComponent TasksComponent;

        /// <summary>
        /// 子任务实例
        /// </summary>
        protected UGameplayTask ChildTask;

        /// <summary>
        /// 帮助debug的调试信息
        /// </summary>
        protected string DebugDescription;

        public UGameplayTask()
        {
            bTickingTask = false;
            bOwnedByTasksComponent = false;
            bClaimRequiredResources = true;
            bOwnerFinished = false;
            TaskState = EGameplayTaskState.Uninitialized;
            ResourceOverlapPolicy = ETaskResourceOverlapPolicy.StartOnTop;
            Priority = FGameplayTaskPriority.DefaultPriority;
        }

        protected static IGameplayTaskOwnerInterface ConvertToTaskOwner(GameObject OwnerObject)
        {
            return OwnerObject.GetComponent<UGameplayTasksComponent>();
        }

        protected static IGameplayTaskOwnerInterface ConvertToTaskOwner(CUnitEntity OwnerActor)
        {
            return ConvertToTaskOwner(OwnerActor.gameObject);
        }

        public string GetInstanceName()
        {
            return InstanceName;
        }

        public bool IsTickingTask()
        {
            return bTickingTask;
        }

        public bool IsPausable()
        {
            return bIsPausable;
        }

        public bool HasOwnerFinished()
        {
            return bOwnerFinished;
        }

        public int GetPriority()
        {
            return (int)Priority;
        }

        /// <summary>
        /// 本任务是否需要优先级或者资源依赖
        /// </summary>
        public bool RequiresPriorityOrResourceManagement()
        {
            return bCaresAboutPriority || RequiredResources.IsEmpty() == false || ClaimedResources.IsEmpty() == false;
        }

        public FGameplayResourceSet GetRequiredResources()
        {
            return RequiredResources;
        }

        public FGameplayResourceSet GetClaimedResources()
        {
            return ClaimedResources;
        }

        public EGameplayTaskState GetState()
        {
            return TaskState;
        }

        public ETaskResourceOverlapPolicy GetResourceOverlapPolicy()
        {
            return ResourceOverlapPolicy;
        }

        public bool IsActive()
        {
            return TaskState == EGameplayTaskState.Active;
        }

        public bool IsPaused()
        {
            return TaskState == EGameplayTaskState.Paused;
        }

        public bool IsFinished()
        {
            return TaskState == EGameplayTaskState.Finished;
        }

        public UGameplayTask GetChildTask()
        {
            return ChildTask;
        }

        public IGameplayTaskOwnerInterface GetTaskOwner()
        {
            return TaskOwner;
        }


        public bool IsOwnedByTasksComponent()
        {
            return bOwnedByTasksComponent;
        }

        public void ReadyForActivation()
        {
            if (RequiresPriorityOrResourceManagement())
            {
                TasksComponent.AddTaskReadyForActivation(this);
            }
            else
            {
                PerformActivation();
            }
        }

        /* Called to trigger the actual task once the delegates have been set up
         *	Note that the default implementation does nothing and you don't have to call it */
        protected virtual void Activate()
        {
            Debug.LogWarning("Please override this method");
        }

        /// <summary>
        /// 初始化任务, 但不激活. 激活需要调用Activate方法
        /// </summary>
        public void InitTask(IGameplayTaskOwnerInterface InTaskOwner, int InPriority)
        {
            //Priority = InPriority;
            TaskOwner = InTaskOwner;
            TaskState = EGameplayTaskState.AwaitingActivation;

            if (bClaimRequiredResources)
            {
                ClaimedResources.AddSet(RequiredResources);
            }

            InTaskOwner.OnGameplayTaskInitialized(this);

            TasksComponent = InTaskOwner.GetGameplayTasksComponent(this);
            bOwnedByTasksComponent = (TaskOwner == TasksComponent);

            //避免两次调用OnGameplayTaskInitialized方法
            if (!bOwnedByTasksComponent)
            {
                TasksComponent.OnGameplayTaskInitialized(this);
            }
        }


        /// <summary>
        /// bTickingTask 为true时, 本方法才起作用
        /// </summary>
        public virtual void TickTask(float DeltaTime)
        {
            Debug.LogWarning("Please override this method");
        }

        /// <summary>
        /// 外部调用完成任务
        /// </summary>
        public virtual void ExternalConfirm(bool bEndTask)
        {
            if (bEndTask)EndTask();
        }

        /// <summary>
        /// 外部调用取消任务
        /// </summary>
        public virtual void ExternalCancel()
        {
            EndTask();
        }

        /* Return debug string describing task */
        public virtual string GetDebugString()
        {
            return "";
        }

        /* Helper function for getting UWorld off a task */
        /*public virtual UWorld GetWorld()
        {
        }*/

        /// <summary>
        /// 获取此task的owner所对应的unitentity
        /// </summary>
        public CUnitEntity GetOwnerActor()
        {
            if (TaskOwner != null)return TaskOwner.GetGameplayTaskOwner(this);
            if (TasksComponent != null)return TasksComponent.GetGameplayTaskOwner(this);
            return null;
        }

        /// <summary>
        /// 获取和owner actor相关的avatar, 一般会是一个pawn或者tower之类的
        /// </summary>
        /// <returns></returns>
        public CUnitEntity GetAvatarActor()
        {
            if (TaskOwner != null) return TaskOwner.GetGameplayTaskAvatar(this);
            if (TasksComponent != null) return TasksComponent.GetGameplayTaskAvatar(this);
            return null;
        }

        /// <summary>
        /// 如果owner "ended", 那么本方法会被调用. 在OnDestroy中调用
        /// </summary>
        public void TaskOwnerEnded()
        {
            if(IsFinished())return;

            bOwnerFinished = true;
            // mark as finished, just to be on the safe side 
            TaskState = EGameplayTaskState.Finished;
        }

        /// <summary>
        /// 一般 task自己会调用此方法
        /// TODO 注意, 在调用本方法之前, 需要派发"on completed" delegates
        /// </summary>
        public void EndTask()
        {
            if (IsFinished()) return;
            TaskState = EGameplayTaskState.Finished;
        }

        /* Marks this task as requiring specified resource which has a number of consequences,
         *	like task not being able to run if the resource is already taken.
         *
         *	@note: Calling this function makes sense only until the task is being passed over to the GameplayTasksComponent.
         *	Once that's that resources data is consumed and further changes won't get applied 
         */
        //public void AddRequiredResource(TSubclassOf<UGameplayTaskResource> RequiredResource){}
        //public void AddRequiredResourceSet(List<TSubclassOf<UGameplayTaskResource>> RequiredResourceSet){}
        public void AddRequiredResourceSet(FGameplayResourceSet RequiredResourceSet)
        {
            RequiredResources.AddSet(RequiredResourceSet);
        }

        //   public void AddClaimedResource(TSubclassOf<UGameplayTaskResource> ClaimedResource){}
        //   public void AddClaimedResourceSet(List<TSubclassOf<UGameplayTaskResource>> AdditionalResourcesToClaim){}
        public void AddClaimedResourceSet(FGameplayResourceSet AdditionalResourcesToClaim)
        {
            ClaimedResources.AddSet(AdditionalResourcesToClaim);
        }

        public virtual bool IsWaitingOnRemotePlayerdata()
        {
            return false;
        }

        public virtual bool IsWaitingOnAvatar()
        {
            return false;
        }

        /* End and CleanUp the task - may be called by the task itself or by the task owner if the owner is ending. 
         *	IMPORTANT! Do NOT call directly! Call EndTask() or TaskOwnerEnded() 
         *	IMPORTANT! When overriding this function make sure to call Super.OnDestroy(bOwnerFinished) as the last thing,
         *		since the function internally marks the task as "Pending Kill", and this may interfere with internal BP mechanics
         */
        protected virtual void OnDestroy(bool bInOwnerFinished)
        {
            TaskState = EGameplayTaskState.Finished;
            TasksComponent.OnGameplayTaskDeactivated(this);
        }

        // protected by design. Not meant to be called outside from GameplayTaskComponent mechanics
        protected virtual void Pause()
        {
            TaskState = EGameplayTaskState.Paused;
            TasksComponent.OnGameplayTaskDeactivated(this);
        }

        protected virtual void Resume()
        {
            TaskState = EGameplayTaskState.Active;
            TasksComponent.OnGameplayTaskActivated(this);
        }

        // IGameplayTaskOwnerInterface BEGIN
        public virtual UGameplayTasksComponent GetGameplayTasksComponent(UGameplayTask Task)
        {
            return TasksComponent;
        }

        public virtual CUnitEntity GetGameplayTaskOwner(UGameplayTask Task)
        {
            return ((Task == ChildTask) || (Task == this)) ? GetOwnerActor() : null;
        }

        public virtual CUnitEntity GetGameplayTaskAvatar(UGameplayTask Task)
        {
            return ((Task == ChildTask) || (Task == this)) ? GetAvatarActor() : null;
        }

        public virtual int GetGameplayTaskDefaultPriority()
        {
            return GetPriority();
        }

        public virtual void OnGameplayTaskInitialized(UGameplayTask Task)
        {
            // only one child task is allowed
            if (ChildTask != null)
            {
                Debug.LogWarning(">> terminating previous child task: %s");
                ChildTask.EndTask();
            }

            ChildTask = Task;
        }

        public virtual void OnGameplayTaskActivated(UGameplayTask Task)
        {
        }

        public virtual void OnGameplayTaskDeactivated(UGameplayTask Task)
        {
            // cleanup after deactivation
            if (Task != ChildTask)return;

            Debug.LogWarning("%s> Child task deactivated: %s (state: %s)");
            if (Task.IsFinished())ChildTask = null;
        }
        // IGameplayTaskOwnerInterface END


        public void ActivateInTaskQueue()
        {
            switch (TaskState)
            {
                case EGameplayTaskState.Uninitialized:
                    Debug.LogWarning("UGameplayTask.ActivateInTaskQueue Task %s passed for activation withouth having InitTask called on it!");
                    break;
                case EGameplayTaskState.AwaitingActivation:
                    PerformActivation();
                    break;
                case EGameplayTaskState.Paused:
                    Resume();
                    break;
                case EGameplayTaskState.Active:
                    // nothing to do here
                    break;
                case EGameplayTaskState.Finished:
                    // If a task has finished, and it's being revived let's just treat the same as AwaitingActivation
                    PerformActivation();
                    break;
            }
        }

        public void PauseInTaskQueue()
        {
            switch (TaskState)
            {
                case EGameplayTaskState.Uninitialized:
                    Debug.LogWarning("UGameplayTask.PauseInTaskQueue Task %s passed for pausing withouth having InitTask called on it!");
                    break;
                case EGameplayTaskState.AwaitingActivation:
                    // nothing to do here. Don't change the state to indicate this task has never been run before
                    break;
                case EGameplayTaskState.Paused:
                    // nothing to do here. Already paused
                    break;
                case EGameplayTaskState.Active:
                    // pause!
                    Pause();
                    break;
                case EGameplayTaskState.Finished:
                    // nothing to do here. But sounds odd, so let's log this, just in case
                    Debug.LogWarning("UGameplayTask.PauseInTaskQueue Task %s being pause while already marked as Finished");
                    break;
            }
        }

        /// <summary>
        /// 激活任务的入口
        /// </summary>
        private void PerformActivation()
        {
            if(IsActive())return;

            TaskState = EGameplayTaskState.Active;
            Activate();

            //有些立即完成的任务, 激活即完成
            //如果没完成, 我们才跟component打交道
            if (IsFinished() == false)
            {
                TasksComponent.OnGameplayTaskActivated(this);
            }
        }


        /*public string GetDebugDescription(){
            if (DebugDescription.IsEmpty())
            {
                DebugDescription = GenerateDebugDescription(){}
            }
            return DebugDescription;
        }*/
        public virtual string GenerateDebugDescription()
        {
            return "";
        }

        public string GetTaskStateName()
        {
            return TaskState.ToString();
        }

        /*T UGameplayTask.NewTask(UObject WorldContextObject, string InstanceName)
        {
            IGameplayTaskOwnerInterface TaskOwner = ConvertToTaskOwner(WorldContextObject){ }
            return (TaskOwner) ? NewTask<T>(*TaskOwner, InstanceName) : null;
            }

        T UGameplayTask.NewTask(IGameplayTaskOwnerInterface TaskOwner, string InstanceName)
        {
            T MyObj = NewObject<T>(){}
            MyObj->InstanceName = InstanceName;
            MyObj->InitTask(TaskOwner, TaskOwner.GetGameplayTaskDefaultPriority()){}
            return MyObj;
        }

         }*/
    }
}