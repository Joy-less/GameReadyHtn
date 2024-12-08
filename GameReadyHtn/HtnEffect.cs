using System.Diagnostics.CodeAnalysis;

namespace GameReadyHtn;

/// <summary>
/// An effect a <see cref="HtnTask"/> will have on a <see cref="HtnAgent"/>'s states.
/// </summary>
public class HtnEffect() {
    /// <summary>
    /// The state to change.
    /// </summary>
    public required object State { get; set; }
    /// <summary>
    /// The operation to perform.
    /// </summary>
    public required HtnOperation Operation { get; set; }
    /// <summary>
    /// The operand for the operation.
    /// </summary>
    public required HtnValue Value { get; set; }

    /// <summary>
    /// Constructs a <see cref="HtnEffect"/> in-line.
    /// </summary>
    [SetsRequiredMembers]
    public HtnEffect(object State, HtnOperation Operation, HtnValue Value) : this() {
        this.State = State;
        this.Operation = Operation;
        this.Value = Value;
    }
    /// <summary>
    /// Gets the predicted state value after the effect is applied.
    /// </summary>
    public object? PredictState(IDictionary<object, object?> States) {
        return Operation.Operate(States[State], Value.Evaluate(States));
    }
}