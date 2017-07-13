using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraScript : MonoBehaviour {
	Unit playerUnit;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}	

	public void SetPlayerUnit(Unit u){
		playerUnit = u;
	}

	public void JumpToPlayer(){
		if(playerUnit != null){
			transform.position = new Vector3(playerUnit.transform.position.x, playerUnit.transform.position.y, transform.position.z);
		}
	}
}
