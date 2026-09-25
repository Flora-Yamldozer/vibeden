using Content.Client.Eui;
using Content.Shared.StationEvents;
using JetBrains.Annotations;
using Robust.Client.Graphics;

namespace Content.Client.StationEvents;

[UsedImplicitly]
public sealed class DonationEui : BaseEui
{
    private readonly DonationWindow _window;

    public DonationEui()
    {
        _window = new DonationWindow();

        _window.DonateButton.OnPressed += _ =>
        {
            SendMessage(new DonationChoiceMessage(DonationUiButton.Donate));
            _window.Close();
        };

        _window.DeclineButton.OnPressed += _ =>
        {
            SendMessage(new DonationChoiceMessage(DonationUiButton.Decline));
            _window.Close();
        };

        _window.OnClose += () => SendMessage(new DonationChoiceMessage(DonationUiButton.Decline));
    }

    public override void Opened()
    {
        IoCManager.Resolve<IClyde>().RequestWindowAttention();
        _window.OpenCentered();
    }

    public override void Closed()
    {
        _window.Close();
    }
}