using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BeamAnimation : MonoBehaviour {
	private SpriteRenderer SR;
	private float spriteLayer = -0.07f;
	private float spriteBoundsY;

	public void setBeamLength(Vector3 startLocation, Vector3 endLocation) {
		startLocation.z = spriteLayer;
		endLocation.z = spriteLayer;
		//set location and look at end location
		transform.position = startLocation;
		
		transform.up = endLocation - transform.position; //look at end location

		float size = (endLocation - startLocation).magnitude;
		Vector3 sizeVector = transform.localScale;
		sizeVector.y = (spriteBoundsY) * size;
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
		spriteBoundsY = SR.bounds.size.y;
	}


	
	// Update is called once per frame
	void Update () {
		beamAnimate();
	}
}
