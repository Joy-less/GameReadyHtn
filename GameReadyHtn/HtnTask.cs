namespace GameReadyHtn;

/// <summary>
/// A task a <see cref="HtnAgent"/> can perform, changing its states.
/// </summary>
public abstract class HtnTask(object? Name = null) {
    /// <summary>
    /// An optional identifier.
    /// </summary>
    public object? Name = Name;
    /// <summary>
    /// The requirements that must be met before the task becomes valid.
    /// </summary>
    public List<HtnCondition> Requirements = [];
    /// <summary>
    /// Invalid tasks will be ignored.
    /// </summary>
    /// <remarks>By default, always returns null.</remarks>
    public Func<HtnAgent, bool?> IsValidOverride = _ => null;
}
/// <summary>
/// A task that runs a raw action with a list of effects.
/// </summary>
public class HtnPrimitiveTask(object? Name = null) : HtnTask(Name) {
    /// <summary>
    /// The effects that the task is predicted to have on the agent's states.
    /// </summary>
    public required List<HtnEffect> Effects;

    /// <summary>
    /// Gets the predicted states after the action is performed.
    /// </summary>
    public Dictionary<object, object?> PredictStates(IReadOnlyDictionary<object, object?> States) {
        Dictionary<object, object?> PredictedStates = new(States);
        foreach (HtnEffect Effect in Effects) {
            PredictedStates[Effect.State] = Effect.PredictState(PredictedStates);
        }
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
    public List<HtnTask> Tasks = [];
}
/// <summary>
/// A task that is completed by running each task in order.
/// </summary>
public class HtnSequenceTask(object? Name = null) : HtnTask(Name) {
    /// <summary>
    /// The tasks that should be run to complete this task.
    /// </summary>
    public List<HtnTask> Tasks = [];
}