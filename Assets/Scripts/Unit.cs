using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//change
public class Unit : MonoBehaviour {
	List<Item> storage;
	List<Item> equipped;
	int hp;
	public int posX;
	public int posY;
	string team; 
	GameController gameController;
	int leftEquipped;
	int rightEquipped;
	public bool alive;
	int speed;
	int currentSpeed;

	public GameObject box;
	public int GetLeftIndex(){
		return leftEquipped;
	}
	public int GetRightIndex(){
		return rightEquipped;
	}
	void Awake(){
		speed = 10;
		currentSpeed = speed;
		alive = true;
		leftEquipped = 0;
		rightEquipped = 1;
		storage = new List<Item>();
		equipped = new List<Item>();
		equipped.Add(new NoWeapon());
		equipped.Add(new NoWeapon());
		equipped.Add(new Fists());
		equipped.Add(new Fists());
		equipped.Add(new Knife());
		equipped.Add(new Knife());
		storage.Add(new Fists());
		for(int i = 0; i<10; i++){
			storage.Add(new Sword());
			storage.Add(new Knife());
		}
		storage.Add(new Fists());
		gameController = GameObject.Find("GameController").GetComponent<GameController>();
		team = "none";
		posX = 0;
		posY = 0;
		hp = 100;
	}

	// Use this for initialization
	void Start () {

	}
	// Update is called once per frame
	void Update () {
	}
	public List<Item> Search(int x, int y){
		Box b = gameController.GetBox(x,y);
		if(b!=null){
			return b.GetItemList();
		}else{
			gameController.SetBox(x,y);
			b = gameController.GetBox(x,y);
			return b.GetItemList();
		}
	}
	public Item GetEquippedWeaponRight(){
		Debug.Log("ER" + rightEquipped);
		if(rightEquipped > equipped.Count - 1){
			rightEquipped = 1;
		}
		return equipped[rightEquipped];
	}
	public Item GetEquippedWeaponLeft(){
		if(leftEquipped > equipped.Count - 1){
			leftEquipped = 0;
		}
		return equipped[leftEquipped];
	}
	public List<Item> GetEquipped(){
		return equipped;
	}
	public void SetEquipped(List<Item> e){
		equipped = e;
	}
	public string GetTeam(){
		return team;
	}
	public void SetTeam(string t){
		team = t;
	}
	public List<Item> GetStorage(){
		return storage;
	}
	public void DropItemFromStorage(int i){
		if(i<storage.Count){
			gameController.DropItem(storage[i], posX, posY);
			storage.RemoveAt(i);
		}
	}
	public bool Move(int x, int y){
		if(x < gameController.gridWidth && y < gameController.gridHeight && x > -1 && y > -1){
			if(!gameController.IsUnit(x,y)){
				gameController.SetUnit(null, posX, posY);
				transform.position = gameController.GetTile(x,y).transform.position;
				posX = x;
				posY = y;
				gameController.SetUnit(this, x, y);
				return true;
			}else if(gameController.GetUnit(x,y).GetTeam() == team){ //maybe make more borad in the future
				gameController.SetUnit(null, posX, posY);
				if(gameController.IsUnit(x,y)){gameController.GetUnit(x,y).ForceMove(posX, posY);}
				transform.position = gameController.GetTile(x,y).transform.position;
				posX = x;
				posY = y;
				gameController.SetUnit(this, x, y);
				return true;
			}else if(gameController.GetUnit(x,y).GetTeam() != team){
				//attack
				Attack(x,y);
				return true;
			}else{
				return false;
			}
		}
		return false;
	}
	public void ForceMove(int x, int y){
			gameController.SetUnit(null, posX, posY);
			transform.position = gameController.GetTile(x,y).transform.position;
			posX = x;
			posY = y;
			gameController.SetUnit(this, x, y);
	}

	public bool MoveNorth(){
		return Move(posX, posY+1);
	}
	public bool MoveSouth(){
		return Move(posX, posY-1);
	}
	public bool MoveEast(){
		return Move(posX+1, posY);
	}
	public bool MoveWest(){
		return Move(posX-1, posY);
	}
	public bool MoveNorthWest(){
		return Move(posX-1, posY+1);
	}
	public bool MoveNorthEast(){
		return Move(posX+1, posY+1);
	}
	public bool MoveSouthWest(){
		return Move(posX-1, posY-1);
	}
	public bool MoveSouthEast(){
		return Move(posX+1, posY-1);
	}

	public bool TickLeft(){
		leftEquipped += 1;
		if(leftEquipped == 1){ //none
			leftEquipped += 1;
		}
		if(leftEquipped == 3){ //fist
			leftEquipped += 1;
		}
		if(leftEquipped == rightEquipped){
			leftEquipped += 1;
		}
		if(leftEquipped > equipped.Count - 1){
			leftEquipped = 0;
		}
		return false;
	}
	public void EquipInactive(){
		leftEquipped = 0;
		rightEquipped = 1;
	}
	public bool TickRight(){
		rightEquipped += 1;
		if(rightEquipped == 0){
			rightEquipped += 1;
		}
		if(rightEquipped == 2){
			rightEquipped+=1;
		}
		if(leftEquipped == rightEquipped){
			rightEquipped += 1;
		}
		if(rightEquipped > equipped.Count - 1){
			rightEquipped = 1;
		}
		return false;
	}
	public void TakeDamage(int dmg){
		hp -= dmg;
		if(hp<1){
			Die();
		}
	}
	public void Die(){
		gameController.SetUnit(null, posX, posY);
		alive = false;
	}
	public void Attack(int x, int y){
		if(gameController.IsUnit(x,y) != false){
			gameController.GetUnit(x,y).TakeDamage(equipped[leftEquipped].damage); 
		}
	}
	public int GetSpeed(){
		return speed;
	}
	public void SetSpeed(int s){
		speed = s;
	}
	public int GetCurrentSpeed(){
		return currentSpeed;
	}
	public void ResetSpeed(){
		currentSpeed = speed;
	}
	public bool TickCurrentSpeed(){
		currentSpeed -= 1;
		if(currentSpeed < 1){
			currentSpeed = speed;
			return true;
		}else{
			return false;
		}
	}
	public void MoveRandom(){
		int rx = Random.Range(posX-1,posX+2);
		int ry = Random.Range(posY-1,posY+2);
		Move(rx, ry);
	}
}
