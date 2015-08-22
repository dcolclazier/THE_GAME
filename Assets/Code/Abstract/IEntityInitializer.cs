using System.Collections.Generic;

namespace Assets.Code.Abstract {
    public interface IEntityInitializer {
        List<string> ComponentsToInit { get; }
        Dictionary<string, int> NumericalAttributesToSet { get; } 
        Dictionary<string, string> StringAttributesToSet { get; } 
    }
}