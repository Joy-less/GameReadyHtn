namespace GameReadyHtn.Tests;

public class FarmerTest {
    public static readonly HtnAgent Agent = new() {
        States = new() {
            [FarmerState.Energy] = 100,
            [FarmerState.CropHealth] = 0,
        },
        Task = new HtnSelectorTask("Root") {
            Tasks = [
                new HtnSequenceTask("CompleteDay") {
                    Tasks = [
                        new HtnPrimitiveTask("Farm") {
                            Effects = [
                                new HtnEffect() {
                                    State = FarmerState.CropHealth,
                                    Operation = HtnOperation.IncreaseBy,
                                    Value = 20,
                                },
                                new HtnEffect() {
                                    State = FarmerState.Energy,
                                    Operation = HtnOperation.DecreaseBy,
                                    Value = 30,
                                },
                            ],
                            Requirements = [
                                new HtnCondition() {
                                    State = FarmerState.Energy,
                                    Comparison = HtnComparison.GreaterThanOrEqualTo,
                                    Value = 30,
                                },
                            ],
                        },
                        new HtnPrimitiveTask("Sleep") {
                            Effects = [
                                new HtnEffect() {
                                    State = FarmerState.Energy,
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
                            State = FarmerState.Energy,
                            Operation = HtnOperation.IncreaseBy,
                            Value = 5,
                        },
                    ],
                },
            ],
        },
    };

    [Fact]
    public void Test() {
        Assert.NotNull(Agent.FindPlan());
        Assert.NotNull(Agent.FindPlan()?.Tasks.FirstOrDefault());
    }
}
public enum FarmerState {
    Energy,
    CropHealth,
}