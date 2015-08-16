using UnityEngine;

namespace Assets.Code {
    static public class Extensions {
        public static void Scale(this BoxCollider2D box, float expansionFactor) {
            
            box.size = new Vector2(box.size.x + 2*expansionFactor, box.size.y + 2*expansionFactor);
        }

        public static void Scale(this CircleCollider2D circle, float expansionFactor) {
            circle.radius += expansionFactor;
        }
    }
}