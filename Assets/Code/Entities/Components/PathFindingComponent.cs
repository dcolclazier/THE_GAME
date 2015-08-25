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
        private void OnSelected(Entity entity) {
            if (entity != Parent) return;

            UpdatePathMap();
            EnablePathArt();
        }
        public void Init() {
            //_playername = Parent.Attributes.Get<string>("Name");
            //Debug.Log("Pathfinder Init for " + _playername);
            
            _destinationCircle = new VectorLine("Destination Circle", new Vector3[720], null, line_thickness);
            _destinationCircle.Draw3DAuto();

            _pathLine = new VectorLine("Move Path Line", new Vector3[20], null, line_thickness, LineType.Continuous);
            

            VectorLine.canvas3D.sortingLayerName = "Select Circles";

            Messenger.AddListener<LayerFlag,RaycastHit2D>("LeftMouseDown",LeftMouseDown);
            Messenger.AddListener<LayerFlag,RaycastHit2D>("RightMouseHeld",RightMouseHeld);
            Messenger.AddListener<Entity>("EntitySelected", OnSelected);
            Messenger.AddListener<Entity>("EntityDeselected", OnDeselected);
            Messenger.AddListener<Entity>("EntityMoved",EntityMoved);

            Parent.Attributes.Register("CurrentlyPathing", false);
            Parent.Attributes.Register("CurrentPathTarget", new Vector2());
            Parent.Attributes.Register("PathIsActive", false);
            Parent.Attributes.Register("CurrentPath", new List<Vector3>());
        }

        private void EntityMoved(Entity entity) {
            if (entity != Parent) return;

            UpdatePathMap();
            UpdatePath();
            DrawPathLine();
        }

        private void RightMouseHeld(LayerFlag arg1, RaycastHit2D arg2) {
            if (!Parent.Attributes.Get<bool>("CurrentlySelected")) return;

            Parent.Attributes.Update("CurrentlyPathing", true);

            _target = SetDestinationCircle();
            UpdatePath();
            EnablePathArt();
            DrawPathLine();
            
            Parent.Attributes.Update("PathIsActive", true);
        }

        private void LeftMouseDown(LayerFlag layerClicked, RaycastHit2D objectClicked) {
            
            if (layerClicked != LayerFlag.Selection) return;
            
            if (!Parent.Attributes.Get<bool>("PathIsActive")) return;

            if (objectClicked.transform.parent.gameObject !=
                Parent.Attributes.Get<GameObject>("GameObject")) return;

            if (!Parent.Attributes.Get<bool>("PathIsActive")) return;
            
            DisablePathArt();
            ResetPath();
        }

        private void ResetPath() {
            _currentPath = null;
        }

        private Vector2 SetDestinationCircle()
        {

            var currentMousePos = UnityUtilites.MouseWorldPoint();
            var radius = Parent.Attributes.Get<float>("ObstructRadius");
            var overlap = Physics2D.OverlapCircle(currentMousePos, radius, (int)LayerFlag.NoWalk);
            if (overlap)
            {
                //Disable pathline
                //Draw red X where mouse position is


                var ray = new Ray2D(overlap.transform.position, currentMousePos - overlap.transform.position.ToVector2());
                var dist = Vector2.Distance(overlap.transform.position, currentMousePos);
                while (overlap)
                {
                    dist += 0.1f;
                    currentMousePos = ray.GetPoint(dist);
                    overlap = Physics2D.OverlapCircle(currentMousePos, radius, (int)LayerFlag.NoWalk);
                }
            }
            _destinationCircle.MakeCircle(currentMousePos, radius, 360);
            Parent.Attributes.Update("CurrentPathTarget", currentMousePos);
            return currentMousePos;
        }


        private void UpdatePathMap() {
            var position = Parent.Attributes.Get<Vector2>("Position");
            if (_pathMap == null) _pathMap = new PathMap(new Node(position, true));
            _pathMap.UpdatePathGraph(position);

        }

        private void EnablePathArt() {
            _destinationCircle.active = true;
            _destinationCircle.Draw3DAuto();
            _pathLine.active = true;
            _pathLine.Draw3DAuto();
            Parent.Attributes.Update("PathIsActive", true);
            //Parent.Attributes.Update("PathIsActive", true);
        }

        private void DisablePathArt() {
            _destinationCircle.active = false;
            _pathLine.active = false;
            Parent.Attributes.Update("PathIsActive", false);
        }

        private void OnDeselected(Entity entity) {
            if (entity != Parent) return;

            DisablePathArt();
            //ResetPath();
        }

        public void OnUpdate() {

        }

        private void DrawPathLine() {
            VectorLine.Destroy(ref _pathLine);
            _pathLine = new VectorLine("Move Path Line", _currentPath.ToArray(), null, line_thickness, LineType.Continuous);
            _pathLine.Draw3DAuto();

        }

        private void UpdatePath() {
            _currentPath = _pathMap.GetBestPath(_target, Parent).ToList();
            Parent.Attributes.Update("CurrentPath", _currentPath);
        }
        
        private PathMap _pathMap;
        private VectorLine _destinationCircle;
        private readonly float line_thickness = 2.0f;  //used by vectrosity
        private VectorLine _pathLine;
        private string _playername;
        private List<Vector3> _currentPath;
        private Vector2 _target;
        public Entity Parent { get; set; }
    }
}