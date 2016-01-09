using UnityEngine;
using System.Collections;

[DisallowMultipleComponent]
[ExecuteInEditMode]
public class Part : MonoBehaviour
{
	[SerializeField]
	private GameObject
		_collection = null;
	[SerializeField]
	private float
		_frame = 0.0f;
	private bool changed = false;

	public string part {
		get {
			if (_collection == null || _frame == 0.0f) {
				return "None";
			} else {
				return _collection.transform.GetChild (Mathf.FloorToInt (_frame) - 1).gameObject.name;
			}
		}
		set {
			value = value.Replace (" ", "_");
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

	public int frame {
		get {
			return Mathf.FloorToInt (_frame);
		}
		set {
			_frame = (float)value;
			changed = true;
		}
	}

	void OnValidate ()
	{
		changed = true;
	}

	void OnDidApplyAnimationProperties ()
	{
		changed = true;
	}

	void Update ()
	{
		if (changed) {

			changed = false;

			MeshFilter meshFilter = GetComponent<MeshFilter> ();
			MeshRenderer meshRenderer = GetComponent<MeshRenderer> ();

			if (_collection != null && _frame != 0.0f) {
				GameObject found = _collection.transform.GetChild (Mathf.FloorToInt (_frame) - 1).gameObject;
				meshFilter.mesh = found.GetComponent<MeshFilter> ().sharedMesh;
				meshRenderer.material = found.GetComponent<MeshRenderer> ().sharedMaterial;
			} else {
				meshFilter.mesh = null;
				meshRenderer.material = null;
			}
		}
	}
}