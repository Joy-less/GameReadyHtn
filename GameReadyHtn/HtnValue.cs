using System.Diagnostics.CodeAnalysis;

namespace GameReadyHtn;

/// <summary>
/// An abstract class for values.
/// </summary>
public abstract class HtnValue {
    /// <summary>
    /// Returns the final value to be used.
    /// </summary>
    public abstract double Evaluate(IDictionary<object, double> States);

    /// <summary>
    /// Constructs a <see cref="HtnConstantValue"/> from a <see cref="double"/>.
    /// </summary>
    public static implicit operator HtnValue(double Value) => new HtnConstantValue(Value);
}
/// <summary>
/// The value of a constant.
/// </summary>
public class HtnConstantValue() : HtnValue {
    /// <summary>
    /// The constant value.
    /// </summary>
    public required double Value { get; set; }

    /// <summary>
    /// Constructs a <see cref="HtnConstantValue"/> in-line.
    /// </summary>
    [SetsRequiredMembers]
    public HtnConstantValue(double Value) : this() {
        this.Value = Value;
    }
    /// <summary>
    /// Returns the constant value.
    /// </summary>
    public override double Evaluate(IDictionary<object, double> States) {
        return Value;
    }
}
/// <summary>
/// The value of a state.
/// </summary>
public class HtnStateValue() : HtnValue {
    /// <summary>
    /// The state to query.
    /// </summary>
    public required object State { get; set; }

    /// <summary>
    /// Constructs a <see cref="HtnStateValue"/> in-line.
    /// </summary>
    [SetsRequiredMembers]
    public HtnStateValue(object State) : this() {
        this.State = State;
    }
    /// <summary>
    /// Returns the state value.
    /// </summary>
    public override double Evaluate(IDictionary<object, double> States) {
        return States.GetValueOrDefault(State);
    }
}
/// <summary>
/// The value of a state after an operation.
/// </summary>
public class HtnStateOperationValue() : HtnValue {
    /// <summary>
    /// The state to query.
    /// </summary>
    public required object State { get; set; }
    /// <summary>
    /// The operation to perform.
    /// </summary>
    public required HtnOperation Operation { get; set; }
    /// <summary>
    /// The operand for the operation.
    /// </summary>
    public required HtnValue Operand { get; set; }

    /// <summary>
    /// Constructs a <see cref="HtnStateOperationValue"/> in-line.
    /// </summary>
    [SetsRequiredMembers]
    public HtnStateOperationValue(object State, HtnOperation Operation, HtnValue Operand) : this() {
        this.State = State;
        this.Operation = Operation;
        this.Operand = Operand;
    }
    /// <summary>
    /// Returns the state value after the operation.
    /// </summary>
    public override double Evaluate(IDictionary<object, double> States) {
        return Operation.Operate(States.GetValueOrDefault(State), Operand.Evaluate(States));
    }
}
/// <summary>
/// The value returned from a function.
/// </summary>
public class HtnDynamicValue() : HtnValue {
    /// <summary>
    /// The function that returns the value.
    /// </summary>
    public required Func<double> GetValue { get; set; }

    /// <summary>
    /// Constructs a <see cref="HtnDynamicValue"/> in-line.
    /// </summary>
    [SetsRequiredMembers]
    public HtnDynamicValue(Func<double> GetValue) : this() {
        this.GetValue = GetValue;
    }
    /// <summary>
    /// Returns the state value after the operation.
    /// </summary>
    public override double Evaluate(IDictionary<object, double> States) {
        return GetValue();
    }
}