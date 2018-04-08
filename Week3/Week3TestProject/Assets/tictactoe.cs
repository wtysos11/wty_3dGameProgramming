using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tictactoe : MonoBehaviour {
	int state;
	int [,] map = new int[3,3];
	bool toggle;
	int remaining;//remain blank
	//for reset button

	void Start(){
		reset ();
	}

	void reset(){
		for (int i = 0; i < 3; i++) {
			for (int j = 0; j < 3; j++) {
				map [i, j] = 0;
			}
		}
		remaining = 9;
		toggle = true;
		state = 3;
	}

	bool checking(){
		for (int i = 0; i < 3; i++) {
			if (map[i,0]!=0 && map [i, 0] == map [i, 1] && map[i,1]==map [i, 2]) {
				if (map [i, 0] == 1) {
					state = 1;
				} else if (map [i, 0] == 2) {
					state = 2;
				}
				return true;
			}

			if (map [0, i] != 0 && map [0, i] == map [1, i] &&map[1,i]==map [2, i]) {
				if (map [0, i] == 1) {
					state = 1;
				} else if (map [0, i] == 2) {
					state = 2;
				}
				return true;
			}
		}
		if (map [1, 1] != 0 &&( (map [0, 0] == map [1, 1] &&map [1, 1]== map [2, 2]) || (map [2, 0] == map [1, 1] &&map [1, 1]== map [0, 2]))) {
			if (map [1, 1] == 1) {
				state = 1;
			} else if (map [1, 1] == 2) {
				state = 2;
			}
			return true;
		}
		if (remaining == 0) {
			state = 0;
			return true;
		}
		return false;
	}

	void OnGUI () {
		if (GUI.Button (new Rect (700, 150, 100, 30), "reset")) {
			reset ();
		}

		if (checking ()) {
			//Debug.Log ("Someone wins!");
			var textStyle = new GUIStyle();
			textStyle.fontSize = 20;
			textStyle.alignment = TextAnchor.MiddleCenter;
			textStyle.fontStyle = FontStyle.Bold;
			if (state == 0) {//both win
				GUI.Label(new Rect(400,0,290,90),"No one wins",textStyle);
			} else if (state == 1) {//first win
				GUI.Label(new Rect(400,0,290,90),"O wins",textStyle);
			} else if (state == 2) {//second win
				GUI.Label(new Rect(400,0,290,90),"X wins",textStyle);
			}
		} 
		for(int i=0;i<3;i++){
			for (int j = 0; j < 3; j++) {
				if (map [i, j] == 1) {
					GUI.Button (new Rect (400 + 100 * i, 100 + 100 * j, 90, 90), "O");
				} else if (map [i, j] == 2) {
					GUI.Button (new Rect (400 + 100 * i, 100 + 100 * j, 90, 90), "X");
				} else if(GUI.Button (new Rect (400+100*i,100+100*j,90,90), "")){//no signal yet
					if (state == 3) {
						if (toggle) {
							map [i, j] = 1;
						} else {
							map [i, j] = 2;
						}
						remaining--;
						toggle = !toggle;					
					}

				}
			}
		}

	}

}
