<img src="https://github.com/Joy-less/GameReadyHtn/blob/main/Assets/Icon.jpg?raw=true" width=256/>

# Game Ready Htn

[![NuGet](https://img.shields.io/nuget/v/GameReadyHtn.svg)](https://www.nuget.org/packages/GameReadyHtn)
 
An easy-to-use implementation of [HTN](https://youtu.be/Z7uU94yPfD4) (Hierarchical Task Networks) to control game characters in C#.

## Features

- Simple and performant, made for game development
- Expressive with minimal boilerplate

## GOAP vs HTN

This library is parallel to [Game Ready Goap](https://github.com/Joy-less/GameReadyGoap), an implementation of GOAP.

Goal-Oriented Action Planning and Hierarchical Task Networks are both powerful choices for controlling NPCs.
HTN defines a structured hierarchy of nested tasks that should be completed.
GOAP defines a set of goals and a set of actions that can be combined to reach those goals.

HTN is simpler, faster and more predictable because the tasks use a predefined order. It's suitable for most game enemies which only have a limited set of actions. It's an implementation of a behaviour tree.

GOAP is more flexible and powerful because the actions can be combined in very complex ways. It's suitable for agents with lots of actions and strategies to consider, such as in Real-Time Strategy games.

[This Reddit discussion](https://www.reddit.com/r/gamedev/comments/1ozugf) provides more comparisons between GOAP, HTN and behaviour trees.

## Usage

First, create an agent with initial states and a root task:
```cs
GoapAgent Agent = new() {
    // These describe the current state of your agent (character).
    States = new() {
        ...
    },
    // This is the root (compound) task your agent will run, changing its states.
    Task = ...,
};
```

Then, finding a plan is easy:
```cs
Agent.FindPlan();
```

Executing plans is also easy:
```cs
Plan.Execute();
```

## Example

A farmer tends to his crops. He starts his day by farming to increase his crop health, which requires energy, then sleeps, increasing his energy. If he's too tired to farm, he sleeps instead.
```cs
public static readonly HtnAgent Agent = new() {
    States = new() {
        ["Energy"] = 100,
        ["CropHealth"] = 0,
    },
    Task = new HtnSelectorTask("Root") {
        Tasks = [
            new HtnSequenceTask("CompleteDay") {
                Tasks = [
                    new HtnPrimitiveTask("Farm") {
                        Effects = [
                            new HtnEffect() {
                                State = "CropHealth",
                                Operation = HtnOperation.IncreaseBy,
                                Value = 20,
                            },
                            new HtnEffect() {
                                State = "Energy",
                                Operation = HtnOperation.DecreaseBy,
                                Value = 30,
                            },
                        ],
                        Requirements = [
                            new HtnCondition() {
                                State = "Energy",
                                Comparison = HtnComparison.GreaterThanOrEqualTo,
                                Value = 30,
                            },
                        ],
                    },
                    new HtnPrimitiveTask("Sleep") {
                        Effects = [
                            new HtnEffect() {
                                State = "Energy",
                                Operation = HtnOperation.IncreaseBy,
                                Value = 5,
                            },
                        ],
                    },
                ],
            },
            new HtnPrimitiveTask("Sleep") {
                Effects = [
                    new HtnEffect() {
                        State = "Energy",
                        Operation = HtnOperation.IncreaseBy,
                        Value = 5,
                    },
                ],
            },
        ],
    },
};
Farmer.FindPlan();
```

We get 2 actions which completes our task by selecting the `CompleteDay` task:

| Action  | Energy | Crop Health |
| ------- | ------ | ----------- |
| -       | 100    | 0           |
| Farm    | 70     | 20          |
| Sleep   | 75     | 20          |

If we set `Energy` to 20 instead of 100, we get 1 action which completes our task by selecting the `Sleep` task:

| Action  | Energy | Crop Health |
| ------- | ------ | ----------- |
| -       | 20     | 0           |
| Sleep   | 25     | 0           |

## Special Thanks

- [This Is Vini](https://youtu.be/Z7uU94yPfD4) for explaining the HTN algorithm.
- [Fluid HTN](https://github.com/ptrefall/fluid-hierarchical-task-network) for explaining the difference between selector and sequencer compound tasks.