using System;

namespace Assets.Code.Entities.Components {
    [Flags] public enum LayerFlag {
        //In order of precedence
        Selection = 1 << 8,
        Units = 1 << 9,
        Props = 1 << 10,
        NoWalk = 1 << 11,
        Ground = 1 << 12,
    }
}