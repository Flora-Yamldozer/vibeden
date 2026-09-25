using System.Numerics;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.CustomControls;
using Robust.Shared.Localization;
using static Robust.Client.UserInterface.Controls.BoxContainer;

namespace Content.Client.StationEvents;

public sealed class DonationWindow : DefaultWindow
{
    public readonly Button DonateButton;
    public readonly Button DeclineButton;

    public DonationWindow()
    {
        Title = Loc.GetString("vibeden-donation-window-title");

        ContentsContainer.AddChild(new BoxContainer
        {
            Orientation = LayoutOrientation.Vertical,
            Children =
            {
                new Label
                {
                    Text = Loc.GetString("vibeden-donation-window-prompt"),
                },
                new BoxContainer
                {
                    Orientation = LayoutOrientation.Horizontal,
                    Align = AlignMode.Center,
                    Children =
                    {
                        (DonateButton = new Button
                        {
                            Text = Loc.GetString("vibeden-donation-window-donate"),
                        }),
                        (new Control
                        {
                            MinSize = new Vector2(20, 0),
                        }),
                        (DeclineButton = new Button
                        {
                            Text = Loc.GetString("vibeden-donation-window-decline"),
                        }),
                    },
                },
            },
        });
    }
}