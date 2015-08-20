using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Code.Statics;
using Assets.Components.Movement;
using UnityEngine;
using Vectrosity;

namespace Assets.Code.Abstract {
    public class PathFindingComponent : IComponent {

        //public float Radius { get; private set; }

        private Vector2 _startPosition;
        private PathGraph _pathGraph;
        //private VectorLine PlayerSelectCircle { get; set; }
        private VectorLine DestinationCircle { get; set; }

        private readonly float line_thickness = 2.0f;  //used by vectrosity
        private VectorLine _movePathLine;
        private Vector3 _pathGoal;

        private void OnSelected(GameObject selectedObject)
        {
            if (selectedObject != Parent.Attributes.Get<GameObject>("GameObject")) {
                Selected = false;
                return;
            }
            
            _pathGraph.UpdatePathGraph(_startPosition);
            Selected = true;
            
            CircleCollider2D circleCollider = Parent.Attributes.Get<CircleCollider2D>("ObstructCollider");
            
            _startPosition = circleCollider.transform.position;

            //PlayerSelectCircle.MakeCircle(_startPosition, circleCollider.radius, 360);
            //PlayerSelectCircle.active = true;
            
            if (DestinationCircle.active) DestinationCircle.active = false;
            if (_movePathLine.active) _movePathLine.active = false;
        }

       
        

        public List<string> Dependencies { get; private set; }

        public void OnUpdate() {
            if (!Selected || !Input.GetMouseButton(1)) return;
            
            Debug.Log("Pathfinding ho! About to set destination circle.");
            SetDestinationCircle();
            if (_pathGraph.TargetNode == null) _pathGraph.TargetNode = new Node(_pathGoal);
            else _pathGraph.TargetNode.Position = _pathGoal;

            var path = _pathGraph.GetBestPath().ToArray();


            VectorLine.Destroy(ref _movePathLine);
            _movePathLine = new VectorLine("Move Path Line", path, null, line_thickness, LineType.Continuous);
            //movePathLine = new VectorLine("Move Path Line", new Vector3[0], null, line_thickness, LineType.Continuous);
            _movePathLine.Draw3DAuto();
            _movePathLine.active = true;
        }

        public void OnAwake() {
            throw new NotImplementedException();
        }

        public void OnStart() {
            throw new NotImplementedException();
        }

        public void Init() {

            Debug.Log("Pathfinding init!");
            DestinationCircle = new VectorLine("Destination Circle", new Vector3[720], null, line_thickness);
            DestinationCircle.Draw3DAuto();

            _movePathLine = new VectorLine("Move Path Line", new Vector3[20], null, line_thickness, LineType.Continuous);
            _movePathLine.Draw3DAuto();

            VectorLine.canvas3D.sortingLayerName = "Select Circles";

            _pathGraph = new PathGraph(new Node(Parent.Attributes.Get<Vector2>("Position"), true));


            Messenger.AddListener<GameObject>("PlayerSelected", OnSelected);
            Messenger.AddListener("OnUpdate", OnUpdate);
        }

        public void OnMessage() {
            throw new NotImplementedException();
        }

        public bool Selected { get; set; }

        void SetDestinationCircle()
        {
            Vector3 point = UnityUtilites.MouseWorldPoint();
            var radius = Parent.Attributes.Get<float>("Radius");
            //if mouse is over no walk collider, move destination location out of contact with colliders in scene
            Collider2D overlap = Physics2D.OverlapCircle(point, radius, 1 << 9);
            if (overlap)
            {
                Ray2D ray = new Ray2D(overlap.transform.position, point - overlap.transform.position);
                float dist = Vector2.Distance(overlap.transform.position, point);
                while (overlap)
                {

                    dist += 0.1f;
                    point = ray.GetPoint(dist);
                    overlap = Physics2D.OverlapCircle(point, radius, 1 << 9);
                }
            }

            //Draw Destination circle
            DestinationCircle.MakeCircle(point, radius, 360);
            DestinationCircle.active = true;
            _pathGoal = point;
        }

        public Entity Parent { get; set; }
        public bool Enabled { get; private set; }
        
    }
}