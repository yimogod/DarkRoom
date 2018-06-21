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
        /** Pause overlapping same-priority tasks. */
        StartOnTop,

        /** Wait for other same-priority tasks to finish. */
        StartAtEnd,
    };

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
        /* This name allows us to find the task later so that we can end it. */
        protected string InstanceName;

        /* This controls how this task will be treaded in relation to other, already running tasks, 
         *	provided GameplayTasksComponent is configured to care about priorities (the default behavior)*/
        protected int Priority;

        /* You should _never_ access it directly. We'll make it private once we have
         *	a good way of "deprecating: direct access */
        protected EGameplayTaskState TaskState;


        protected ETaskResourceOverlapPolicy ResourceOverlapPolicy;

        /* If true, this task will receive TickTask calls from TasksComponent */
        protected bool bTickingTask = true;

        /* Should this task run on simulated clients? This should only be used in rare cases, such as movement tasks. Simulated Tasks do not broadcast their end delegates.  */
        protected bool bSimulatedTask = true;

        /* Am I actually running this as a simulated task. (This will be true on clients that simulating. This will be false on the server and the owning client) */
        protected bool bIsSimulating = true;

        protected bool bIsPausable = true;

        protected bool bCaresAboutPriority = true;

        /* this is set to avoid duplicate calls to task's owner and TasksComponent when both are the same object */
        protected bool bOwnedByTasksComponent = true;

        protected bool bClaimRequiredResources = true;

        protected bool bOwnerFinished = true;

        /* Abstract "resource" IDs this task needs available to be able to get activated. */
        protected FGameplayResourceSet RequiredResources;

        /**
         *	Resources that are going to be locked when this task gets activated, but are not required to get this task started
         */
        protected FGameplayResourceSet ClaimedResources;

        /* Task Owner that created us */
        protected IGameplayTaskOwnerInterface TaskOwner;

        protected UGameplayTasksComponent TasksComponent;

        /* child task instance */
        protected UGameplayTask ChildTask;

        protected string DebugDescription;

        public UGameplayTask()
        {
        }


        public void ReadyForActivation()
        {
        }

        /* Called to trigger the actual task once the delegates have been set up
         *	Note that the default implementation does nothing and you don't have to call it */
        protected virtual void Activate()
        {
        }

        /* Initailizes the task with the task owner interface instance but does not actviate until Activate() is called */
        public void InitTask(IGameplayTaskOwnerInterface InTaskOwner, int InPriority)
        {
        }


        public virtual void InitSimulatedTask(UGameplayTasksComponent InGameplayTasksComponent)
        {
        }

        /* Tick function for this task, if bTickingTask == true */
        public virtual void TickTask(float DeltaTime)
        {
        }

        /* Called when the task is asked to confirm from an outside node. What this means depends on the individual task. By default, this does nothing other than ending if bEndTask is true. */
        public virtual void ExternalConfirm(bool bEndTask)
        {
        }

        /* Called when the task is asked to cancel from an outside node. What this means depends on the individual task. By default, this does nothing other than ending the task. */
        public virtual void ExternalCancel()
        {
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

        /* Proper way to get the owning actor of task owner. This can be the owner itself since the owner is given as a interface */
        public CUnitEntity GetOwnerActor()
        {
            return null;
        }

        /* Proper way to get the avatar actor associated with the task owner (usually a pawn, tower, etc) */
        public CUnitEntity GetAvatarActor()
        {
            return null;
        }

        /* Called when task owner has "ended" (before the task ends) kills the task. Calls OnDestroy. */
        public void TaskOwnerEnded()
        {
        }

        /* Called explicitly to end the task (usually by the task itself). Calls OnDestroy. 
         *	@NOTE: you need to call EndTask before sending out any "on completed" delegates. 
         *	If you don't the task will still be in an "active" state while the event receivers may
         *	assume it's already "finished" */
        public void EndTask()
        {
        }

        public virtual bool IsSupportedForNetworking()
        {
            return bSimulatedTask;
        }

        public string GetInstanceName()
        {
            return InstanceName;
        }

        public bool IsTickingTask()
        {
            return bTickingTask;
        }

        public bool IsSimulatedTask()
        {
            return bSimulatedTask;
        }

        public bool IsSimulating()
        {
            return bIsSimulating;
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
            return Priority;
        }

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

        }

        //   public void AddClaimedResource(TSubclassOf<UGameplayTaskResource> ClaimedResource){}
        //   public void AddClaimedResourceSet(List<TSubclassOf<UGameplayTaskResource>> AdditionalResourcesToClaim){}
        public void AddClaimedResourceSet(FGameplayResourceSet AdditionalResourcesToClaim)
        {
        }

        public ETaskResourceOverlapPolicy GetResourceOverlapPolicy()
        {
            return ResourceOverlapPolicy;
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
        }

        protected static IGameplayTaskOwnerInterface ConvertToTaskOwner(GameObject OwnerObject)
        {
            return null;
        }

        protected static IGameplayTaskOwnerInterface ConvertToTaskOwner(CUnitEntity OwnerActor)
        {
            return null;
        }

// protected by design. Not meant to be called outside from GameplayTaskComponent mechanics
        protected virtual void Pause()
        {
        }

        protected virtual void Resume()
        {
        }

// IGameplayTaskOwnerInterface BEGIN
        public virtual UGameplayTasksComponent GetGameplayTasksComponent(UGameplayTask Task)
        {
            return null;
        }

        public virtual CUnitEntity GetGameplayTaskOwner(UGameplayTask Task)
        {
            return null;
        }

        public virtual CUnitEntity GetGameplayTaskAvatar(UGameplayTask Task)
        {
            return null;
        }

        public virtual int GetGameplayTaskDefaultPriority()
        {
            return -1;
        }

        public virtual void OnGameplayTaskInitialized(UGameplayTask Task)
        {
        }

        public virtual void OnGameplayTaskActivated(UGameplayTask Task)
        {
        }
        public virtual void OnGameplayTaskDeactivated(UGameplayTask Task)
        {
        }
        // IGameplayTaskOwnerInterface END


        private void ActivateInTaskQueue()
        {
        }

        private void PauseInTaskQueue()
        {
        }

        private void PerformActivation()
        {
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
            return "";
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