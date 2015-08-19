using System.Linq;
using Assets.Code.Entities;
using UnityEngine;
using Vectrosity;

namespace Assets.Code.Abstract {
    public class TempPlayer : Entity {

        public float Radius { get; private set; }
        public Vector2 Position { get; private set; }
        public PathGraph MovementGraph { get; private set; }

        public VectorLine SelectCircle { get; private set; }
        public VectorLine DestinationCircle { get; private set; }

        private readonly float line_thickness = 2.0f;
        private VectorLine _movePathLine;
        private Vector3 _pathGoal;

        void Start() {
            Radius = 0f;
            Position = transform.position;
            SelectCircle = new VectorLine("Select Circle", new Vector3[720], null, line_thickness);
            SelectCircle.Draw3DAuto();
            DestinationCircle = new VectorLine("Destination Circle", new Vector3[720], null, line_thickness);
            DestinationCircle.Draw3DAuto();
            _movePathLine = new VectorLine("Move Path Line", new Vector3[20], null, line_thickness, LineType.Continuous);
            _movePathLine.Draw3DAuto();
            VectorLine.canvas3D.sortingLayerName = "Select Circles";
            MovementGraph = new PathGraph(new Node(Position,true));
            Messenger.AddListener<GameObject>("PlayerSelected",OnSelected);
            Messenger.AddListener<GameObject>("PlayerMouseHover",OnMouseHover);
        }

        private void OnSelected(GameObject selectedObject) {
            if (selectedObject != gameObject) {
                Selected = false;
                return;
            }

            MovementGraph.UpdatePathGraph(transform.position);

            Selected = true;
            //int i = 0;
           
            CircleCollider2D collider = gameObject.GetComponentInChildren<CircleCollider2D>();
            Radius = collider.radius;
            Position = collider.transform.position;
            SelectCircle.MakeCircle(Position, Radius, 360);
            SelectCircle.active = true;
            if (DestinationCircle.active) DestinationCircle.active = false;
            if (_movePathLine.active) _movePathLine.active = false;
        }

        public bool Selected { get; set; }

        private void OnMouseHover(GameObject hoveredObject) {
            if (hoveredObject != gameObject) return;
            Debug.Log("Hovering over me!");
        }
        void setDestinationCircle()
        {
            Debug.Log("DEBUG: Running setDestination.");
            Vector3 point = UnityUtilites.MouseWorldPoint();

            //if mouse is over no walk collider, move destination location out of contact with colliders in scene
            Collider2D overlap = Physics2D.OverlapCircle(point, Radius, 1 << 10);
            if (overlap)
            {
                Ray2D ray = new Ray2D(overlap.transform.position, point - overlap.transform.position);
                float dist = Vector2.Distance(overlap.transform.position, point);
                while (overlap)
                {
                    
                    dist += 0.1f;
                    point = ray.GetPoint(dist);
                    overlap = Physics2D.OverlapCircle(point, Radius, 1 << 10);
                }
            }

            //Draw Destination circle
            DestinationCircle.MakeCircle(point, Radius, 360);
            DestinationCircle.active = true;
            _pathGoal = point;
        }
        protected override void Update() {
            if (Selected && Input.GetMouseButton(1))
            {
                setDestinationCircle();
                

                if (MovementGraph.TargetNode == null) MovementGraph.TargetNode = new Node(_pathGoal);
                else MovementGraph.TargetNode.position = _pathGoal;

                //MovementGraph.UpdateTargetNode();
                //MovementGraph.Update(MovementGraph.TargetNode);
                //MovementGraph.UpdateSourceNeighbors();
               // MovementGraph.UpdateStaticNodeNeighbors();


                Debug.Log("Here goes nothing...");
                var path = MovementGraph.GetBestPath().ToArray();


                VectorLine.Destroy(ref _movePathLine);
                _movePathLine = new VectorLine("Move Path Line", path, null, line_thickness, LineType.Continuous);
                //movePathLine = new VectorLine("Move Path Line", new Vector3[0], null, line_thickness, LineType.Continuous);
                _movePathLine.Draw3DAuto();
                _movePathLine.active = true;
            }
        }
    }
}