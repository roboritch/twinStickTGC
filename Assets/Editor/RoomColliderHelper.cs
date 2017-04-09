using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class RoomColliderHelper : Editor {

	public static Vector2 pointLocation;
	public static int pointPos;

#if false
	public override void OnInspectorGUI() {
		base.OnInspectorGUI();
		//target script
		EdgeCollider2D myTarget = (EdgeCollider2D)target;
		addPointAtStart(myTarget);
		removePointAtStart(myTarget);
	}
#endif

	private void addPointAtStart(EdgeCollider2D myTarget) {
		//increase array size by 1 
		Vector2[] points = new Vector2[myTarget.pointCount+1];
		if (GUILayout.Button("Add point at start")) {
			if(myTarget.pointCount == 0) {
				return;
			}
			points[0] = myTarget.points[0];
            for (int i = 1; i < points.Length; i++) {
				points[i] = myTarget.points[i-1];
			}
			myTarget.points = points;
		}
	}

	private void removePointAtStart(EdgeCollider2D myTarget) {
		Vector2[] points = new Vector2[myTarget.pointCount - 1];
		if (GUILayout.Button("Remove point at start")) {
			for (int i = 0; i < points.Length; i++) {
				points[i] = myTarget.points[i + 1];
			}
			myTarget.points = points;
		}
	}

	//does no work values reset
	private void addPointMenu(EdgeCollider2D myTarget) {
		//seporator
		GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });
				
		EditorGUILayout.Vector2Field("point", pointLocation);
		//the position to insert (move point there to +1 of current position)

		EditorGUILayout.IntField("point position", pointPos);
		
		if (GUILayout.Button("Add point")) {
			if(myTarget.points.Length == 0) {
				Debug.LogWarning("must have one point");
                return;
			}
            if (pointPos > myTarget.points.Length || pointPos < 0) {
				Debug.Log("not valid point location: adding to last location");
				pointPos = myTarget.points.Length;
			}
			//increase array size by 1 
			Vector2[] points = new Vector2[myTarget.points.Length+1];
			System.Array.Copy(myTarget.points, points, myTarget.points.Length);

			//swap down to last index
			Vector2 tempP = points[pointPos];
			points[pointPos] = pointLocation;
			
			for (int i = pointPos+1; i < points.Length-1; i++) {
				points[i] = tempP;
				tempP = points[i + 1];
			}
			points[points.Length - 1] = tempP;

			myTarget.points = points;

		}

	}
}
