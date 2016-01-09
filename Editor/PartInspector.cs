using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Part))]
public class PartInspector : Editor
{
	SerializedProperty _collection;
	SerializedProperty _frame;
	private Part part;
	GameObject collection = null;

	void OnEnable ()
	{
		_collection = serializedObject.FindProperty ("_collection");
		_frame = serializedObject.FindProperty ("_frame");

		part = (Part)target;
		MeshFilter meshFilter;
		MeshRenderer meshRenderer;

		meshFilter = part.gameObject.GetComponent<MeshFilter> ();
		if (meshFilter == null) {
			meshFilter = part.gameObject.AddComponent<MeshFilter> ();
		}			
		meshFilter.hideFlags = HideFlags.HideInInspector;

		meshRenderer = part.gameObject.GetComponent<MeshRenderer> ();
		if (meshRenderer == null) {
			meshRenderer = part.gameObject.AddComponent<MeshRenderer> ();
		}			
		meshRenderer.hideFlags = HideFlags.HideInInspector;
	}

	public override void OnInspectorGUI ()
	{
		serializedObject.Update ();

		// Show field to select collection.
		EditorGUILayout.PropertyField (_collection);

		// Get game object stored in serialized property.
		collection = (GameObject)_collection.objectReferenceValue;

		bool errored = false;

		if (collection == null) {
			EditorGUILayout.HelpBox ("Please select a collection to use with this part.", MessageType.Info);
			errored = true;
		}
		// Empty collection found.  Display error warning.
		else if (collection.transform.childCount < 1) {
			EditorGUILayout.HelpBox ("Invalid collection.  Please choose another collection.", MessageType.Error);
			errored = true;
		}

		int index = Mathf.FloorToInt (_frame.floatValue);
		if (!errored && index > collection.transform.childCount) {
			index = 0;
			_frame.floatValue = (float)index;
		}
		if (!errored) {
			ArrayList options = new ArrayList ();
			options.Add ("None");
			foreach (Transform child in collection.transform) {
				options.Add (child.name);
			}
			index = EditorGUILayout.Popup ("Part", index, (string[])options.ToArray (typeof(string)), EditorStyles.popup);
		}

		if (GUI.changed) {
			if (collection == null) {
				index = 0;
			}
			_frame.floatValue = (float)index;
		}

		serializedObject.ApplyModifiedProperties ();
	}
}