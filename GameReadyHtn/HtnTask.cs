namespace GameReadyHtn;

/// <summary>
/// A task a <see cref="HtnAgent"/> can perform, changing its states.
/// </summary>
public abstract class HtnTask(object? Name = null) {
    /// <summary>
    /// An optional identifier.
    /// </summary>
    public object? Name { get; set; } = Name;
    /// <summary>
    /// The requirements that must be met before the task becomes valid.
    /// </summary>
    public List<HtnCondition> Requirements { get; set; } = [];
    /// <summary>
    /// Invalid tasks will be ignored.
    /// </summary>
    /// <remarks>By default, always returns null.</remarks>
    public Func<HtnAgent, bool?> IsValidOverride { get; set; } = _ => null;
    /// <summary>
    /// The function that asynchronously executes the task and returns true if successfully completed.
    /// </summary>
    /// <remarks>By default, always returns true.</remarks>
    public Func<Task<bool>> ExecuteAsync { get; set; } = () => Task.FromResult(true);
}
/// <summary>
/// A task that runs a raw action with a list of effects.
/// </summary>
public class HtnPrimitiveTask(object? Name = null) : HtnTask(Name) {
    /// <summary>
    /// The effects that the task is predicted to have on the agent's states.
    /// </summary>
    public required List<HtnEffect> Effects { get; set; }

    /// <summary>
    /// Changes the given states by the effects of the task.
    /// </summary>
    public void UpdateStates(IDictionary<object, object?> States) {
        foreach (HtnEffect Effect in Effects) {
            States[Effect.State] = Effect.PredictState(States);
        }
    }
    /// <summary>
    /// Gets the predicted states after the task is performed.
    /// </summary>
    public Dictionary<object, object?> PredictStates(IDictionary<object, object?> States) {
        Dictionary<object, object?> PredictedStates = new(States);
        UpdateStates(PredictedStates);
        return PredictedStates;
    }
}
/// <summary>
/// A task that is completed by choosing the first valid task.
/// </summary>
public class HtnSelectorTask(object? Name = null) : HtnTask(Name) {
    /// <summary>
    /// The choices of task that could be run to complete this task.
    /// </summary>
    public List<HtnTask> Tasks { get; set; } = [];
}
/// <summary>
/// A task that is completed by running each task in order.
/// </summary>
public class HtnSequenceTask(object? Name = null) : HtnTask(Name) {
    /// <summary>
    /// The tasks that should be run to complete this task.
    /// </summary>
    public List<HtnTask> Tasks { get; set; } = [];
}