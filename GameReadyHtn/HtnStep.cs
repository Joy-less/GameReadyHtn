namespace GameReadyHtn;

internal sealed class HtnStep {
    public required HtnStep? Previous;
    public required HtnPrimitiveTask? Task;
    public required Dictionary<object, object?> PredictedStates;
    public required int TotalSteps;

    /// <summary>
    /// Gets the full list of tasks resulting in <see cref="PredictedStates"/>.
    /// </summary>
    public List<HtnPrimitiveTask> GetTasks() {
        Stack<HtnPrimitiveTask> Tasks = new(TotalSteps);
        HtnStep? CurrentStep = this;
        while (CurrentStep is not null) {
            if (CurrentStep.Task is not null) {
                Tasks.Push(CurrentStep.Task);
            }
            CurrentStep = CurrentStep.Previous;
        }
        return [.. Tasks];
    }
}