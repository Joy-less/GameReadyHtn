using System.Diagnostics.CodeAnalysis;

namespace GameReadyHtn;

/// <summary>
/// A condition for a state's value.
/// </summary>
public class HtnCondition() {
    /// <summary>
    /// The state to compare.
    /// </summary>
    public required object State;
    /// <summary>
    /// How to compare the state value and value.
    /// </summary>
    public required HtnComparison Comparison;
    /// <summary>
    /// The value to compare the state with.
    /// </summary>
    public required HtnValue Value;
    /// <summary>
    /// If true, plans that get the agent closer to the condition will be considered, even if they won't reach it. The values must be numbers.<br/>
    /// Default: false
    /// </summary>
    public bool BestEffort = false;

    /// <summary>
    /// Constructs a <see cref="HtnCondition"/> in-line.
    /// </summary>
    [SetsRequiredMembers]
    public HtnCondition(object State, HtnComparison Comparison, HtnValue Value) : this() {
        this.State = State;
        this.Comparison = Comparison;
        this.Value = Value;
    }
    /// <summary>
    /// Returns true if the condition is met with the given states.
    /// </summary>
    public bool IsMet(IDictionary<object, object?> States) {
        return Comparison.IsMet(States.GetValueOrDefault(State), Value.Evaluate(States));
    }
    /// <summary>
    /// Returns true if the condition is met with the given states, or closer to being met than with the previous states.
    /// </summary>
    public bool IsMetOrCloser(IDictionary<object, object?> States, IDictionary<object, object?> PreviousStates) {
        if (!BestEffort) {
            return IsMet(States);
        }
        return Comparison.IsMetOrCloser(Value.Evaluate(States), States.GetValueOrDefault(State), PreviousStates.GetValueOrDefault(State));
    }
}