using System.Collections.Generic;
using System.Linq;
using Assets.Code.Abstract;
using Assets.Code.Movement;
using Assets.Code.Statics;
using UnityEngine;
using Vectrosity;

namespace Assets.Code.Entities.Components {
    public class PathFindingComponent : IComponent {

        public List<string> Dependencies {
            get { return new List<string>() {
                "GameObject",
                "Obstructable",
                "Selectable"
            }; }
        }

        private void OnSelected(GameObject selectedObject) {
            if (selectedObject != Parent.Attributes.Get<GameObject>("GameObject")) return;

            _pathMap = new PathMap(new Node(Parent.Attributes.Get<Vector2>("Position"), true));
            _pathMap.UpdatePathGraph(Parent.Attributes.Get<CircleCollider2D>("ObstructCollider").transform.position);

            _destinationCircle.active = true;
            _pathLine.active = true;

			_destinationCircle.Draw3DAuto();
            Parent.Attributes.Update("PathIsActive", true);
        }
        private void OnDeselected(GameObject objectDeselected) {
            _pathLine.active = false;
            _destinationCircle.active = false;
            Parent.Attributes.Update("PathIsActive", false);
        }

        public void OnUpdate() {
            if (!Parent.Attributes.Get<bool>("CurrentlySelected")) return;
            if (!Input.GetMouseButton(1)) {
                Parent.Attributes.Update("CurrentlyPathing",false);
                return;
            }
            Parent.Attributes.Update("CurrentlyPathing",true);

            var path = _pathMap.GetBestPath(SetDestinationCircle(), Parent);
            var enumerable = path as IList<Vector3> ?? path.ToList();
            Parent.Attributes.Update("CurrentPath", enumerable.ToList());
                
            VectorLine.Destroy(ref _pathLine);
            _pathLine = new VectorLine("Move Path Line", enumerable.ToArray(), null, line_thickness, LineType.Continuous);
            _pathLine.Draw3DAuto();
        }
        
        public void Init() {

            _destinationCircle = new VectorLine("Destination Circle", new Vector3[720], null, line_thickness);
            _destinationCircle.Draw3DAuto();

            _pathLine = new VectorLine("Move Path Line", new Vector3[20], null, line_thickness, LineType.Continuous);
            _pathLine.Draw3DAuto();

            VectorLine.canvas3D.sortingLayerName = "Select Circles";

            Messenger.AddListener<GameObject>("GameObjectSelected", OnSelected);
            Messenger.AddListener<GameObject>("GameObjectDeselected", OnDeselected);
            Messenger.AddListener("OnUpdate", OnUpdate);

            Parent.Attributes.Register("CurrentlyPathing", false);
            Parent.Attributes.Register("CurrentPathTarget", new Vector2());
            Parent.Attributes.Register("PathIsActive", false);
            Parent.Attributes.Register("CurrentPath", new List<Vector3>());
        }
        private Vector2 SetDestinationCircle()
        {

            var currentMousePos = UnityUtilites.MouseWorldPoint();
            var radius = Parent.Attributes.Get<float>("ObstructRadius");
            var overlap = Physics2D.OverlapCircle(currentMousePos, radius, 1 << 11);
            if (overlap)
            {
                var ray = new Ray2D(overlap.transform.position, currentMousePos - overlap.transform.position.ToVector2());
                var dist = Vector2.Distance(overlap.transform.position, currentMousePos);
                while (overlap)
                {
                    dist += 0.1f;
                    currentMousePos = ray.GetPoint(dist);
                    overlap = Physics2D.OverlapCircle(currentMousePos, radius, 1 << 11);
                }
            }
            _destinationCircle.MakeCircle(currentMousePos, radius, 360);
            Parent.Attributes.Update("CurrentPathTarget", currentMousePos);
            return currentMousePos;
        }

        public bool Enabled { get; private set; }
        private PathMap _pathMap;
        private VectorLine _destinationCircle;
        private readonly float line_thickness = 2.0f;  //used by vectrosity
        private VectorLine _pathLine;
        
        public Entity Parent { get; set; }
    }
}