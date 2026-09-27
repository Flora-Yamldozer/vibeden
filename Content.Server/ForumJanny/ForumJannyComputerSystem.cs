using Content.Shared.Access.Systems;
using Content.Shared.Dataset;
using Content.Shared.ForumJanny;
using Content.Shared.UserInterface;
using Robust.Server.GameObjects;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;

namespace Content.Server.ForumJanny;

public sealed class ForumJannyComputerSystem : EntitySystem
{
    [Dependency] private readonly AccessReaderSystem _access = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly UserInterfaceSystem _ui = default!;

    private static readonly ProtoId<LocalizedDatasetPrototype> Messages = "ForumJannyPostMessages";
    private static readonly ProtoId<LocalizedDatasetPrototype> Authors = "ForumJannyPostAuthors";

    public override void Initialize()
    {
        SubscribeLocalEvent<ForumJannyComputerComponent, BoundUIOpenedEvent>(OnOpened);
        Subs.BuiEvents<ForumJannyComputerComponent>(ForumJannyUiKey.Key, subs =>
        {
            subs.Event<ForumJannyApproveMessage>(OnApprove);
            subs.Event<ForumJannyDenyMessage>(OnDeny);
        });
    }

    private void OnOpened(Entity<ForumJannyComputerComponent> ent, ref BoundUIOpenedEvent args)
    {
        if (args.UiKey is not ForumJannyUiKey.Key)
            return;

        while (ent.Comp.Requests.Count < 5)
            AddRequest(ent.Comp);

        UpdateUi(ent);
    }

    private void OnApprove(Entity<ForumJannyComputerComponent> ent, ref ForumJannyApproveMessage args)
    {
        HandleDecision(ent, args.RequestId, true, args.Actor);
    }

    private void OnDeny(Entity<ForumJannyComputerComponent> ent, ref ForumJannyDenyMessage args)
    {
        HandleDecision(ent, args.RequestId, false, args.Actor);
    }

    private void HandleDecision(Entity<ForumJannyComputerComponent> ent, int requestId, bool approved, EntityUid actor)
    {
        if (!actor.Valid || !_access.IsAllowed(actor, ent))
            return;

        var request = ent.Comp.Requests.FindIndex(request => request.Id == requestId);
        if (request < 0)
            return;

        ent.Comp.Requests.RemoveAt(request);
        if (approved)
            ent.Comp.Approved++;
        else
            ent.Comp.Denied++;

        AddRequest(ent.Comp);
        UpdateUi(ent);
    }

    private void AddRequest(ForumJannyComputerComponent component)
    {
        var messageDataset = ProtoMan.Index(Messages).Values;
        var authorDataset = ProtoMan.Index(Authors).Values;
        var message = Loc.GetString(messageDataset[_random.Next(messageDataset.Count)]);
        var author = Loc.GetString(authorDataset[_random.Next(authorDataset.Count)]);
        component.Requests.Add(new ForumJannyPostRequest(component.NextRequestId++, author, message));
    }

    private void UpdateUi(Entity<ForumJannyComputerComponent> ent)
    {
        _ui.SetUiState(ent.Owner, ForumJannyUiKey.Key,
            new ForumJannyComputerState(ent.Comp.Requests, ent.Comp.Approved, ent.Comp.Denied));
    }
}
