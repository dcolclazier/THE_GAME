using System;

namespace Assets.Code.Entities.Components {
    [Flags] public enum LayerFlag { //In order of precedence
        Units = 1 << 8,
        Props = 1 << 9,
        SelectCircle = 1 << 10,
        NoWalk = 1 << 11,
        Ground = 1 << 12,
    }
}