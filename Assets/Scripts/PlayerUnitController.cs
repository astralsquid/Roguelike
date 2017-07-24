using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//
public class PlayerUnitController : MonoBehaviour {
	enum InputModes {movement, inventory, search};
	InputModes inputMode;
	Unit playerUnit;
	public GameObject centerUIMarker;
	public MainCameraScript mainCameraScript;
	public GameObject leftPanel;
	public GameObject leftNonePanel;

	public Text weaponNameTextLeft;
	public Text weaponDamageTextLeft;
	public Text weaponTypeTextLeft;

	public GameObject rightPanel;
	public GameObject rightNonePanel;

	public Text weaponNameTextRight;
	public Text weaponDamageTextRight;
	public Text weaponTypeTextRight;

	public GameObject inventoryPanel;

	public Text invLeftText;
	public Text invRightText;

	// Use this for initialization
	bool canMoveNorth = true;
	bool canMoveSouth = true;
	bool canMoveEast = true;
	bool canMoveWest= true;
	bool canMoveNorthWest = true;
	bool canMoveSouthWest = true;
	bool canMoveNorthEast = true;
	bool canMoveSouthEast= true;
	bool canTickLeft = true;
	bool canTickRight = true;
	bool canOpenInventory = true;
	bool canCloseInventory = false;
	bool canDrop = true;
	bool canSearch = true;
	bool canSwap = true;
	bool onRightPanel = true;

	public GameController gameController;
	public ItemView storageView;
	public ItemView secondaryStorageView;

	List<Item> baseItems; 
	void Start () {
		inputMode = InputModes.movement;
	}
	
	// Update is called once per frame
	void Update () {
		HandleInputs();
		mainCameraScript.JumpToPlayer();
	}	

	void HandleInputs(){
		if(inputMode == InputModes.movement){
			if(ScanForBaseInputs()){
					gameController.TakeTurn();
				}
		}else if(inputMode == InputModes.inventory){
			ScanForMenuInputs();
		}else if(inputMode == InputModes.search){
			ScanForSearchInput();
		}
	}

	void SetSecondaryList(List<Item> il){
		secondaryStorageView.Reset(il);
	}

	void OpenInventory(){
		invRightText.text = "Sack";
		invLeftText.text = "Equipped";
		baseItems = new List<Item>();
		for(int i = 0; i<4; i++){
			baseItems.Add(playerUnit.GetEquipped()[0]);
			playerUnit.GetEquipped().RemoveAt(0);
		}
		inputMode = InputModes.inventory;
		inventoryPanel.transform.position = centerUIMarker.transform.position;
		storageView.Reset(playerUnit.GetStorage());
	}
	void CloseInventory(){
		for(int i = 0; i<playerUnit.GetEquipped().Count; i++){
			baseItems.Add(playerUnit.GetEquipped()[i]);
		}
		playerUnit.SetEquipped(baseItems);
		inventoryPanel.transform.position = new Vector3(-1000,-1000,-1000);
		storageView.Reset(playerUnit.GetStorage());
		inputMode = InputModes.movement;
		gameController.ClearBoxes(playerUnit.posX, playerUnit.posY);

	}
	void DropItem(int i){
		if(onRightPanel){
			Item newItem = storageView.GetItem();
			gameController.DropItem(newItem, playerUnit.posX, playerUnit.posY);
			storageView.SoftReset();
		}else{
			if(secondaryStorageView.GetCurrentIndex() == playerUnit.GetLeftIndex()-4){
				playerUnit.EquipInactive();
				FillUIReset();
			}
			if(secondaryStorageView.GetCurrentIndex() == playerUnit.GetRightIndex()-4){
				playerUnit.EquipInactive();
				FillUIReset();
			}
			Item newItem = secondaryStorageView.GetItem();
			gameController.DropItem(newItem, playerUnit.posX, playerUnit.posY);
			secondaryStorageView.SoftReset();
		}
	}
	void ScanForMenuInputs(){
		if (Input.GetAxisRaw ("MoveNorth") != 0 && canMoveNorth){
			canMoveNorth = false;
			if(onRightPanel) {storageView.PrevOption();}
			else{secondaryStorageView.PrevOption();}
		}else if(Input.GetAxisRaw ("MoveNorth") == 0){
			canMoveNorth = true;
		}
		if (Input.GetAxisRaw ("MoveSouth") != 0 && canMoveSouth) {
			canMoveSouth = false;
			if(onRightPanel){storageView.NextOption();}
			else{secondaryStorageView.NextOption();}
		}else if(Input.GetAxisRaw ("MoveSouth") == 0){
			canMoveSouth = true;
		}

		if (Input.GetAxisRaw ("MoveWest") != 0 && canMoveWest){
			canMoveWest = false;
			SwitchInventoryFocus();
		}else if(Input.GetAxisRaw ("MoveWest") == 0){
			canMoveWest = true;
		}

		if (Input.GetAxisRaw ("MoveEast") != 0 && canMoveEast) {
			canMoveEast = false;
			SwitchInventoryFocus();
		}else if(Input.GetAxisRaw ("MoveEast") == 0){
			canMoveEast = true;
		}

		if(Input.GetAxisRaw("OpenInventory") != 0 && canOpenInventory){
			CloseInventory();
			canOpenInventory = false;
		}else if(Input.GetAxisRaw("OpenInventory") == 0){
			canOpenInventory = true;
		}
		if(Input.GetAxisRaw("Drop") != 0 && canDrop){
			if(onRightPanel){DropItem(storageView.GetCurrentIndex());}
			else{DropItem(secondaryStorageView.GetCurrentIndex());}
			canDrop = false;
		}else if(Input.GetAxisRaw("Drop") == 0){
			canDrop = true;
		}
		if(Input.GetAxisRaw("Search") != 0 && canSearch){
			//searchMode = true;
			CloseInventory();
			inputMode = InputModes.search;
			canSearch = false;
		}else if(Input.GetAxisRaw("Search") == 0){
			canSearch = true;
		}
		if(Input.GetAxisRaw("Swap") != 0 && canSwap){
			canSwap = false;
			if(onRightPanel){
				secondaryStorageView.AddItem(storageView.GetItem());
				storageView.SoftReset();
				secondaryStorageView.SoftReset();
			}else{
				storageView.AddItem(secondaryStorageView.GetItem());
				storageView.SoftReset();
				secondaryStorageView.SoftReset();
			}
		}else if(Input.GetAxisRaw("Swap") == 0){
			canSwap = true;
		}
	}
	void ScanForSearchInput(){
		if (Input.GetAxisRaw ("Wait") != 0){
			OpenInventory();
			SetSecondaryList(playerUnit.Search(playerUnit.posX, playerUnit.posY));
			//searchMode = false;
			inputMode = InputModes.inventory;
		}
		if (Input.GetAxisRaw ("MoveNorth") != 0){
			
		}
		if (Input.GetAxisRaw ("MoveSouth") != 0 ) {
			
		}
		if (Input.GetAxisRaw ("MoveEast") != 0 ) {
			
		}
		if (Input.GetAxisRaw ("MoveWest") != 0 ) {
			
		} 
		if (Input.GetAxisRaw ("MoveNorthWest") != 0 ){

		}
		if (Input.GetAxisRaw ("MoveSouthWest") != 0 ) {

		}
		if (Input.GetAxisRaw ("MoveNorthEast") != 0 ) {

		}
		if (Input.GetAxisRaw ("MoveSouthEast") != 0 ) {

		}
		if (Input.GetAxisRaw ("MoveSouthEast") != 0 ) {

		} 
		if(Input.GetAxisRaw("Search") != 0 && canSearch){
			//searchMode = false;
		}else if(Input.GetAxisRaw("Search") == 0){
			canSearch = true;
		}
	}
	

	bool ScanForBaseInputs(){
		if (Input.GetAxisRaw ("MoveNorth") != 0 && canMoveNorth){
			canMoveNorth = false;
			return playerUnit.MoveNorth ();
		}else if(Input.GetAxisRaw ("MoveNorth") == 0){
			canMoveNorth = true;
		}

		if (Input.GetAxisRaw ("MoveSouth") != 0 && canMoveSouth) {
			canMoveSouth = false;
			return playerUnit.MoveSouth ();
		}else if(Input.GetAxisRaw ("MoveSouth") == 0){
			canMoveSouth = true;
		}

		if (Input.GetAxisRaw ("MoveEast") != 0 && canMoveEast) {
			canMoveEast = false;
			return playerUnit.MoveEast ();
		}else if(Input.GetAxisRaw ("MoveEast") == 0){
			canMoveEast = true;
		}

		if (Input.GetAxisRaw ("MoveWest") != 0 && canMoveWest) {
			canMoveWest = false;
			return 	playerUnit.MoveWest ();
		} else if(Input.GetAxisRaw ("MoveWest") == 0){
			canMoveWest = true;
		}

		if (Input.GetAxisRaw ("MoveNorthWest") != 0 && canMoveNorthWest){
			canMoveNorthWest = false;
			return playerUnit.MoveNorthWest ();
		}else if(Input.GetAxisRaw ("MoveNorthWest") == 0){
			canMoveNorthWest = true;
		}

		if (Input.GetAxisRaw ("MoveSouthWest") != 0 && canMoveSouthWest) {
			canMoveSouthWest = false;
			return playerUnit.MoveSouthWest ();
		}else if(Input.GetAxisRaw ("MoveSouthWest") == 0){
			canMoveSouthWest = true;
		}

		if (Input.GetAxisRaw ("MoveNorthEast") != 0 && canMoveNorthEast) {
			canMoveNorthEast = false;
			return playerUnit.MoveNorthEast ();
		}else if(Input.GetAxisRaw ("MoveNorthEast") == 0){
			canMoveNorthEast = true;
		}

		if (Input.GetAxisRaw ("MoveSouthEast") != 0 && canMoveSouthEast) {
			canMoveSouthEast = false;
			return 	playerUnit.MoveSouthEast ();
		} else if(Input.GetAxisRaw ("MoveSouthEast") == 0){
			canMoveSouthEast = true;
		}

		if (Input.GetAxisRaw ("MoveSouthEast") != 0 && canMoveSouthEast) {
			canMoveSouthEast = false;
			return 	playerUnit.MoveSouthEast ();
		} else if(Input.GetAxisRaw ("MoveSouthEast") == 0){
			canMoveSouthEast = true;
		}

		if (Input.GetAxisRaw ("TickRight") != 0 && canTickRight) {
			canTickRight = false;
			playerUnit.TickRight ();
			FillUI();
		} else if(Input.GetAxisRaw ("TickRight") == 0){
			canTickRight = true;
		}
		if (Input.GetAxisRaw ("TickLeft") != 0 && canTickLeft) {
			canTickLeft = false;
			playerUnit.TickLeft ();
			FillUI();
		} else if(Input.GetAxisRaw ("TickLeft") == 0){
			canTickLeft = true;
		}
		if(Input.GetAxisRaw("OpenInventory") != 0 && canOpenInventory){
			OpenInventory();
			SetSecondaryList(playerUnit.GetEquipped());
			canOpenInventory = false;
		}else if(Input.GetAxisRaw("OpenInventory") == 0){
			canOpenInventory = true;
		}	
		if(Input.GetAxisRaw("Search") != 0 && canSearch){
			inputMode = InputModes.search;
			canSearch = false;
			//EngageSearch();
		}else if(Input.GetAxisRaw("Search") == 0){
			canSearch = true;
		}

		return false;
	}
	void ScanForInput(){

	}
	public void SetPlayerUnit(Unit u){
		playerUnit = u;
		mainCameraScript.SetPlayerUnit(u);
	}
	public void FillUIReset(){
		leftNonePanel.transform.position = leftPanel.transform.position;
		rightNonePanel.transform.position = rightPanel.transform.position;
	}
	public void FillUI(){
		Item leftWeapon = playerUnit.GetEquippedWeaponLeft();
		if(leftWeapon.type != "none"){
			weaponNameTextLeft.text = leftWeapon.name;
			weaponDamageTextLeft.text = leftWeapon.damage.ToString();
			weaponTypeTextLeft.text = leftWeapon.type;
			leftNonePanel.transform.position = new Vector3(-1000,-1000,-1000);
		}else{
			leftNonePanel.transform.position = leftPanel.transform.position;
		}

		Item rightWeapon = playerUnit.GetEquippedWeaponRight();
		if(rightWeapon.type != "none"){
			weaponNameTextRight.text = rightWeapon.name;
			weaponDamageTextRight.text = rightWeapon.damage.ToString();
			weaponTypeTextRight.text = rightWeapon.type;
			rightNonePanel.transform.position = new Vector3(-1000,-1000,-1000);
		}else{
			rightNonePanel.transform.position = rightPanel.transform.position;
		}
	}

	public Unit GetPlayerUnit(){
		return playerUnit;
	}
	public void MoveNorth(){
		playerUnit.MoveNorth();
	}
	public void MoveSouth(){
		playerUnit.MoveSouth();
	}
	public void MoveEast(){
		playerUnit.MoveEast();
	}
	public void MoveWest(){
		playerUnit.MoveWest();
	}
	public void MoveNorthWest(){
		playerUnit.MoveNorthWest();
	}
	public void MoveNorthEast(){
		playerUnit.MoveNorthEast();
	}
	public void MoveSouthWest(){
		playerUnit.MoveSouthWest();
	}
	public void MoveSouthEast(){
		playerUnit.MoveSouthEast();
	}
	void SwitchInventoryFocus(){
		onRightPanel = !onRightPanel;
	}
}
