using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingData : MonoBehaviour {

	public List<PingInter> PingInter;

	private float[] _pingInteractable;
	private Vector4[] _originInteractable;

	private underwaterFX underwater;
	private int pingsInter;

	void Start () {
		underwater = Camera.main.GetComponent<underwaterFX> (); 

		pingsInter = PingInter.Count;
		_pingInteractable = new float[pingsInter];
		_originInteractable = new Vector4[pingsInter];

		SetupInteractables ();
	}

	void SetupInteractables () {
		for (int i = 0; i < PingInter.Count; i++) {
			_pingInteractable [i] = PingInter [i].getPing;
			_originInteractable [i] = PingInter [i].getOrigin;
		}
	}

	void Update () {
		SetupInteractables ();
		UpdateShader ();
	}

	void UpdateShader () {
		underwater._mat.SetFloatArray ("_pingInter", _pingInteractable);
		underwater._mat.SetVectorArray ("_originInter", _originInteractable);
		underwater._mat.SetInt ("_pingsInter", pingsInter);
	}

}