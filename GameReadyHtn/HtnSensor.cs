using System.Diagnostics.CodeAnalysis;

namespace GameReadyHtn;

/// <summary>
/// A dynamic updater of a state.
/// Before finding a plan, the agent will call the function to set the state.
/// </summary>
public class HtnSensor() {
    /// <summary>
    /// The state to change.
    /// </summary>
    public required object State { get; set; }
    /// <summary>
    /// The function that returns the value.
    /// </summary>
    public required Func<HtnValue> GetValue { get; set; }

    /// <summary>
    /// Constructs a <see cref="HtnSensor"/> in-line.
    /// </summary>
    [SetsRequiredMembers]
    public HtnSensor(object State, Func<HtnValue> GetValue) : this() {
        this.State = State;
        this.GetValue = GetValue;
    }
}