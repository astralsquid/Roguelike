using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour {
	public List<Item> items;
	// Use this for initialization
	void Awake(){
		items = new List<Item>();
	}

	void Start () {
		
	}
	// Update is called once per frame
	void Update () {
		
	}
	void LateUpdate(){

	}
	public void AddItem(Item i){
		Debug.Log(i.name);
		items.Add(i);
	}
	public Item GetItem(int i){
		return items[i];
	}
	public List<Item> GetItemList(){
		return items;
	}
	public void DeleteItem(int i){
		items.RemoveAt(i);
	}
	public void DestroyIfEmpty(){
		Debug.Log("destroying");
		if(items.Count < 1){
			 Destroy(gameObject);
		}
	}
}