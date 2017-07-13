using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item {
	public string name;
	public string type;
	public string desc;
	public int damage;
	public int hands = 0;
	public Item(){
		damage = 0;
		name = "item";
		type = "type";
		desc = "";
	}
}
