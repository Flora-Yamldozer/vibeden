using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared.ForumJanny;

[RegisterComponent, NetworkedComponent]
public sealed partial class ForumJannyComputerComponent : Component
{
    [ViewVariables]
    public List<ForumJannyPostRequest> Requests = new();

    [ViewVariables]
    public int Approved;

    [ViewVariables]
    public int Denied;

    [ViewVariables]
    public int NextRequestId;
}

[Serializable, NetSerializable]
public sealed class ForumJannyPostRequest
{
    public int Id;
    public string Author;
    public string Message;

    public ForumJannyPostRequest(int id, string author, string message)
    {
        Id = id;
        Author = author;
        Message = message;
    }
}

[Serializable, NetSerializable]
public sealed class ForumJannyComputerState : BoundUserInterfaceState
{
    public List<ForumJannyPostRequest> Requests;
    public int Approved;
    public int Denied;

    public ForumJannyComputerState(List<ForumJannyPostRequest> requests, int approved, int denied)
    {
        Requests = requests;
        Approved = approved;
        Denied = denied;
    }
}

[Serializable, NetSerializable]
public sealed class ForumJannyApproveMessage : BoundUserInterfaceMessage
{
    public int RequestId;

    public ForumJannyApproveMessage(int requestId)
    {
        RequestId = requestId;
    }
}

[Serializable, NetSerializable]
public sealed class ForumJannyDenyMessage : BoundUserInterfaceMessage
{
    public int RequestId;

    public ForumJannyDenyMessage(int requestId)
    {
        RequestId = requestId;
    }
}

[Serializable, NetSerializable]
public enum ForumJannyUiKey : byte
{
    Key
}
