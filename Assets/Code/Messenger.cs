using System.Linq;

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

    public class BroadcastException : Exception {
        public BroadcastException(string msg) : base(msg) { }
    }

    public class ListenerException : Exception {
        public ListenerException(string msg)
            : base(msg) { }
    }
    public sealed class MessengerHelper :   MonoBehaviour {
        void Awake() {
            DontDestroyOnLoad(gameObject);
        }
        public void OnLevelWasLoaded(int unused) {
            Messenger.Cleanup();
        }
    }
    static internal class Messenger {
         
        static private MessengerHelper _messengerHelper = (new GameObject("MessengerHelper")).AddComponent<MessengerHelper>();

        static public Dictionary<string, Delegate> eventTable = new Dictionary<string, Delegate>();
        static public List<string> permenantMessages = new List<string>();

        static public void MarkAsPermanent(string eventType) {
            permenantMessages.Add(eventType);
        }

        static public void Cleanup() {
            Debug.Log("MESSENGER Cleanup. Make sure that none of necessary listeners are removed.");
 
		var messagesToRemove = (from pair 
                                in eventTable 
                                let wasFound = permenantMessages.Any(message => pair.Key == message) 
                                where !wasFound 
                                select pair.Key).ToList();

        foreach (var message in messagesToRemove) 
		    eventTable.Remove( message );
		}

        static public void PrintEventTable() {
            Debug.Log("\t\t\t=== MESSENGER PrintEventTable ===");

            foreach (KeyValuePair<string, Delegate> pair in eventTable)
            {
                Debug.Log("\t\t\t" + pair.Key + "\t\t" + pair.Value);
            }

            Debug.Log("\n");
        }

        static public void OnListenerAdding(string eventType, Delegate listenerBeingAdded) {
            
            //if the eventType doesn't exist, add it with a null listener
            if(!eventTable.ContainsKey(eventType)) eventTable.Add(eventType,null);

            //either way, though...
            var listeners = eventTable[eventType];
            if(listeners != null && listeners.GetType() != listenerBeingAdded.GetType())
                throw new ListenerException(string.Format("Tried to add listener with wrong signature for event type {0}. " +
                                                          "Current listeners have type {1} and listener being added has type {2}", 
                                                          eventType, listeners.GetType().Name, listenerBeingAdded.GetType().Name));
        }

        static public void OnListenerRemoving(string eventType, Delegate listenerBeingRemoved) {
            if (!eventTable.ContainsKey(eventType)) return;
            var listeners = eventTable[eventType];
            if (listeners == null) {
                throw new ListenerException(string.Format("Tried to remove listener for event type \"{0}\" but the current listener is null.",eventType));
            } else if (listeners.GetType() != listenerBeingRemoved.GetType()) {
                throw new ListenerException( string.Format("Tried to remove listener with wrong signature for event type {0}. " +
                                                           "Current listeners have type {1} and listener being added has type {2}",
                                                            eventType, listeners.GetType().Name, listenerBeingRemoved.GetType().Name));
            } else {
                throw new ListenerException(string.Format("Tried to remove listener for type \"{0}\" but Messenger didn't know about this event type.", eventType));
            }
        }

        static public void OnListenerRemoved(string eventType) {
            if (eventTable[eventType] == null) {
                eventTable.Remove(eventType);
            }
        }

        static public void OnBroadcasting(string eventType) {
            if (!eventTable.ContainsKey(eventType)) {
                throw new BroadcastException(string.Format("Broadcasting message \"{0}\" but no listener found. Try marking the message with Messenger.MarkAsPermanent.", eventType));
            }
        }

        static public BroadcastException CreateBroadcastSignatureException(string eventType) {
            return new BroadcastException(string.Format("Broadcasting message \"{0}\" but no listener found. Try marking the message with Messenger.MarkAsPermanent.", eventType));

        }


        static public void AddListener(string eventType, Callback handler)
        {
            OnListenerAdding(eventType, handler);
            eventTable[eventType] = (Callback)eventTable[eventType] + handler;
        }

        static public void AddListener<T>(string eventType, Callback<T> handler) {
            OnListenerAdding(eventType, handler);
            eventTable[eventType] = (Callback<T>)eventTable[eventType] + handler;
        }

        static public void AddListener<T, T2>(string eventType, Callback<T, T2> handler)
        {
            OnListenerAdding(eventType, handler);
            eventTable[eventType] = (Callback<T, T2>)eventTable[eventType] + handler;
        }

        static public void AddListener<T, T2, T3>(string eventType, Callback<T, T2, T3> handler)
        {
            OnListenerAdding(eventType, handler);
            eventTable[eventType] = (Callback<T, T2, T3>)eventTable[eventType] + handler;
        }
	    
        static public void RemoveListener(string eventType, Callback handler) {
            OnListenerRemoving(eventType, handler);   
            eventTable[eventType] = (Callback)eventTable[eventType] - handler;
            OnListenerRemoved(eventType);
        }
 
	    static public void RemoveListener<T>(string eventType, Callback<T> handler) {
            OnListenerRemoving(eventType, handler);
            eventTable[eventType] = (Callback<T>)eventTable[eventType] - handler;
            OnListenerRemoved(eventType);
        }

        static public void RemoveListener<T, T2>(string eventType, Callback<T, T2> handler)
        {
            OnListenerRemoving(eventType, handler);
            eventTable[eventType] = (Callback<T, T2>)eventTable[eventType] - handler;
            OnListenerRemoved(eventType);
        }
 
	    static public void RemoveListener<T, T2, T3>(string eventType, Callback<T, T2, T3> handler) {
            OnListenerRemoving(eventType, handler);
            eventTable[eventType] = (Callback<T, T2, T3>)eventTable[eventType] - handler;
            OnListenerRemoved(eventType);
        }
      
        static public void Broadcast(string eventType)
        {
            OnBroadcasting(eventType);

            Delegate listeners;
            if (!eventTable.TryGetValue(eventType, out listeners)) return;
            
            var callback = listeners as Callback;
            if (callback != null) callback();
            else throw CreateBroadcastSignatureException(eventType);
        }

        static public void Broadcast<T>(string eventType, T arg1)
        {
            OnBroadcasting(eventType);

            Delegate listeners;
            if (!eventTable.TryGetValue(eventType, out listeners)) return;
            
            var callback = listeners as Callback<T>;
            if (callback != null) callback(arg1);
            else throw CreateBroadcastSignatureException(eventType);
            
        }

        static public void Broadcast<T, T2>(string eventType, T arg1, T2 arg2)
        {
            OnBroadcasting(eventType);

            Delegate listeners;
            if (!eventTable.TryGetValue(eventType, out listeners)) return;
            
            var callback = listeners as Callback<T, T2>;
            if (callback != null) callback(arg1, arg2);
            else throw CreateBroadcastSignatureException(eventType);
        }

        public static void Broadcast<T, T2, T3>(string eventType, T arg1, T2 arg2, T3 arg3) {
            OnBroadcasting(eventType);

            Delegate listeners;
            if (!eventTable.TryGetValue(eventType, out listeners)) return;

            var callback = listeners as Callback<T, T2, T3>;
            if (callback != null) callback(arg1, arg2, arg3);
            else throw CreateBroadcastSignatureException(eventType);
        }
    }
}