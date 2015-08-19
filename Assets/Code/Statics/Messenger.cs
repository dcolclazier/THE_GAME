using System.Linq;
using Assets.Code.Exceptions;

namespace Assets.Code {
    // C# messenger by David Colclazier v1.0
    // Based on Rod Hyde's "CSharpMessenger" and Magnus Wolffelt's "CSharpMessenger Extended" and Ilya Suzdalnitski's "Advanced C# Messenger"

    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public delegate void Callback();
    public delegate void Callback<T>(T arg1);
    public delegate void Callback<T, T2>(T arg1, T2 arg2);
    public delegate void Callback<T, T2, T3>(T arg1, T2 arg2, T3 arg3);

    static internal class Messenger {

        // ReSharper disable once UnusedMember.Local
#pragma warning disable 0414
        private static readonly Dictionary<string, Delegate> EventTable = new Dictionary<string, Delegate>();
        private static readonly List<string> PermenantMessages = new List<string>();

        static public void MarkAsPermanent(string eventType) {
            PermenantMessages.Add(eventType);
        }

        static public void Cleanup() {
            Debug.Log("MESSENGER Cleanup. Make sure that none of necessary listeners are removed.");
 
		    var messagesToRemove = (from pair 
                                in EventTable 
                                let wasFound = PermenantMessages.Any(message => pair.Key == message) 
                                where !wasFound 
                                select pair.Key).ToList();

            foreach (var message in messagesToRemove) 
		        EventTable.Remove( message );
            PrintEventTable();

		}

        static public void PrintEventTable() {
            Debug.Log("\t\t\t=== MESSENGER PrintEventTable ===");

            foreach (var pair in EventTable) {
                Debug.Log("\t\t\t" + pair.Key + "\t\t" + pair.Value);
            }

            Debug.Log("\n");
        }

        private static void OnListenerAdding(string eventType, Delegate listenerBeingAdded) {
            
            //if the eventType doesn't exist, add it with a null listener
            if(!EventTable.ContainsKey(eventType)) EventTable.Add(eventType,null);

            //either way, though...
            var listeners = EventTable[eventType];
            if(listeners != null && listeners.GetType() != listenerBeingAdded.GetType())
                throw new ListenerException(string.Format("Tried to add listener with wrong signature for event type {0}. " +
                                                          "Current listeners have type {1} and listener being added has type {2}", 
                                                          eventType, listeners.GetType().Name, listenerBeingAdded.GetType().Name));
        }

        private static void OnListenerRemoving(string eventType, Delegate listenerBeingRemoved) {
            if (!EventTable.ContainsKey(eventType)) return;
            var listeners = EventTable[eventType];
            if (listeners == null) {
                throw new ListenerException(string.Format("Tried to remove listener for event type \"{0}\" but the current listener is null.",eventType));
            } else if (listeners.GetType() != listenerBeingRemoved.GetType()) {
                throw new ListenerException(
                    string.Format("Tried to remove listener with wrong signature for event type {0}. " +
                                  "Current listeners have type {1} and listener being added has type {2}",
                        eventType, listeners.GetType().Name, listenerBeingRemoved.GetType().Name));
            }
            //} else {
            //    throw new ListenerException(string.Format("Tried to remove listener for type \"{0}\" but Messenger didn't know about this event type.", eventType));
            //}
        }

        private static void OnListenerRemoved(string eventType) {

            if (PermenantMessages.Contains(eventType)) PermenantMessages.Remove(eventType);
            if (EventTable[eventType] == null) EventTable.Remove(eventType);
        }

        private static void OnBroadcasting(string eventType) {
            if (EventTable.ContainsKey(eventType)) return;

            Debug.Log(string.Format("Broadcasting message \"{0}\" but no listener found. Try marking the message with Messenger.MarkAsPermanent.", eventType));
        }

        private static BroadcastException CreateBroadcastSignatureException(string eventType) {
            return new BroadcastException(string.Format("Broadcasting message \"{0}\" but no listener found. Try marking the message with Messenger.MarkAsPermanent.", eventType));

        }


        static public void AddListener(string eventType, Callback handler)
        {
            OnListenerAdding(eventType, handler);
            EventTable[eventType] = (Callback)EventTable[eventType] + handler;
        }

        static public void AddListener<T>(string eventType, Callback<T> handler) {
            OnListenerAdding(eventType, handler);
            EventTable[eventType] = (Callback<T>)EventTable[eventType] + handler;
        }

        static public void AddListener<T, T2>(string eventType, Callback<T, T2> handler)
        {
            OnListenerAdding(eventType, handler);
            EventTable[eventType] = (Callback<T, T2>)EventTable[eventType] + handler;
        }

        static public void AddListener<T, T2, T3>(string eventType, Callback<T, T2, T3> handler)
        {
            OnListenerAdding(eventType, handler);
            EventTable[eventType] = (Callback<T, T2, T3>)EventTable[eventType] + handler;
        }
	    
        static public void RemoveListener(string eventType, Callback handler) {
            OnListenerRemoving(eventType, handler);   
            EventTable[eventType] = (Callback)EventTable[eventType] - handler;
            OnListenerRemoved(eventType);
        }
 
	    static public void RemoveListener<T>(string eventType, Callback<T> handler) {
            OnListenerRemoving(eventType, handler);
            EventTable[eventType] = (Callback<T>)EventTable[eventType] - handler;
            OnListenerRemoved(eventType);
        }

        static public void RemoveListener<T, T2>(string eventType, Callback<T, T2> handler)
        {
            OnListenerRemoving(eventType, handler);
            EventTable[eventType] = (Callback<T, T2>)EventTable[eventType] - handler;
            OnListenerRemoved(eventType);
        }
 
	    static public void RemoveListener<T, T2, T3>(string eventType, Callback<T, T2, T3> handler) {
            OnListenerRemoving(eventType, handler);
            EventTable[eventType] = (Callback<T, T2, T3>)EventTable[eventType] - handler;
            OnListenerRemoved(eventType);
        }
      
        static public void Broadcast(string eventType)
        {
            OnBroadcasting(eventType);

            Delegate listeners;
            if (!EventTable.TryGetValue(eventType, out listeners)) return;
            
            var callback = listeners as Callback;
            if (callback != null) callback();
            else throw CreateBroadcastSignatureException(eventType);
        }

        static public void Broadcast<T>(string eventType, T arg1)
        {
            OnBroadcasting(eventType);

            Delegate listeners;
            if (!EventTable.TryGetValue(eventType, out listeners)) return;
            
            var callback = listeners as Callback<T>;
            if (callback != null) callback(arg1);
            else throw CreateBroadcastSignatureException(eventType);
            
        }

        static public void Broadcast<T, T2>(string eventType, T arg1, T2 arg2)
        {
            OnBroadcasting(eventType);

            Delegate listeners;
            if (!EventTable.TryGetValue(eventType, out listeners)) return;
            
            var callback = listeners as Callback<T, T2>;
            if (callback != null) callback(arg1, arg2);
            else throw CreateBroadcastSignatureException(eventType);
        }

        public static void Broadcast<T, T2, T3>(string eventType, T arg1, T2 arg2, T3 arg3) {
            OnBroadcasting(eventType);

            Delegate listeners;
            if (!EventTable.TryGetValue(eventType, out listeners)) return;

            var callback = listeners as Callback<T, T2, T3>;
            if (callback != null) callback(arg1, arg2, arg3);
            else throw CreateBroadcastSignatureException(eventType);
        }
    }
}