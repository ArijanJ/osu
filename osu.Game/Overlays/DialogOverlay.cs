﻿// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Transforms;
using osu.Game.Graphics;
using osu.Game.Overlays.Dialog;
using OpenTK.Graphics;

namespace osu.Game.Overlays
{
    public class DialogOverlay : FocusedOverlayContainer
    {
        private Container dialogContainer;
        private PopupDialog currentDialog;

        public void Push(PopupDialog dialog)
        {
            State = Visibility.Visible;

            dialogContainer.Add(dialog);
            dialog.Show();
            dialog.StateChanged += delegate (OverlayContainer c, Visibility v)
            {
                if (v == Visibility.Hidden)
                {
                    c.Delay(PopupDialog.EXIT_DURATION);
                    c.Expire();
                    if (c == currentDialog)
                        State = Visibility.Hidden;
                }
            };

            var lastDialog = currentDialog;
            currentDialog = dialog;
            lastDialog?.Hide();
        }

        protected override void PopIn()
        {
            base.PopIn();
            FadeIn(PopupDialog.ENTER_DURATION, EasingTypes.OutQuint);
        }

        protected override void PopOut()
        {
            base.PopOut();
            FadeOut(PopupDialog.EXIT_DURATION, EasingTypes.InSine);
        }

        public DialogOverlay()
        {
            RelativeSizeAxes = Axes.Both;

            Children = new Drawable[]
            {
                new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = Color4.Black.Opacity(0.5f),
                        },
                    },
                },
                dialogContainer = new Container
                {
                    RelativeSizeAxes = Axes.Both,
                },
            };
        }
    }
}
