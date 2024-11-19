namespace GameReadyHtn;

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

    /*/// <summary>
    /// Gets all of the effects of the task including compound tasks.
    /// </summary>
    public abstract IEnumerable<HtnEffect> GetEffects();
    /// <summary>
    /// Gets the predicted states after the action is performed.
    /// </summary>
    public Dictionary<object, object?> PredictStates(IReadOnlyDictionary<object, object?> States) {
        Dictionary<object, object?> PredictedStates = new(States);
        foreach (HtnEffect Effect in Effects) {
            PredictedStates[Effect.State] = Effect.PredictState(PredictedStates);
        }
        return PredictedStates;
    }*/
}
public class HtnPrimitiveTask(object? Name = null) : HtnTask(Name) {
    /// <summary>
    /// The effects that the task is predicted to have on the agent's states.
    /// </summary>
    public required List<HtnEffect> Effects;

    /*public override IEnumerable<HtnEffect> GetEffects() {
        foreach (HtnEffect Effect in Effects) {
            yield return Effect;
        }
    }*/

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
public class HtnSelectorTask(object? Name = null) : HtnTask(Name) {
    /// <summary>
    /// The choices of task that could be run to complete this task.
    /// </summary>
    public List<HtnTask> Tasks = [];

    /*public override IEnumerable<HtnEffect> GetEffects() {
        foreach (HtnTask Task in Tasks) {
            yield return Effect;
        }
    }*/
}
public class HtnSequenceTask(object? Name = null) : HtnTask(Name) {
    /// <summary>
    /// The tasks that should be run to complete this task.
    /// </summary>
    public List<HtnTask> Tasks = [];
}