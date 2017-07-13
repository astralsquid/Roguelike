using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIUnitController : MonoBehaviour {
	Unit unit;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetUnit(Unit u){
		unit = u;
	}

	public void Move(int x, int y){
		unit.Move(x,y);
	}
	public Unit GetUnit(){
		return unit;
	}
	public void MoveNorth(){
		unit.MoveNorth();
	}
	public void MoveSouth(){
		unit.MoveSouth();
	}
	public void MoveEast(){
		unit.MoveEast();
	}
	public void MoveWest(){
		unit.MoveWest();
	}
	public void MoveNorthWest(){
		unit.MoveNorthWest();
	}
	public void MoveNorthEast(){
		unit.MoveNorthEast();
	}
	public void MoveSouthWest(){
		unit.MoveSouthWest();
	}
	public void MoveSouthEast(){
		unit.MoveSouthEast();
	}

	public void TakeAction(){
		if(unit.alive){
			if(unit.TickCurrentSpeed()){
				unit.ResetSpeed();
				//Take Action
				unit.MoveRandom();
			}
		}
	}
}
