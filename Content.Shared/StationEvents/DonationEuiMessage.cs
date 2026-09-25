using Content.Shared.Eui;
using Robust.Shared.Serialization;

namespace Content.Shared.StationEvents;

[Serializable, NetSerializable]
public enum DonationUiButton
{
    Decline,
    Donate,
}

[Serializable, NetSerializable]
public sealed class DonationChoiceMessage : EuiMessageBase
{
    public readonly DonationUiButton Button;

    public DonationChoiceMessage(DonationUiButton button)
    {
        Button = button;
    }
}