using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic2048
{
    public List<Cell> cells;
    public int maxNumberOnDesktop;

    public struct Cell
    {
        public int x, y,value;

        public Cell(int x,int y,int value)
        {
            this.x = x;
            this.y = y;
            this.value = value;
        }
    };
    
    public GameLogic2048()
    {
        cells = new List<Cell>();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                cells.Add(new Cell(j,i,-1));
            }
        }

        maxNumberOnDesktop = -1;
    }


    public Action<List<Cell>> dispalyAction;

    private bool hasChange = false;
    public bool Move(KeyCode moveID,ref bool usefulMove,ref int currentMaxValue)
    {
        hasChange = false;
        if (moveID == KeyCode.UpArrow)
        {
            MoveUp2();
        }else if (moveID == KeyCode.RightArrow)
        {
            MoveRight2();
        }else if (moveID == KeyCode.DownArrow)
        {
            MoveDown2();
        }
        else
        {
            MoveLeft2();
        }


        bool spwan = false;
        if (hasChange == true)
        {
            usefulMove = true;
            int newPointX = 0, newPointY = 0, newValue = -1; 
            spwan = SpwanARandomNumber(ref newPointX,ref newPointY,ref newValue);
        }
        else
        {
            usefulMove = false;
        }
        
        if (dispalyAction != null)
        {
            dispalyAction(cells);
        }

        currentMaxValue = this.maxNumberOnDesktop;
        return isGameOver();
    }

    private bool isGameOver()
    {
        bool isGameOver = true;
        
        for (int i = 0; i < 4; i++)
        {
            bool outLoop = false;
            for (int j = 0; j < 4; j++)
            {
                Cell current = cells[i*4+j];
                if (current.value == -1)
                {
                    isGameOver = false;
                    return isGameOver;
                }
                if (j < 3)
                {
                    Cell front = cells[i*4+j+1];
                    if (front.value == current.value)
                    {
                        isGameOver = false;
                        return isGameOver;
                    }
                }

                if (j > 1)
                {
                    Cell back = cells[i*4+j-1];
                    if (back.value == current.value)
                    {
                        isGameOver = false;
                        return isGameOver;
                    }
                }

                if (i < 3)
                {
                    Cell up = cells[(i+1)*4+j];
                    if (up.value == current.value)
                    {
                        isGameOver = false;
                        return isGameOver;
                    }
                }

                if (i > 1)
                {
                    Cell down = cells[(i-1)*4+j];
                    if (down.value == current.value)
                    {
                        isGameOver = false;
                        return isGameOver;
                    }
                }
            }
        }
        return isGameOver;
    }
    
    private List<List<Cell>> caculateStacks;
    
    private void MoveLeft2()
    {
        caculateStacks = new List<List<Cell>>();
        for (int i = 0; i < 4; i++)
        {
            List<Cell> caculateStack = new List<Cell>();
            bool containSpace = false;
            for (int j = 0; j <4; j++)
            {
                Cell currentCell =cells[i*4+j];
                if (currentCell.value != -1)
                {
                    caculateStack.Add(currentCell);
                    if (containSpace == true)
                    {
                        hasChange = true;
                    }
                }
                else
                {
                    if (j <= 3 )
                    {
                        containSpace = true;
                    }
                }
            }
            caculateStacks.Add(caculateStack);
        }

        for (int i = 0; i < caculateStacks.Count ; i++)
        {
            var caculateStack = caculateStacks[i];
            if (caculateStack.Count > 0)
            {
                Caculate(caculateStack, 1);
                int baseY = caculateStack[0].y;
                for (int j = 0; j < 4; j++)
                {
                    if (caculateStack.Count > 0)
                    {
                        var curCell = caculateStack[0];
                        curCell.x = j;
                        curCell.y = baseY;
                        cells[baseY*4+j] = curCell;
                        caculateStack.RemoveAt(0);
                    }
                    else
                    {
                        cells[baseY*4+j] = new Cell(j,baseY,-1);
                    }
                }
            }
        }
        
    }

    private void Caculate(List<Cell> stack,int position)
    {
        string s = "";
        foreach (var cell in stack)
        {
            s += cell.value.ToString() + "   ";
        }
        Debug.Log(s);
        s = "";
        if (position < stack.Count)
        {
            if (stack[position].value == stack[position - 1].value)
            {
                hasChange = true;
                var cell_1 = stack[position - 1];
                cell_1.value = cell_1.value * 2;
                if (maxNumberOnDesktop < cell_1.value)
                {
                    maxNumberOnDesktop = cell_1.value;
                }
                stack[position - 1] = cell_1; 
                
                stack.RemoveAt(position);
                s = "";
                foreach (var cell in stack)
                {
                    s += cell.value.ToString() + "   ";
                }
                Debug.Log(s);
                //2048一次只减一个，所以到此为止
            }
            else
            {
                position += 1;
                Caculate(stack,position);
            }
        }
    }
    
    
    private void MoveRight2()
    {
        caculateStacks = new List<List<Cell>>();
        for (int i = 0; i < 4; i++)
        {
            List<Cell> caculateStack = new List<Cell>();
            bool containSpace = false;
            for (int j = 3; j >= 0; j--)
            {
                Cell currentCell = cells[i*4+j];
                if (currentCell.value != -1)
                {
                    caculateStack.Add(currentCell);
                    if (containSpace == true)
                    {
                        hasChange = true;
                    }
                }
                else
                {
                    if (j >= 1 )
                    {
                        containSpace = true;
                    }
                }
            }
            caculateStacks.Add(caculateStack);
        }

        for (int i = 0; i < caculateStacks.Count ; i++)
        {
            var caculateStack = caculateStacks[i];
            if (caculateStack.Count > 0)
            {
                Caculate(caculateStack, 1);
                int baseY = caculateStack[0].y;
                for (int j = 3; j >=0; j--)
                {
                    if (caculateStack.Count > 0)
                    {
                        var curCell = caculateStack[0];
                        curCell.x = j;
                        curCell.y = baseY;
                        cells[baseY*4+j] = curCell;
                        caculateStack.RemoveAt(0);
                    }
                    else
                    {
                        cells[baseY*4+j] = new Cell(j,baseY,-1) ;
                    }
                }
            }
        }

    }
    private void MoveDown2()
    {
        caculateStacks = new List<List<Cell>>();
        for (int i = 0; i < 4; i++)
        {
            List<Cell> caculateStack = new List<Cell>();
            bool containSpace = false;
            for (int j = 0; j < 4; j++)
            {
                Cell currentCell = cells[j*4+i];
                if (currentCell.value != -1)
                {
                    caculateStack.Add(currentCell);
                    if (containSpace == true)
                    {
                        hasChange = true;
                    }
                }
                else
                {
                    if (j <= 3)
                    {
                        containSpace = true;
                    }
                }
            }
            caculateStacks.Add(caculateStack);
        }

        for (int i = 0; i < caculateStacks.Count ; i++)
        {
            var caculateStack = caculateStacks[i];
            if (caculateStack.Count > 0)
            {
                Caculate(caculateStack, 1);
                int baseX = caculateStack[0].x;
                for (int j = 0; j < 4; j++)
                {
                    if (caculateStack.Count > 0)
                    {
                        var curCell = caculateStack[0];
                        curCell.x = baseX;
                        curCell.y = j;
                        cells[j*4+baseX] = curCell;
                        caculateStack.RemoveAt(0);
                    }
                    else
                    {
                        cells[j*4+baseX] = new Cell(baseX,j,-1);
                    }
                }
            }
        }
    }
    private void MoveUp2()
    {
        caculateStacks = new List<List<Cell>>();
        for (int i = 0; i < 4; i++)
        {
            List<Cell> caculateStack = new List<Cell>();
            bool containSpace = false;
            for (int j = 3; j >=0; j--)
            {
                Cell currentCell = cells[j*4+i];
                if (currentCell.value != -1)
                {
                    caculateStack.Add(currentCell);
                    if (containSpace == true)
                    {
                        hasChange = true;
                    }
                }
                else
                {
                    if (j >= 1 )
                    {
                        containSpace = true;
                    }
                }
            }
            caculateStacks.Add(caculateStack);
        }

        for (int i = 0; i < caculateStacks.Count ; i++)
        {
            var caculateStack = caculateStacks[i];
            if (caculateStack.Count > 0)
            {
                Caculate(caculateStack,1);
                int baseX = caculateStack[0].x;
                for (int j = 3; j >= 0; j--)
                {
                    if (caculateStack.Count > 0)
                    {
                        var curCell = caculateStack[0];
                        curCell.x = baseX;
                        curCell.y = j;
                        cells[j*4+baseX] = curCell;
                        caculateStack.RemoveAt(0);
                    }
                    else
                    {
                        cells[j*4+baseX] = new Cell(baseX,j,-1);    
                    }
                }
            }
            
        }

    }
    
    /// <summary>
    /// 随机生成一个新的数字
    /// </summary>
    /// <param name="positionX">数字的x位置</param>
    /// <param name="positionY">数字的y位置</param>
    /// <param name="value">数字的值</param>
    /// <returns></returns>
    public bool SpwanARandomNumber(ref int positionX, ref int positionY, ref int value)
    {

        List<int> canSpwanNumbers = new List<int>(){2,4,8,16,32,64,128,256,512,1024};
        bool hasSpwan = false;
        if (maxNumberOnDesktop == -1)
        {
            maxNumberOnDesktop = 2;
            value = 2;
            positionX = UnityEngine.Random.Range(0, 4);
            positionY = UnityEngine.Random.Range(0, 4);
            var cell =  cells[positionY*4+positionX];
            cell.value = value;
            cells[positionY * 4 + positionX] = cell;
            hasSpwan = true;
        }
        else
        {
            for (int i = 0; i < canSpwanNumbers.Count; i++)
            {
                if (canSpwanNumbers[i] > maxNumberOnDesktop)
                {
                    canSpwanNumbers.RemoveAt(i);
                    i--;
                }
            }
            
            
            List<Cell> canSpwanPoints = new List<Cell>();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    var curCell = cells[i * 4 + j]; 
                    if (cells[i*4+j].value == -1)
                    {
                        canSpwanPoints.Add(curCell);
                    }
                }
            }
            if (canSpwanPoints.Count > 0)
            {
                
                List<int> rates = new List<int>();
                for (int i = 0; i < canSpwanNumbers.Count; i++)
                {
                    int id = canSpwanNumbers[i];
                    for (int j = 0; j < id; j++)
                    {
                        rates.Add(id);
                    }
                }

                int rid = UnityEngine.Random.Range(0, rates.Count);

                //生成最大数的概率最高
                bool makeMaxNumber = false;
                if (makeMaxNumber)
                {
                    value = rates[rid];
                }
                else//2048 默认生成2
                {
                    value = 2;
                }

                int rPositionID = UnityEngine.Random.Range(0, canSpwanPoints.Count);
                positionX = canSpwanPoints[rPositionID].x;
                positionY = canSpwanPoints[rPositionID].y;
                var curCell = cells[positionY*4+positionX]; 
                curCell.value = value;
                cells[positionY * 4 + positionX] = curCell;
                hasSpwan = true;
            }
            else
            {
                hasSpwan = false;
            }
        }

        if (dispalyAction != null)
        {
            dispalyAction(cells);
        }

        return hasSpwan;
    }

}
