using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoWeapon : Weapon {
	public NoWeapon(){
		damage = 0;
		type = "none";
		name = "none";
	}
}
