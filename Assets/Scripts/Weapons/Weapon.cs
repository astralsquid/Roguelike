using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item{
	public int hands;
	public Weapon(){
		hands = 1;
		damage = 1;
		name = "weapon";
		type = "none";
	}
}