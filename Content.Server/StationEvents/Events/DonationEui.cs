using Content.Server.EUI;
using Content.Shared.Eui;
using Content.Shared.StationEvents;

namespace Content.Server.StationEvents.Events;

public sealed class DonationEui : BaseEui
{
    private readonly EntityUid _target;
    private readonly DonationEventRule _rule;

    public DonationEui(EntityUid target, DonationEventRule rule)
    {
        _target = target;
        _rule = rule;
    }

    public override void HandleMessage(EuiMessageBase msg)
    {
        base.HandleMessage(msg);

        if (msg is not DonationChoiceMessage choice)
        {
            Close();
            return;
        }

        _rule.HandleChoice(_target, choice.Button);
        Close();
    }
}