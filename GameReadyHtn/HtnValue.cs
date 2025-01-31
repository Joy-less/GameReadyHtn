using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace GameReadyHtn;

/// <summary>
/// An abstract class for values.
/// </summary>
public abstract class HtnValue {
    /// <summary>
    /// Returns the final value to be used.
    /// </summary>
    public abstract object? Evaluate(IDictionary<object, object?> States);

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static implicit operator HtnValue(bool Value) => new HtnConstantValue(Value);
    public static implicit operator HtnValue(sbyte Value) => new HtnConstantValue(Value);
    public static implicit operator HtnValue(byte Value) => new HtnConstantValue(Value);
    public static implicit operator HtnValue(short Value) => new HtnConstantValue(Value);
    public static implicit operator HtnValue(ushort Value) => new HtnConstantValue(Value);
    public static implicit operator HtnValue(int Value) => new HtnConstantValue(Value);
    public static implicit operator HtnValue(uint Value) => new HtnConstantValue(Value);
    public static implicit operator HtnValue(long Value) => new HtnConstantValue(Value);
    public static implicit operator HtnValue(ulong Value) => new HtnConstantValue(Value);
    public static implicit operator HtnValue(Int128 Value) => new HtnConstantValue(Value);
    public static implicit operator HtnValue(UInt128 Value) => new HtnConstantValue(Value);
    public static implicit operator HtnValue(BigInteger Value) => new HtnConstantValue(Value);
    public static implicit operator HtnValue(Half Value) => new HtnConstantValue(Value);
    public static implicit operator HtnValue(float Value) => new HtnConstantValue(Value);
    public static implicit operator HtnValue(double Value) => new HtnConstantValue(Value);
    public static implicit operator HtnValue(decimal Value) => new HtnConstantValue(Value);
    public static implicit operator HtnValue(string Value) => new HtnConstantValue(Value);
    public static implicit operator HtnValue(char Value) => new HtnConstantValue(Value);
    public static implicit operator HtnValue(DateTime Value) => new HtnConstantValue(Value);
    public static implicit operator HtnValue(DateTimeOffset Value) => new HtnConstantValue(Value);
    public static implicit operator HtnValue(TimeSpan Value) => new HtnConstantValue(Value);
    public static implicit operator HtnValue(Guid Value) => new HtnConstantValue(Value);
#pragma warning restore CS1591
}
/// <summary>
/// The value of a constant.
/// </summary>
public class HtnConstantValue() : HtnValue {
    /// <summary>
    /// The constant value.
    /// </summary>
    public required object? Value { get; set; }

    /// <summary>
    /// Constructs a <see cref="HtnConstantValue"/> in-line.
    /// </summary>
    [SetsRequiredMembers]
    public HtnConstantValue(object? Value) : this() {
        this.Value = Value;
    }
    /// <summary>
    /// Returns the constant value.
    /// </summary>
    public override object? Evaluate(IDictionary<object, object?> States) {
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
    public override object? Evaluate(IDictionary<object, object?> States) {
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
    public override object? Evaluate(IDictionary<object, object?> States) {
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
    public required Func<object?> GetValue { get; set; }

    /// <summary>
    /// Constructs a <see cref="HtnDynamicValue"/> in-line.
    /// </summary>
    [SetsRequiredMembers]
    public HtnDynamicValue(Func<object?> GetValue) : this() {
        this.GetValue = GetValue;
    }
    /// <summary>
    /// Returns the state value after the operation.
    /// </summary>
    public override object? Evaluate(IDictionary<object, object?> States) {
        return GetValue();
    }
}