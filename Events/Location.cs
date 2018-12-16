using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Flow {
	public class Location : MonoBehaviour {
		protected void OnTriggerEnter(Collider other)
		{
			LocationEntered triggerData = new LocationEntered();
			
			triggerData.locationEntered = this.gameObject;
			triggerData.triggerCollider = other;
			
			EventManager.Trigger<LocationEntered>(triggerData);
		}
	}
}
