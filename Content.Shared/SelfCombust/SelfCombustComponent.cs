using Robust.Shared.GameStates;

namespace Content.Shared.SelfCombust;

[RegisterComponent, NetworkedComponent]
public sealed partial class SelfCombustComponent : Component
{
    [DataField]
    public float FireStacks = 5f;
}
