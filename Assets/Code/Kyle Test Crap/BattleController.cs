using System;
using System.Collections.Generic;
using Assets.Code.Abstract;
using Assets.Code.Statics;
using Assets.Code.Entities;
using UnityEngine;
using UnityEngine.UI;
using Vectrosity;

public class BattleController {

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

	private void newRound ()
	{
		Messenger.Broadcast("New Round");

	}

	private void turnCount(string team, int actions)
	{
		if (team == "Player") playerRoundActions += actions; 
		if (team == "Enemy") enemyRoundActions += actions;
	}

	private void setRoundTurnOrder()
	{
		string firstTurn;
		string secondTurn;
		var turnsSupported = new int();

		if (playerRoundActions >= enemyRoundActions)
		{
			firstTurn = "Player";
			secondTurn = "Enemy";
			turnsSupported = Mathf.FloorToInt(playerRoundActions);
		}
		else 
		{
			firstTurn = "Enemy";
			secondTurn = "Player";
			turnsSupported = Mathf.FloorToInt(playerRoundActions);
		}

		if (turnsSupported > 4) turnsSupported = 4;
		Round = new Turn[turnsSupported*2+1];
		for (int i=0; i<Round.Length; i++)
		{
			//if i is 0 or even .Team = firstTurn else .Team =SecondTurn;
			Round[i].Team = firstTurn;
			Round[i].Actions = 2;
		}

		//add in left over actions to the turns
		//set the UI elements to display the turn order

	}


}