using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//change
public class GameController : MonoBehaviour {
	public GameObject tile;
	public int gridHeight;
	public int gridWidth;
	public GameObject auc;

	public GameObject unit;
	public GameObject box;
	public PlayerUnitController puc;
	Unit playerUnit;

	List<AIUnitController> aucList;

	GameObject[] tiles;
	Box[] boxes;
	Unit[] units;

	// Use this for initialization
	void Start () {
		InitializeLists();
		CreateLevel();
		SpawnPlayerUnit();
		SpawnActors();
	}

	void InitializeLists(){
		boxes = new Box[gridHeight * gridWidth];
	}
	
	public void DropItem(Item i, int x, int y){
		Box b = GetBox(x,y);
		if(b == null){
			SetBox(x, y);
			b = GetBox(x,y);
		}
		b.AddItem(i);
	}
	public Box GetBox(int x, int y){
		return boxes[x*gridHeight + y];
	}
	public void SetBox(int x, int y){
	if(GetBox(x,y) == null){
		GameObject boxObject = Instantiate(box, GetTile(x,y).transform.position, Quaternion.identity);
		boxes[x*gridHeight + y] = boxObject.GetComponent<Box>();
	}
		//return boxObject.GetComponent<Box>();
	}
	void SpawnActors(){
		aucList = new List<AIUnitController>();
		AIUnitController newAuc = Instantiate(auc, new Vector3(0,0,0), Quaternion.identity).GetComponent<AIUnitController>();
		aucList.Add(newAuc);
		Unit newUnit = Instantiate(unit, new Vector3(0,0,0), Quaternion.identity).GetComponent<Unit>();
		newUnit.SetTeam("none");
		newAuc.SetUnit(newUnit);
		newAuc.Move(2,2);
	}
	public void ClearBoxes(int posX, int posY){
		for(int x = posX -1; x < posX + 2; x++){
			for(int y = posY - 1; y < posY + 2; y++){
				if(x > -1 && y > -1){
					if(x < gridWidth && y < gridHeight){
						if(GetBox(x,y)!=null){
							GetBox(x,y).DestroyIfEmpty();
						}
					}
				}
			}
		}
	}
	public void TakeTurn(){
		//StartCoroutine(TakeActions());
		while(!playerUnit.TickCurrentSpeed()){
			for(int i = 0; i<aucList.Count; i++){
				aucList[i].TakeAction();
			}
		}
	}

	IEnumerator TakeActions(){
		yield return new WaitForSeconds(1.0f);
		Debug.Log("taken");
	}

	public bool IsUnit(int x, int y){
		return (GetUnit(x,y) != null);
	}

	void SpawnPlayerUnit(){
		GameObject newPlayerUnit = Instantiate(unit, new Vector3(0,0,0), Quaternion.identity);
		puc.SetPlayerUnit(newPlayerUnit.GetComponent<Unit>());
		puc.FillUI();
		newPlayerUnit.transform.position = GetTile(0,0).transform.position;
		playerUnit = newPlayerUnit.GetComponent<Unit>();
	}

	void CreateLevel(){
		units = new Unit[gridHeight*gridWidth];
		LayTiles();
	}

	void LayTiles(){
		tiles = new GameObject[gridHeight*gridWidth];
		for(int x = 0; x<gridWidth; x++){
			for(int y = 0; y<gridHeight; y++){
				GameObject newTile = Instantiate(tile, new Vector3(x,y,0), Quaternion.identity);
				SetTile(newTile, x,y);
			}
		}
	}
	
	void SpawnPlayer(){

	}

	public void SetTile(GameObject g, int x, int y){
		tiles[x * gridHeight + y] = g;
	}
	public GameObject GetTile(int x, int y){
		return tiles[x * gridHeight + y];
	}
	public void SetUnit(Unit u, int x, int y){
		units[x * gridHeight + y] = u;
	}
	public Unit GetUnit(int x, int y){
		return units[x * gridHeight + y];
	}
	// Update is called once per frame
	void Update () {
		
	}
}
