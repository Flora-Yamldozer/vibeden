using Content.Shared.ForumJanny;
using JetBrains.Annotations;
using Robust.Client.UserInterface;

namespace Content.Client.ForumJanny;

[UsedImplicitly]
public sealed class ForumJannyComputerBoundUserInterface(EntityUid owner, Enum uiKey) : BoundUserInterface(owner, uiKey)
{
    private ForumJannyComputerWindow? _window;

    protected override void Open()
    {
        base.Open();
        _window = this.CreateWindow<ForumJannyComputerWindow>();
        _window.OnApprove += id => SendMessage(new ForumJannyApproveMessage(id));
        _window.OnDeny += id => SendMessage(new ForumJannyDenyMessage(id));
    }

    protected override void UpdateState(BoundUserInterfaceState state)
    {
        base.UpdateState(state);
        if (state is ForumJannyComputerState forumState)
            _window?.Update(forumState);
    }
}
