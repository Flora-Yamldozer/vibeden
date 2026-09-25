using Content.Server.Explosion.EntitySystems;
using Content.Shared.GameTicking;
using Robust.Shared.Timing;
using Timer = Robust.Shared.Timing.Timer;

namespace Content.Server.Station.Systems;

public sealed partial class SalvageSpecialistExploderSystem : EntitySystem
{
    [Dependency] private ExplosionSystem _explosion = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<PlayerSpawnCompleteEvent>(OnPlayerSpawnComplete);
    }

    private void OnPlayerSpawnComplete(PlayerSpawnCompleteEvent ev)
    {
        if (ev.JobId != "SalvageSpecialist")
            return;

        var mob = ev.Mob;
        Timer.Spawn(TimeSpan.FromSeconds(60), () =>
        {
            if (!Exists(mob))
                return;

            _explosion.QueueExplosion(mob, ExplosionSystem.DefaultExplosionPrototypeId, 100f, 1f, 3f);
        });
    }
}