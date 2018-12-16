using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Flow {	
	public class EventManager {
		public static class EventTypeRegister<T> {
			public static UnityEventImpl<T> unityEvent = new UnityEventImpl<T>();
		}
			
		public static void Trigger<T>(T eventDetails) {
			EventTypeRegister<T>.unityEvent.Invoke(eventDetails);
		}
		
		public static void Listen<T>(UnityAction<T> action) {
			EventTypeRegister<T>.unityEvent.AddListener(action);
		}
		
		public static void StopListening<T>(UnityAction<T> action) {
			EventTypeRegister<T>.unityEvent.RemoveListener(action);
		}
	}
	
	[System.Serializable]
	public class UnityEventImpl<T> : UnityEvent<T>
	{
	}
}
