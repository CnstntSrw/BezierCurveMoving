using UnityEngine;

public class BezierCurve : MonoBehaviour {

	public Vector3[] points;
	
	public Vector3 GetPoint (float k) {
		return BezierHelper.GetPoint(points[0], points[1], points[2], points[3], k);
	}
	
	public Vector3 GetVelocity (float k) {
		return BezierHelper.GetFirstDerivative(points[0], points[1], points[2], points[3], k) - transform.position;
	}
	
	public Vector3 GetDirection (float k) {
		return GetVelocity(k).normalized;
	}
}