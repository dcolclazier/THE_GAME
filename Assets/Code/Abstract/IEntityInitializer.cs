using System.Collections.Generic;

namespace Assets.Code.Abstract {
    public interface IEntityInitializer {
        List<string> ComponentsToInit { get; }
        List<string> UniqueEntityTags { get; } 
    }
}