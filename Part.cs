using UnityEngine;
using System.Collections;

[DisallowMultipleComponent]
[ExecuteInEditMode]
public class Part : MonoBehaviour
{
	/*
	 *	Selected Game Object which stores the Game Object's used for mesh/material referencing.
	 */
	[SerializeField]
	private GameObject
		_collection = null;

	/*
	 *	Currently selected mesh/material pair from the collection.
	 *	Due to Unity limitations with the animation engine, had to be stored as float and proxied as below.
	 */	
	[SerializeField]
	private float
		_frame = 0.0f;

	//	Let's the object know it needs to change its mesh/material during its next Update.
	private bool changed = false;

	/*	
	 *	String proxy for frame, since strings cannot be used for animation and Unity limitations on keeping values in sync.
	 */
	public string part {
		get {
			if (_collection == null || frame == 0) {
				return "None";
			} else {
				return _collection.transform.GetChild (frame - 1).gameObject.name;
			}
		}
		set {
			if (_collection == null || value == "None") {
				frame = 0;
			} else {
				int index = _collection.transform.FindChild (value).GetSiblingIndex ();
				if (index != -1) {
					frame = index + 1;
				} else {
					frame = 0;
				}
			}
		}
	}

	/*	
	 *	Integer proxy for frame, since integers cannot be used for animation and Unity limitations on keeping values in sync.
	 */
	public int frame {
		get {
			return Mathf.FloorToInt (_frame);
		}
		set {
			_frame = (float)value;
			changed = true;
		}
	}

	/*
	 *	Evaluated when Unity changes a value.  Used to tell mesh/material to change.
	 */
	void OnValidate ()
	{
		changed = true;
	}

	/*
	 *	Evaluated when Unity's animation system changes a value.  Used to tell mesh/material to change.
	 */
	void OnDidApplyAnimationProperties ()
	{
		changed = true;
	}

	/*
	 *	Sets mesh/material based on current frame.
	 */
	void Update ()
	{
		if (changed) {

			changed = false;

			MeshFilter meshFilter = GetComponent<MeshFilter> ();
			MeshRenderer meshRenderer = GetComponent<MeshRenderer> ();

			if (_collection != null && frame != 0) {
				GameObject found = _collection.transform.GetChild (frame - 1).gameObject;
				meshFilter.mesh = found.GetComponent<MeshFilter> ().sharedMesh;
				meshRenderer.material = found.GetComponent<MeshRenderer> ().sharedMaterial;
			} else {
				meshFilter.mesh = null;
				meshRenderer.material = null;
			}
		}
	}
}