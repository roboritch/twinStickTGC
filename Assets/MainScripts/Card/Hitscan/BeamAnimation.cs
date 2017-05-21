using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BeamAnimation : MonoBehaviour {
	private SpriteRenderer SR;
	private float spriteLayer = -0.07f;

	public void setBeamLength(Vector2 startLocation, Vector2 endLocation) {
		Vector3 startLocation3D = startLocation;
		Vector3 endLocation3D = endLocation;

		startLocation3D.z = spriteLayer;
		endLocation3D.z = spriteLayer;
		//set location and look at end location
		transform.position = startLocation3D;
		
        transform.up = endLocation3D - transform.position; //look at end location

		float size = (endLocation3D - startLocation3D).magnitude;
		Vector3 sizeVector = transform.localScale;
		sizeVector.y = size;
		transform.localScale = sizeVector;
	}

	/// <summary>
	/// |-------------|
	/// width of the beam (from start of sprit.x to end)
	/// Mesure using empties if necasary
	/// </summary>
	[SerializeField]
	private float currentWidth = 0.48f;
	public void setBeamWidth(float wantendWidth) {
		Vector3 newScale = transform.localScale;

		newScale.x = wantendWidth/currentWidth;
		transform.localScale = newScale;
	}

	public void setBeamColor(Color color) {
		SR.color = color;
	}

	public void startBeamAnimation(float disipationAmount) {
		//disipation amount effected by inital beam size
		disipationAmountPerSecond = disipationAmount * transform.localScale.x;
		animate = true;
	}

	private float disipationAmountPerSecond = 2f;
	private float disipationEndpoint = 0.2f;
	private bool animate = false;
	private float animationLengthLeft;
	private void beamAnimate() {
		if (!animate) {
			return;
		}
		float incAmountThisFrame = disipationAmountPerSecond * Time.deltaTime;
		Vector3 newScale = transform.localScale;
		if(newScale.x <= disipationEndpoint) {
			Destroy(gameObject);
			return;
		}
		newScale.x = newScale.x - incAmountThisFrame;
		transform.localScale = newScale;
	}

	// Use this for initialization
	void Awake () {
		SR = GetComponent<SpriteRenderer>();
	}

	void Start() {
		
	}


	
	// Update is called once per frame
	void Update () {
		beamAnimate();
	}
}
