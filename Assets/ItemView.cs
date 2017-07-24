using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemView : MonoBehaviour {
	public List<GameObject> selectionSpots;
	public GameObject cursor;
	public List<GameObject> options;
	public List<Item> itemList;
	public Scrollbar scrollbar;
	int index = 0;
	int cursorIndex = 0;
	int offset = 0;
	// Use this for initialization
	void Start () {
		itemList = new List<Item>();
		FillOptions(0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public int GetCurrentIndex(){
		return index;
	}
	public void NextOption(){
		Debug.Log(index);
		if(index < itemList.Count - 1){
			index += 1;
			if(cursorIndex < options.Count - 1){
				cursorIndex += 1;
			}else{
				offset += 1;
			}
			FillOptions(offset);
			cursor.transform.position = selectionSpots[cursorIndex].transform.position;
		}
	}
	public void PrevOption(){
		if(index > 0){
			index -= 1;
			if(cursorIndex > 0){
				cursorIndex -= 1;
			}else{
				offset -= 1;
			}
			FillOptions(offset);
			cursor.transform.position = selectionSpots[cursorIndex].transform.position;
		}
	}
	public void FillOptions(int offset){
		scrollbar.size = 1.0f;
		if(itemList.Count > 1){
			scrollbar.value = ((float)index)/((float)itemList.Count-1);
			scrollbar.size = 10.0f/itemList.Count;
		}else{
			scrollbar.value = 0;
		}
		for(int i = 0; i<options.Count; i++){
			Text name = options[i].transform.Find("Name").GetComponent<Text>();
			if(i+offset < itemList.Count){
				name.text = itemList[i+offset].name;
			}else{
				name.text = "";
			}
		}
	}
	public void Reset(){
			index = 0;
			cursorIndex = 0;
			cursor.transform.position = selectionSpots[0].transform.position;
			offset = 0;
			FillOptions(offset);
			if(itemList.Count -1 > 0){scrollbar.value = ((float)index)/((float)itemList.Count-1);}
			else{scrollbar.value=0;}
			if(itemList.Count > 10){scrollbar.size = 10.0f/itemList.Count;}
			else{scrollbar.size = 1.0f;}
	}
	public void SoftReset(List<Item> il){
		itemList = il;
		FillOptions(offset);
		if(itemList.Count -1 > 0){scrollbar.value = ((float)index)/((float)itemList.Count-1);}
		else{scrollbar.value=0;}
		if(itemList.Count > 10){scrollbar.size = 10.0f/itemList.Count;}
		else{scrollbar.size = 1.0f;}
		if(index > itemList.Count - 1 && index != 0){
			index -= 1;
			cursorIndex -= 1;
			cursor.transform.position = selectionSpots[cursorIndex].transform.position;
		}
	}

	public void SoftReset(){
		SoftReset(itemList);
	}
	public void Reset(List<Item> il){
		itemList = il;
		Reset();
	}

	public Item GetItem(){
		if(itemList.Count > 0){
			Item ite = itemList[index];
			itemList.RemoveAt(index);
			return ite;
		}else{
			return null;
		}
	}

	public void AddItem(Item i){
		if(i!= null){
			itemList.Add(i);
		}
	}
}