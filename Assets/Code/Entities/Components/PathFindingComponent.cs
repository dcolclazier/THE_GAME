using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Code.Statics;
using Assets.Components.Movement;
using UnityEngine;
using Vectrosity;

namespace Assets.Code.Abstract {
    public class PathFindingComponent : IComponent {

        public List<string> Dependencies { get; private set; }
        
        private void OnSelected(GameObject selectedObject) {
            if (selectedObject != Parent.Attributes.Get<GameObject>("GameObject")) return;


            var circleCollider = Parent.Attributes.Get<CircleCollider2D>("ObstructCollider");
            _startPosition = circleCollider.transform.position;
            
            _pathMap.UpdatePathGraph(_startPosition);


            if (_destinationCircle.active) _destinationCircle.active = false;
            if (_pathLine.active) _pathLine.active = false;
        }

        public void OnUpdate() {
            
            if (!Parent.Attributes.Get<bool>("CurrentlySelected") || !Input.GetMouseButton(1)) return;

            
            SetDestinationCircle();


            if (_pathMap.TargetNode == null) _pathMap.TargetNode = new Node(_pathGoal);
            else _pathMap.TargetNode.Position = _pathGoal;
            _pathMap.UpdatePathGraph(_startPosition);
            var path = _pathMap.GetBestPath().ToArray();


            VectorLine.Destroy(ref _pathLine);
            _pathLine = new VectorLine("Move Path Line", path, null, line_thickness, LineType.Continuous);
            _pathLine.Draw3DAuto();
            //_pathLine.active = true;

            _pathLine.active = Parent.Attributes.Get<bool>("CurrentlySelected");
        }
        void SetDestinationCircle()
        {

            var currentMousePos = UnityUtilites.MouseWorldPoint();
            var radius = Parent.Attributes.Get<float>("ObstructRadius");
            //if mouse is over no walk collider, move destination location out of contact with colliders in scene
            Collider2D overlap = Physics2D.OverlapCircle(currentMousePos, radius, 1 << 11);
            if (overlap)
            {
                Ray2D ray = new Ray2D(overlap.transform.position, currentMousePos - overlap.transform.position.ToVector2());
                float dist = Vector2.Distance(overlap.transform.position, currentMousePos);
                while (overlap)
                {

                    dist += 0.1f;
                    currentMousePos = ray.GetPoint(dist);
                    overlap = Physics2D.OverlapCircle(currentMousePos, radius, 1 << 11);
                }
            }

            //Draw Destination circle
            _destinationCircle.MakeCircle(currentMousePos, radius, 360);
            _destinationCircle.active = true;
            _pathGoal = currentMousePos;
        }
        public void Init() {

            _destinationCircle = new VectorLine("Destination Circle", new Vector3[720], null, line_thickness);
            _destinationCircle.Draw3DAuto();
            _pathLine = new VectorLine("Move Path Line", new Vector3[20], null, line_thickness, LineType.Continuous);
            _pathLine.Draw3DAuto();

            VectorLine.canvas3D.sortingLayerName = "Select Circles";

            //_startPosition = Parent.Attributes.Get<Vector2>("Position");
            var asdf = Parent.Attributes.Get<Vector2>("Position");
            Debug.Log("Position X: " + asdf.x);
            Debug.Log("Position Y: " + asdf.y);
            _pathMap = new PathMap(new Node(asdf, true));

            Messenger.AddListener<GameObject>("GameObjectSelected", OnSelected);
            Messenger.AddListener<GameObject>("GameObjectDeselected", OnDeselected);
            Messenger.AddListener("OnUpdate", OnUpdate);
        }

        private void OnDeselected(GameObject objectDeselected) {
            //if()
            _pathLine.active = false;
            _destinationCircle.active = false;
        }


        public bool Enabled { get; private set; }
        private Vector2 _startPosition;
        private PathMap _pathMap;
        private VectorLine _destinationCircle;
        private readonly float line_thickness = 2.0f;  //used by vectrosity
        private VectorLine _pathLine;
        private Vector3 _pathGoal;
        public Entity Parent { get; set; }
    }
}