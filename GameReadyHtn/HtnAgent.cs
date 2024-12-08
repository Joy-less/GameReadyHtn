using System.Collections.Concurrent;

namespace GameReadyHtn;

/// <summary>
/// An entity that can plan tasks, changing its states.
/// </summary>
public class HtnAgent(object? Name = null) {
    /// <summary>
    /// An optional identifier.
    /// </summary>
    public object? Name { get; set; } = Name;
    /// <summary>
    /// The values describing the agent's current state.
    /// </summary>
    public required ConcurrentDictionary<object, object?> States { get; set; }
    /// <summary>
    /// The root task the agent will perform, changing its states.
    /// </summary>
    public required HtnTask Task { get; set; }
    /// <summary>
    /// The state sensors the agent uses to dynamically update its states.
    /// </summary>
    public List<HtnSensor> Sensors { get; set; } = [];

    /// <summary>
    /// Gets the current value of the given state, casting it to the given type.
    /// </summary>
    public T GetState<T>(object State) {
        return (T)States.GetValueOrDefault(State)!;
    }
    /// <summary>
    /// Gets the current value of the given state, cast to a <see langword="dynamic"/>.
    /// </summary>
    public dynamic? GetState(object State) {
        return States.GetValueOrDefault(State);
    }
    /// <summary>
    /// Sets the current value of the given state.
    /// </summary>
    public void SetState(object State, HtnValue Value) {
        States[State] = Value.Evaluate(States);
    }
    /// <summary>
    /// Updates the agent's states from its sensors.
    /// </summary>
    public void SenseStates() {
        foreach (HtnSensor Sensor in Sensors) {
            SetState(Sensor.State, Sensor.GetValue());
        }
    }
    /// <summary>
    /// Attempts to find a plan that completes the tasks in order using backtracking.
    /// </summary>
    public HtnPlan? FindPlan() {
        return HtnPlan.Find(this);
    }
    /// <summary>
    /// Returns true if the task is valid for this agent with the given states.
    /// </summary>
    public bool IsTaskValid(HtnTask Task, IDictionary<object, object?> States) {
        if (Task.IsValidOverride(this) is bool Override) {
            return Override;
        }
        if (!Task.Requirements.All(Requirement => Requirement.IsMet(States))) {
            return false;
        }
        return true;
    }
    /// <summary>
    /// Returns true if the task is valid for this agent.
    /// </summary>
    public bool IsTaskValid(HtnTask Task) {
        SenseStates();
        return IsTaskValid(Task, States);
    }
}