using System;
using System.Collections.Generic;
using Assets.Code.Statics;
using UnityEngine;

namespace Assets.Code.Abstract
{
    public static class EntityManager {
        public static List<Entity> MasterEntityList { get; private set; }
        public static List<Entity> PermenantEntityList { get; private set; }

        public static NodeManager NodeMgr { get; private set; }
        private static EntityManagerHelper _helper = (new GameObject().AddComponent<EntityManagerHelper>());

        static EntityManager()  {
            NodeMgr = new NodeManager();
            MasterEntityList = new List<Entity>();
            PermenantEntityList = new List<Entity>();

            Messenger.AddListener<Entity>("EntityCreated",OnEntityCreated);
            Messenger.MarkAsPermanent("EntityCreated");
            Messenger.AddListener<Entity>("EntityDestroyed", OnEntityDestroyed);
            Messenger.MarkAsPermanent("EntityDestroyed");
        }

        static void CreateEntity(EntityInitializer initializer) {
            
        }

        static public void Cleanup() {
            NodeMgr.ClearEntities();
            MasterEntityList.Clear();
            //not sure how I want to handle this yet..
            // should this delete entitys? does unity do that?
            // should this just remove items from the list?
        }
        private static void OnEntityDestroyed(Entity entity) {
            if (!MasterEntityList.Contains(entity))
                throw new Exception("An entity was destroyed, but it doesn't exist in the master list...");
            else MasterEntityList.Remove(entity);

        }

        private static void OnEntityCreated(Entity entity) {
            if(!MasterEntityList.Contains(entity)) MasterEntityList.Add(entity);
            else throw new Exception("A duplicate entity was detected by EntityManager.");

        }


        public static IEnumerable<Node> GetAllSolidNodes() {
            return NodeMgr.GetAllSolidNodes();
        }
    }

    //public abstract class Entity : MonoBehaviour, IObstructable {
        
    //    private bool _currentlySolid = true;

    //    public NodeManager.ColliderType ColliderType { get; private set; }
    //    public List<Node> CollisionNodes { get; private set; }

    //    protected void Awake() {
    //        if (Collider is CircleCollider2D) ColliderType = NodeManager.ColliderType.Circle;
    //        else if (Collider is PolygonCollider2D) ColliderType = NodeManager.ColliderType.Polygon;
    //        else if (Collider is BoxCollider2D) ColliderType = NodeManager.ColliderType.Box;
    //        else { throw new Exception("ENTITY: Could not determine collider type. ");}
    //        CollisionNodes = new List<Node>(NodeManager.GetNodes(this, 0f));

    //    }
    //    protected virtual void Start() {
    //        Messenger.Broadcast("EntityCreated", this);
    //    }
    //    public bool Solid {
    //        get {
    //            return _currentlySolid;
    //        }
    //        protected set {
    //            var changed = value != _currentlySolid;
    //            _currentlySolid = value;
    //            if(changed) UpdateNodes();
    //        }
    //    }

    //    public Collider2D Collider { get { return GetComponent<Collider2D>(); } }

    //    public virtual void Phase() {
    //        Solid = !Solid;
    //    }

    //    private void UpdateNodes() {
    //        Messenger.Broadcast(Solid ? "EntityAppeared" : "EntityDisappeared", this);
    //    }

    //    protected virtual void Update() {
            
    //    }
    //}
}
