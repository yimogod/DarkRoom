namespace DarkRoom.Game
{
    public interface IGameplayTaskOwnerInterface
    {
        /** Finds tasks component for given GameplayTask, Task.GetGameplayTasksComponent() may not be initialized at this point! */
        UGameplayTasksComponent GetGameplayTasksComponent(UGameplayTask Task);

        /** Get owner of a task or default one when task is null */
        CUnitEntity GetGameplayTaskOwner(UGameplayTask Task);

        /** Get "body" of task's owner / default, having location in world (e.g. Owner = AIController, Avatar = Pawn) */
        CUnitEntity GetGameplayTaskAvatar(UGameplayTask Task);

        /** Get default priority for running a task */
        int GetGameplayTaskDefaultPriority();

        /** Notify called after GameplayTask finishes initialization (not active yet) */
        void OnGameplayTaskInitialized(UGameplayTask Task);

        /** Notify called after GameplayTask changes state to Active (initial activation or resuming) */
        void OnGameplayTaskActivated(UGameplayTask Task);

        /** Notify called after GameplayTask changes state from Active (finishing or pausing) */
        void OnGameplayTaskDeactivated(UGameplayTask Task);
    }
}
