using System;

namespace GameReadyHtn;

/// <summary>
/// A sequence of primitive tasks a <see cref="HtnAgent"/> could perform.
/// </summary>
public class HtnPlan {
    /// <summary>
    /// The agent the plan was created for.
    /// </summary>
    public required HtnAgent Agent;
    /// <summary>
    /// The actions that should be performed to reach the goal.
    /// </summary>
    public required List<HtnPrimitiveTask> Tasks;
    /// <summary>
    /// The agent's predicted states after the plan is performed.
    /// </summary>
    public required Dictionary<object, object?> PredictedStates;

    /// <summary>
    /// Attempts to find a plan that completes the tasks in order using backtracking.
    /// </summary>
    public static HtnPlan? Find(HtnAgent Agent) {
        // Update agent states from sensors
        Agent.SenseStates();

        // Create first step from initial states
        HtnStep FirstStep = new() {
            Previous = null,
            Task = null,
            PredictedStates = new(Agent.States),
            TotalSteps = 0,
        };

        // Finds the final step from the current step to complete the task
        HtnStep? GetSteps(HtnStep CurrentStep, HtnTask Task) {
            // Ensure the task is valid
            if (!Agent.IsTaskValid(Task)) {
                return null;
            }
            // Add step based on task type
            switch (Task) {
                case HtnPrimitiveTask PrimitiveTask: {
                    // Add step from primitive task
                    HtnStep NextStep = new() {
                        Previous = CurrentStep,
                        Task = PrimitiveTask,
                        PredictedStates = PrimitiveTask.PredictStates(CurrentStep.PredictedStates),
                        TotalSteps = CurrentStep.TotalSteps + 1,
                    };
                    // Return single step
                    return NextStep;
                }
                case HtnSelectorTask SelectorTask: {
                    // Find a valid sub task
                    foreach (HtnTask SubTask in SelectorTask.Tasks) {
                        // Find subsequent steps to complete sub task
                        HtnStep? NextStep = GetSteps(CurrentStep, SubTask);
                        // Choose this sub task if valid
                        if (NextStep is not null) {
                            return NextStep;
                        }
                    }
                    // No sub task was valid
                    return null;
                }
                case HtnSequenceTask SequenceTask: {
                    // Add each sub task
                    foreach (HtnTask SubTask in SequenceTask.Tasks) {
                        // Find subsequent steps to complete sub task
                        HtnStep? NextStep = GetSteps(CurrentStep, SubTask);
                        // Return sequence invalid if sub task is invalid
                        if (NextStep is null) {
                            return null;
                        }
                        // Add step from sub task
                        CurrentStep = NextStep;
                    }
                    // Return sequence of steps
                    return CurrentStep;
                }
                default:
                    // Task type not implemented
                    throw new NotImplementedException(Task.GetType().Name);
            }
        }

        // Find subsequent steps to complete task
        HtnStep? FinalStep = GetSteps(FirstStep, Agent.Task);

        // No plan found
        if (FinalStep is null) {
            return null;
        }

        // Plan found
        return new HtnPlan() {
            Agent = Agent,
            Tasks = FinalStep.GetTasks(),
            PredictedStates = FinalStep.PredictedStates,
        };
    }

    /// <summary>
    /// Tries to execute each task in the plan by calling <see cref="HtnTask.ExecuteAsync"/> and updating the agent's states.
    /// </summary>
    /// <returns>Whether the plan was fully executed successfully.</returns>
    public async Task<bool> ExecuteAsync() {
        foreach (HtnPrimitiveTask Task in Tasks) {
            // Cancel if task is invalid
            if (!Agent.IsTaskValid(Task)) {
                return false;
            }
            // Execute task and cancel if failed
            if (!await Task.ExecuteAsync()) {
                return false;
            }
            // Apply the task's effects
            Task.UpdateStates(Agent.States);
            // Update states from sensors
            Agent.SenseStates();
        }
        return true;
    }
    /// <inheritdoc cref="ExecuteAsync()"/>
    public bool Execute() {
        return ExecuteAsync().GetAwaiter().GetResult();
    }
}