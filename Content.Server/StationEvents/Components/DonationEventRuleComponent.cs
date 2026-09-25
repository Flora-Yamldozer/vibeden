using Content.Server.StationEvents.Events;

namespace Content.Server.StationEvents.Components;

[RegisterComponent, Access(typeof(DonationEventRule))]
public sealed partial class DonationEventRuleComponent : Component
{
}