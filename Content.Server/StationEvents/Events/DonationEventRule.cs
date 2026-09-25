using System.Linq;
using Content.Server.EUI;
using Content.Server.Explosion.EntitySystems;
using Content.Server.StationEvents.Components;
using Content.Server.Stunnable;
using Content.Shared.Chat;
using Content.Shared.GameTicking.Components;
using Content.Shared.StationEvents;
using Content.Shared.Throwing;
using Robust.Server.Player;
using Robust.Shared.Enums;
using Robust.Shared.Player;
using Robust.Shared.Random;

namespace Content.Server.StationEvents.Events;

public sealed partial class DonationEventRule : StationEventSystem<DonationEventRuleComponent>
{
    [Dependency] private EuiManager _euiManager = default!;
    [Dependency] private IPlayerManager _playerManager = default!;
    [Dependency] private IRobustRandom _random = default!;
    [Dependency] private StunSystem _stun = default!;
    [Dependency] private ThrowingSystem _throwing = default!;
    [Dependency] private ExplosionSystem _explosion = default!;
    [Dependency] private SharedChatSystem _chat = default!;

    protected override void Started(Entity<DonationEventRuleComponent, GameRuleComponent> ent, ref GameRuleStartedEvent args)
    {
        base.Started(ent, ref args);

        var players = _playerManager.Sessions
            .Where(session => session.Status == SessionStatus.InGame && session.AttachedEntity is { Valid: true })
            .ToList();

        if (players.Count == 0)
            return;

        var player = _random.Pick(players);
        _euiManager.OpenEui(new DonationEui(player.AttachedEntity!.Value, this), player);
    }

    public void HandleChoice(EntityUid target, DonationUiButton button)
    {
        if (!Exists(target))
            return;

        if (button == DonationUiButton.Donate)
        {
            _chat.TryEmoteWithoutChat(target, "cry");
            _explosion.QueueExplosion(target, "VibedenDonation", 1000f, 1f, 100f, canCreateVacuum: false);
            return;
        }

        _throwing.TryThrow(target, _random.NextVector2(), 10f);
        _stun.TryUpdateParalyzeDuration(target, TimeSpan.FromSeconds(20));
    }
}