using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Flow {
	
	public class FlowBehaviour : MonoBehaviour {
		
		public class LocalEventRegistar
		{
			private Dictionary<object, object> _dict = new Dictionary<object, object>();
	
			UnityEventImpl<T> FindEventImpl<T>() where T : struct
			{
				var keyType = typeof(T);
				object found;
				_dict.TryGetValue(keyType, out found);
				UnityEventImpl<T> eventImpl;
				if(found == null) {
					eventImpl = new UnityEventImpl<T>();
					_dict.Add(keyType, eventImpl);
				} else {
					eventImpl = found as UnityEventImpl<T>;
				}
				
				return eventImpl;
			}
			
			public void Trigger<T>(T value) where T : struct
			{
				FindEventImpl<T>().Invoke(value);
			}
			
			public void Listen<T>(UnityAction<T> value) where T : struct
			{
				FindEventImpl<T>().AddListener(value);
			}
	
			public void StopListening<T>(UnityAction<T> value) where T : struct
			{
				FindEventImpl<T>().RemoveListener(value);
			}
		}
		
		LocalEventRegistar innerFlow = new LocalEventRegistar();
		
		public void Trigger<T>(T eventDetails) where T : struct {
			innerFlow.Trigger<T>(eventDetails);
			EventManager.Trigger<T>(eventDetails);
		}
			
		public void Listen<T>(UnityAction<T> action) where T : struct {
			innerFlow.Listen<T>(action);
		}
			
		public void StopListening<T>(UnityAction<T> action) where T : struct {
			innerFlow.StopListening<T>(action);
		}
	}	
}