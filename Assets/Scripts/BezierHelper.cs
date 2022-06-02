using UnityEngine;

public static class BezierHelper {

	public static Vector3 GetPoint (Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t) {
		t = Mathf.Clamp01(t);
		float diff = 1f - t;
		return
			diff * diff * diff * p0 +
			3f * diff * diff * t * p1 +
			3f * diff * t * t * p2 +
			t * t * t * p3;
	}

	public static Vector3 GetFirstDerivative (Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t) {
		t = Mathf.Clamp01(t);
		float diff = 1f - t;
		return
			3f * diff * diff * (p1 - p0) +
			6f * diff * t * (p2 - p1) +
			3f * t * t * (p3 - p2);
	}
}