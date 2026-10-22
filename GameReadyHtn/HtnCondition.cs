using System.Diagnostics.CodeAnalysis;

namespace GameReadyHtn;

/// <summary>
/// A condition for a state's value.
/// </summary>
public class HtnCondition() {
    /// <summary>
    /// The state to compare.
    /// </summary>
    public required object State { get; set; }
    /// <summary>
    /// How to compare the state value and value.
    /// </summary>
    public required HtnComparison Comparison { get; set; }
    /// <summary>
    /// The value to compare the state with.
    /// </summary>
    public required HtnValue Value { get; set; }
    /// <summary>
    /// If true, plans that get the agent closer to the condition will be considered, even if they won't reach it.<br/>
    /// Default: false
    /// </summary>
    public bool BestEffort { get; set; } = false;

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
    public bool IsMet(IDictionary<object, double> States) {
        return Comparison.IsMet(States.GetValueOrDefault(State), Value.Evaluate(States));
    }
    /// <summary>
    /// Returns true if the condition is met with the given states, or closer to being met than with the previous states.
    /// </summary>
    public bool IsMetOrCloser(IDictionary<object, double> States, IDictionary<object, double> PreviousStates) {
        if (!BestEffort) {
            return IsMet(States);
        }
        return Comparison.IsMetOrCloser(Value.Evaluate(States), States.GetValueOrDefault(State), PreviousStates.GetValueOrDefault(State));
    }
}