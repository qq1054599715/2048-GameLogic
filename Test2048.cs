using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test2048 : MonoBehaviour
{
	private GameLogic2048 gameLogic2048;

	public List<GameLogic2048.Cell> Cells;
	// Use this for initialization
	void Start () {
		gameLogic2048 = new GameLogic2048();
		gameLogic2048.dispalyAction = (List<GameLogic2048.Cell> cells) => {
			Cells = new List<GameLogic2048.Cell>();
			Cells.AddRange(cells.ToArray());
			};
		int newPointX=0, newPointY=0, newValue=0;
		
		gameLogic2048.SpwanARandomNumber(ref newPointX,ref newPointY,ref newValue);
		gameLogic2048.SpwanARandomNumber(ref newPointX,ref newPointY,ref newValue);
	}


	private void OnGUI()
	{
		if (Cells != null)
		{
			for (int i = 0; i < Cells.Count; i++)
			{
				Vector2 position = new Vector2(Cells[i].x,Cells[i].y)*100;
				int value = Cells[i].value;
				string context = value.ToString();
				if (value == -1)
				{
					context = " ";
				}
				GUI.TextArea(new Rect(position.x, position.y, 100, 100),context);
			}
		}
		GUI.Label(new Rect(0,500,500,100),  "当前最大值："+currentMaxValue.ToString());
		if (isGameOver == true)
		{
			GUI.Label(new Rect(0,600,500,100),  "游戏结束");
		}
	}
	
	int currentMaxValue = 0;
	bool isGameOver = false;
	// Update is called once per frame
	void Update ()
	{

		bool effectiveInput = false;
		
		
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			isGameOver =  gameLogic2048.Move(KeyCode.DownArrow,ref effectiveInput,ref currentMaxValue);
		}
		else
		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			isGameOver = gameLogic2048.Move(KeyCode.RightArrow,ref effectiveInput,ref currentMaxValue);
		}
		else
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			isGameOver = gameLogic2048.Move(KeyCode.UpArrow,ref effectiveInput,ref currentMaxValue);
		}
		else 
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			isGameOver = gameLogic2048.Move(KeyCode.LeftArrow,ref effectiveInput,ref currentMaxValue);
		}
		
		if (isGameOver == true)
		{
			Debug.Log("游戏结束");
		}
		
	}
}
