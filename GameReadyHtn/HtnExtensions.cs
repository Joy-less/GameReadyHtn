namespace GameReadyHtn;

/// <summary>
/// A set of extension methods used in <see cref="GameReadyHtn"/>.
/// </summary>
public static class HtnExtensions {
    /// <summary>
    /// Compares the first value with the second value.
    /// </summary>
    public static bool IsMet(this HtnComparison Comparison, dynamic? ValueA, dynamic? ValueB) {
        return Comparison switch {
            HtnComparison.EqualTo => ValueA == ValueB,
            HtnComparison.NotEqualTo => ValueA != ValueB,
            HtnComparison.LessThan => ValueA < ValueB,
            HtnComparison.GreaterThan => ValueA > ValueB,
            HtnComparison.LessThanOrEqualTo => ValueA <= ValueB,
            HtnComparison.GreaterThanOrEqualTo => ValueA >= ValueB,
            _ => throw new NotImplementedException()
        };
    }
    /// <summary>
    /// Compares the value with the target, or the value with the previous value.
    /// </summary>
    public static bool IsMetOrCloser(this HtnComparison Comparison, dynamic? Target, dynamic? Value, dynamic? PreviousValue) {
        return Comparison switch {
            HtnComparison.EqualTo => (Value == Target) || (Math.Abs(Value - Target) < Math.Abs(PreviousValue - Target)),
            HtnComparison.NotEqualTo => (Value != Target) || (Math.Abs(Value - Target) > Math.Abs(PreviousValue - Target)),
            HtnComparison.LessThan => (Value < Target) || (Value < PreviousValue),
            HtnComparison.GreaterThan => (Value > Target) || (Value > PreviousValue),
            HtnComparison.LessThanOrEqualTo => (Value <= Target) || (Value < PreviousValue),
            HtnComparison.GreaterThanOrEqualTo => (Value >= Target) || (Value > PreviousValue),
            _ => throw new NotImplementedException()
        };
    }
    /// <summary>
    /// Performs the operation on the first value with the second value.
    /// </summary>
    public static dynamic? Operate(this HtnOperation Operation, dynamic? ValueA, dynamic? ValueB) {
        return Operation switch {
            HtnOperation.SetTo => ValueB,
            HtnOperation.IncreaseBy => ValueA + ValueB,
            HtnOperation.DecreaseBy => ValueA - ValueB,
            HtnOperation.MultiplyBy => ValueA * ValueB,
            HtnOperation.DivideBy => ValueA / ValueB,
            HtnOperation.ModuloBy => ValueA % ValueB,
            HtnOperation.ExponentiateBy => Math.Pow(Convert.ToDouble(ValueA), Convert.ToDouble(ValueB)),
            _ => throw new NotImplementedException()
        };
    }
}