using System;
using System.Collections.Generic;
using Assets.Code.Entities;
using Assets.Code.Movement;
using Assets.Code.Scripts;
using UnityEngine;

namespace Assets.Code.Statics
{
    public static class EntityManager {
        public static List<Entity> MasterEntityList { get; private set; }
        public static List<Entity> PermenantEntityList { get; private set; }

        public static NodeManager NodeMgr { get; private set; }


        static EntityManager()  {
            NodeMgr = new NodeManager();
            MasterEntityList = new List<Entity>();
            //PermenantEntityList = new List<Entity>();

            Messenger.AddListener<Entity>("EntityCreated",OnEntityCreated);
            Messenger.MarkAsPermanent("EntityCreated");
            Messenger.AddListener<Entity>("EntityDestroyed", OnEntityDestroyed);
            Messenger.MarkAsPermanent("EntityDestroyed");
        }

        static public void Cleanup() {
            //NodeMgr.ClearEntities();
            //MasterEntityList.Clear();
            //not sure how I want to handle this yet..
            // should this delete entitys? does unity do that?
            // should this just remove items from the list?
        }
        private static void OnEntityDestroyed(Entity entity) {
            if (!MasterEntityList.Contains(entity))
                throw new Exception("An entity was destroyed, but it doesn't exist in the master list...");
            
            MasterEntityList.Remove(entity);

        }

        private static void OnEntityCreated(Entity entity) {
            if(!MasterEntityList.Contains(entity)) MasterEntityList.Add(entity);
            else throw new Exception("A duplicate entity was detected by EntityManager.");

        }


        public static IEnumerable<Node> GetAllSolidNodes() {
            return NodeManager.GetAllSolidNodes();
        }

        public static IEnumerable<Node> GetNodesForEntity(Entity entity) {
            return NodeMgr.GetNodes(entity);
        }
    }
}
