using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO this should be refactored to be a clojure type @tom
//By that I mean apply transactions and then reapply if conflicted
//For the case of expressions maybe we need a way to grab values as a string
public static class PropertyManager<T> where T : class, new() {
	public static T value = new T();
}
