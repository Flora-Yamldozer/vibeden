using Content.Server.Atmos.EntitySystems;
using Content.Shared.Atmos.Components;
using Content.Shared.Popups;
using Content.Shared.SelfCombust;
using Content.Shared.Verbs;
using Robust.Shared.Random;

namespace Content.Server.SelfCombust;

public sealed class SelfCombustSystem : EntitySystem
{
    [Dependency] private readonly FlammableSystem _flammable = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<SelfCombustComponent, GetVerbsEvent<AlternativeVerb>>(OnGetVerbs);
    }

    private void OnGetVerbs(Entity<SelfCombustComponent> ent, ref GetVerbsEvent<AlternativeVerb> args)
    {
        if (!args.CanAccess || !args.CanInteract || args.User != ent.Owner ||
            !TryComp<FlammableComponent>(ent, out _))
        {
            return;
        }

        args.Verbs.Add(new AlternativeVerb
        {
            Text = Loc.GetString("self-combust-verb"),
            Priority = 1,
            Act = () => Combust(ent),
        });
    }

    private void Combust(Entity<SelfCombustComponent> ent)
    {
        if (!TryComp<FlammableComponent>(ent, out var flammable))
            return;

        if (_random.Next(100) == 0)
        {
            _popup.PopupEntity(Loc.GetString("self-combust-singularity"), ent, ent);
            EntityManager.SpawnEntity("Singularity", Transform(ent).Coordinates);
            EntityManager.DeleteEntity(ent);
            return;
        }

        _flammable.AdjustFireStacks(ent, ent.Comp.FireStacks, flammable, ignite: true);
        _popup.PopupEntity(Loc.GetString("self-combust-ignited"), ent, ent);
    }
}
