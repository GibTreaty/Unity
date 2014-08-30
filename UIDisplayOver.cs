using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(RectTransform))]
public class UIDisplayOver : MonoBehaviour {

	public GameObject target;
	public float offset;
	public float size = .25f;
	public bool scaleWithCamera;
	public bool pointAtCamera = true;

	void LateUpdate() {
		if(target) {
			Bounds bounds = CalculateRendererBounds();
			Vector3 position = new Vector3(bounds.center.x, bounds.max.y, bounds.center.z);
			Vector3[] corners = new Vector3[4];

			GetComponent<RectTransform>().GetWorldCorners(corners);

			float height = Mathf.Abs(corners[0].y - corners[1].y);
			position += Vector3.up * (height * .5f + offset);

			transform.position = position;
		}

		if(Camera.main) {
			if(pointAtCamera)
				transform.rotation = Camera.main.transform.rotation;

			if(scaleWithCamera) {
				float distance = Vector3.Distance(transform.position, Camera.main.transform.position);

				transform.localScale = Vector3.one * size * distance;
			}
		}
	}

	Bounds CalculateRendererBounds() {
		Bounds bounds = new Bounds();
		Renderer[] renderers = target.GetComponentsInChildren<Renderer>();

		if(target) bounds.center = target.transform.position;

		if(renderers.Length > 0) {
			bounds = renderers[0].bounds;

			for(int i = 1; i < renderers.Length; i++)
				bounds.Encapsulate(renderers[i].bounds);
		}

		return bounds;
	}
}
