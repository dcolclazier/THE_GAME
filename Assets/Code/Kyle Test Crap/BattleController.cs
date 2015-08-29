using System;
using System.Collections.Generic;
using Assets.Code.Abstract;
using Assets.Code.Statics;
using UnityEngine;
using UnityEngine.UI;
using Vectrosity;

public class BattleController : MonoBehaviour{
	
	public int TestPlayerActions;   //TESTING ONLY
	public int TestEnemyActions;    //TESTING ONLY
	
	private int playerRoundActions;
	private int playerActionsRemain;
	private int enemyRoundActions;
	private int enemyActionsRemain;
	
	private Turn[] Round;
	
	
	// Use this for initialization
	void Start () {
		Messenger.AddListener<string, int>("Actions For Team", turnCount);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void newRound ()
	{
		Messenger.Broadcast("New Round");
		GameObject newRoundText = GameObject.Find("New Round Text");
		Animator animator = newRoundText.GetComponent<Animator>();
		animator.Play("Round_Text");
	}
	
	private void turnCount(string team, int actions)
	{
		if (team == "Player") playerRoundActions += actions; 
		if (team == "Enemy") enemyRoundActions += actions;
	}
	
	public void setRoundTurnOrder()  //PUBLIC FOR TESTING
	{ 
		playerRoundActions = TestPlayerActions;  //TESTING ONLY 
		enemyRoundActions = TestEnemyActions;    //TESTING ONLY
		
		string firstTurn;
		string secondTurn;
		var turnsSupported = new int();
		playerActionsRemain = playerRoundActions;
		enemyActionsRemain = enemyRoundActions;
		GameObject[] oldCircles = GameObject.FindGameObjectsWithTag("TurnCircle");
		foreach (GameObject circ in oldCircles) GameObject.Destroy(circ);

		if (playerRoundActions >= enemyRoundActions)
		{
			firstTurn = "Player";
			secondTurn = "Enemy";
			turnsSupported = Mathf.FloorToInt(enemyRoundActions/2);
		}
		else 
		{
			firstTurn = "Enemy";
			secondTurn = "Player";
			turnsSupported = Mathf.FloorToInt(playerRoundActions/2);
		}
		
		if (turnsSupported > 4) turnsSupported = 4;
		int totalTurns = (turnsSupported*2) + 1;
		Round = new Turn[totalTurns];
		string who = firstTurn;
		for (int i=0; i<totalTurns; i++)
		{
			Round[i] = new Turn();
			Round[i].Team = who;
			if (who.Equals("Player"))
			{
				if (playerActionsRemain > 1) 
				{
					playerActionsRemain -= 2;
					Round[i].Actions = 2;
				}
				else 
				{
					playerActionsRemain--;
					Round[i].Actions = 1;
				}

			}
			else 
			{
				if (enemyActionsRemain > 1) 
				{
					enemyActionsRemain -= 2;
					Round[i].Actions = 2;
				}
				else 
				{
					enemyActionsRemain--;
					Round[i].Actions = 1;
				}

			}
			if (who.Equals(firstTurn)) who = secondTurn;
			else who = firstTurn;
		}
		
		//add in left over actions to the turns
		int[] firstOrder = new int[5];
		int[] secondOrder = new int[4];
		bool firstTurnEven = true; //Does whoever goes first have an even or odd number of turns?
		if (turnsSupported%2 == 0) firstTurnEven = false;
		Debug.Log("First turn is even??  " + firstTurnEven);
		var startPoint = Mathf.FloorToInt(totalTurns/2);
		Debug.Log("START POINT:  " + startPoint);
		if (firstTurnEven)
		{
			firstOrder[0] = startPoint - 1;
			firstOrder[1] = startPoint + 1;
			firstOrder[2] = startPoint - 3;
			firstOrder[3] = startPoint + 3;

			secondOrder[0] = startPoint;
			secondOrder[1] = startPoint - 2;
			secondOrder[2] = startPoint + 2;
			//secondOrder[3] = startPoint - 4;
			//secondOrder[4] = startPoint + 4;
			
		}
		else
		{
			firstOrder[0] = startPoint;
			firstOrder[1] = startPoint - 2;
			firstOrder[2] = startPoint + 2;
			firstOrder[3] = startPoint - 4;
			firstOrder[4] = startPoint + 4;

			secondOrder[0] = startPoint -1;
			secondOrder[1] = startPoint + 1;
			secondOrder[2] = startPoint - 3;
			secondOrder[3] = startPoint + 3;
		}
		
		if (firstTurn.Equals("Player"))
		{
			int i = 0;
			while (playerActionsRemain > 0)
			{
				if (i > turnsSupported) i=0;
				Round[firstOrder[i]].Actions++;  //array index out of range
				playerActionsRemain--;
				i++;
			}
			i = 0;
			while (enemyActionsRemain > 0)
			{
				if (i == turnsSupported) i=0;
				Round[secondOrder[i]].Actions++;
				enemyActionsRemain--;
				i++;
			}
		}
		else
		{
			int i = 0;
			while (enemyActionsRemain > 0)
			{
				if (i > turnsSupported) i=0;
				Round[firstOrder[i]].Actions++;
				enemyActionsRemain--;
				i++;
			}
			i = 0;
			while (playerActionsRemain > 0)
			{
				if (i == turnsSupported) i=0;
				Round[secondOrder[i]].Actions++;
				playerActionsRemain--;
				i++;
			}
		}
		//set the UI elements to display the turn order
		Transform circleGroup = GameObject.Find("Turn_Circles").transform;
		int actionNum = 0;
		int offset = ((playerRoundActions + enemyRoundActions)/2) * 20 -3;
		for(int i=0; i<Round.Length; i++)
		{
			for (int j=0; j<Round[i].Actions; j++)    //null ref exception
			{
				GameObject circle;
				Vector2 newlocal;
				if (Round[i].Team.Equals("Player")) circle = Canvas.Instantiate(Resources.Load("Prefabs/Player Action")) as GameObject;
				else circle = Canvas.Instantiate(Resources.Load("Prefabs/Enemy Action")) as GameObject;
				
				circle.transform.SetParent(circleGroup, false);
				newlocal = circle.transform.position; 
				newlocal.x += actionNum*20 + 3 - offset;
				circle.transform.position = newlocal;
				actionNum++;
			}
		}
	}
	
	
	
}


