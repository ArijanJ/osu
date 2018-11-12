﻿// Copyright (c) 2007-2018 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using System.Linq;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Osu.Objects;
using osu.Game.Rulesets.Osu.UI;
using OpenTK;

namespace osu.Game.Rulesets.Osu.Mods
{
    public class OsuModHardRock : ModHardRock, IApplicableToHitObject
    {
        public override double ScoreMultiplier => 1.06;
        public override bool Ranked => true;

        public void ApplyToHitObject(HitObject hitObject)
        {
            var osuObject = (OsuHitObject)hitObject;

            osuObject.Position = new Vector2(osuObject.Position.X, OsuPlayfield.BASE_SIZE.Y - osuObject.Y);

            var slider = hitObject as Slider;
            if (slider == null)
                return;

            slider.HeadCircle.Position = new Vector2(slider.HeadCircle.Position.X, OsuPlayfield.BASE_SIZE.Y - slider.HeadCircle.Position.Y);
            slider.TailCircle.Position = new Vector2(slider.TailCircle.Position.X, OsuPlayfield.BASE_SIZE.Y - slider.TailCircle.Position.Y);

            slider.NestedHitObjects.OfType<SliderTick>().ForEach(h => h.Position = new Vector2(h.Position.X, OsuPlayfield.BASE_SIZE.Y - h.Position.Y));
            slider.NestedHitObjects.OfType<RepeatPoint>().ForEach(h => h.Position = new Vector2(h.Position.X, OsuPlayfield.BASE_SIZE.Y - h.Position.Y));

            var newControlPoints = new Vector2[slider.Path.ControlPoints.Length];
            for (int i = 0; i < slider.Path.ControlPoints.Length; i++)
                newControlPoints[i] = new Vector2(slider.Path.ControlPoints.Span[i].X, -slider.Path.ControlPoints.Span[i].Y);

            slider.Path = new SliderPath(slider.Path.Type, newControlPoints, slider.Path.ExpectedDistance);
        }
    }
}
