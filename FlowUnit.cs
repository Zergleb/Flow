using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Flow {
	public class FlowUnit : FlowBehaviour {
		public static FlowUnit SYSTEM_UNIT = new FlowUnit();
		//Public Properties
		public int player = 0;
		public float maxHealth = 100f;
		
		public bool destroyOnDeath = true;
		
		//Private
		float health = 100f;
		bool isDead;
		
		void Start() {
			health = maxHealth;
		}

        internal void setHealth(int health)
        {
			this.health = health;
        }

        public float getHealth() {
			return health;
		}
		
		void OnMouseDown() {
			UnitSelected selected = new UnitSelected();
			selected.selectedUnit = this;
		
			this.Trigger<UnitSelected>(selected);
		}
		
		
		private readonly object damageLock = new object();
		public void TakeDamage(FlowUnit damagingUnit, float damage) {
			lock(damageLock) {
				if(isDead) {
					//return;
				}
			
				var ev = new UnitDamaged();
				ev.damageTaken = damage;
				ev.damagingUnit = damagingUnit;
				ev.damagedUnit = this;
			
				health -= damage;
			
				this.Trigger<UnitDamaged>(ev);
				if(health <= 0) {
					isDead = true;
					var death = new UnitDied();
					death.damageTaken = damage;
					death.damagingUnit = damagingUnit;
					death.deadUnit = this;
					this.Trigger<UnitDied>(death);
					if(destroyOnDeath) {
						FlowPool.Recycle(this.gameObject);
					}
				}
			}
		}
		
		public void Kill(FlowUnit damagingUnit) {
			TakeDamage(damagingUnit, health);
		}
		
		[InspectorGadgets.Button]
		public void ClickToKill() {
			if(Application.isPlaying) {
				Kill(this);
			}
		}
	}
	
	public struct UnitDamaged {
		public FlowUnit damagedUnit;
		public FlowUnit damagingUnit;
		public float damageTaken;
	}
	
	public struct UnitDied {
		public FlowUnit deadUnit;
		public FlowUnit damagingUnit;
		public float damageTaken;
	}

	public struct UnitSelected {
		public FlowUnit selectedUnit;
	}
}
