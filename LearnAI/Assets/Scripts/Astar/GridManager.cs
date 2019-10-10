using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour {
    //该类用于处理网格的全部属性

    /*该类的单体实例*/
    private static GridManager s_Instance = null;
    /*单例属性，供外界调用*/
    public static GridManager instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = FindObjectOfType(typeof(GridManager)) as GridManager;
                if (s_Instance == null)
                    Debug.Log("Could not locate a GridManager " + "object. \n You have to have exactly " + "one GridManager in the scene.");
            }
            return s_Instance;
        }
    }

    /*网格行数目*/
    public int numOfRows;
    /*网格列数目*/
    public int numOfColumns;
    /*各个网格单元尺寸*/
    public float gridCellSize;
    /*对网格的可视化*/
    public bool showGrid = true;
    /*对障碍物的可视化*/
    public bool showObstacleBlocks = true;

    /**/
    private Vector3 origin = new Vector3();
    /**/
    private GameObject[] obstacleList;
    /**/
    public Node[,] nodes { get; set; }
    /*只读属性 ???*/
    public Vector3 Origin
    {
        get { return origin; }
    }

    void Awake()
    {
        obstacleList = GameObject.FindGameObjectsWithTag("Obstacle");
        CalculateObstacles();
    }
    /// <summary>
    /// find all the obstacles on the map
    /// </summary>
    void CalculateObstacles()
    {
        nodes = new Node[numOfColumns, numOfRows];
        int index = 0;
        for(int i = 0; i < numOfColumns; i++)
        {
            for(int j = 0; j < numOfRows; j++)
            {
                Vector3 cellPos = GetGridCellCenter(index);//从网格单元索引处返回世界坐标系中该单元位置
                Node node = new Node(cellPos);
                nodes[i, j] = node;
                index++;
            }
        }
        if (obstacleList != null && obstacleList.Length > 0)
        {
            //for each obstacle found on the map ,record it in our list
            foreach(GameObject data in obstacleList)
            {
                int indexCell = GetGridIndex(data.transform.position);//根据既定位置返回索引
                int col = GetColumn(indexCell);//根据索引返回行
                int row = GetRow(indexCell);
                nodes[row, col].MarkAsObstacle();//将节点更新为障碍物
            }
        }
    }
    /// <summary>
    /// //从网格单元索引处返回世界坐标系中该单元位置
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public Vector3 GetGridCellCenter(int index)
    {
        Vector3 cellPosition = GetGridCellPosition(index);
        cellPosition.x += (gridCellSize / 2.0f);
        cellPosition.y += (gridCellSize / 2.0f);
        return cellPosition;
    }

    public Vector3 GetGridCellPosition(int index)
    {
        int row = GetRow(index);
        int col = GetColumn(index);
        float xPosInGrid = col * gridCellSize;
        float zPosInGrid = row * gridCellSize;
        return Origin + new Vector3(xPosInGrid, 0.0f, zPosInGrid);
    }
    /// <summary>
    /// 根据既定位置返回网格中的单元索引
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public int GetGridIndex(Vector3 pos)
    {
        if (!IsInBounds(pos))
        {
            return -1;
        }
        pos -= Origin;
        int col = (int)(pos.x / gridCellSize);
        int row = (int)(pos.z / gridCellSize);
        return (row * numOfColumns + col);
    }
    /// <summary>
    /// 判断点是否在格子中
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public bool IsInBounds(Vector3 pos)
    {
        float width = numOfColumns * gridCellSize;
        float height = numOfRows * gridCellSize;
        return (pos.x >= Origin.x && pos.x <= Origin.x + width && pos.z <= Origin.z + height && pos.z >= Origin.z);
    }
    
    public int GetRow(int index)
    {
        int row = index / numOfColumns;
        return row;
    }

    public int GetColumn(int index)
    {
        int col = index % numOfColumns;
        return col;
    }
    /// <summary>
    /// 获取某点的邻接节点
    /// </summary>
    /// <param name="node"></param>
    /// <param name="neighbors"></param>
    public void GetNeighbours(Node node,ArrayList neighbors)
    {
        Vector3 neighborPos = node.position;
        int neighborIndex = GetGridIndex(neighborPos);

        int row = GetRow(neighborIndex);
        int column = GetColumn(neighborIndex);

        //Bottom
        int leftNodeRow = row - 1;
        int leftNodeColumn = column;
        AssignNeighbour(leftNodeRow, leftNodeColumn, neighbors);

        //Top
        leftNodeRow = row + 1;
        leftNodeColumn = column;
        AssignNeighbour(leftNodeRow, leftNodeColumn, neighbors);

        //Right
        leftNodeRow = row;
        leftNodeColumn = column + 1;
        AssignNeighbour(leftNodeRow, leftNodeColumn, neighbors);

        //Left
        leftNodeRow = row;
        leftNodeColumn = column - 1;
        AssignNeighbour(leftNodeRow, leftNodeColumn, neighbors);
    }
    /// <summary>
    /// 对节点进行检测并查看是否为障碍物对象，如果不是，可将邻接节点置于引用数组列表里
    /// </summary>
    /// <param name="row"></param>
    /// <param name="column"></param>
    /// <param name="neighbors"></param>
    void AssignNeighbour(int row,int column,ArrayList neighbors)
    {
        if (row != -1 && column != -1 && row < numOfRows && column < numOfColumns)
        {
            Node nodeToAdd = nodes[row, column];
            if (!nodeToAdd.bObstacle)
            {
                neighbors.Add(nodeToAdd);
            }
        }
    }

    void OnDrawGizmos()
    {
        if (showGrid)
        {
            DebugDrawGrid(transform.position, numOfRows, numOfColumns, gridCellSize, Color.blue);
        }

        Gizmos.DrawSphere(transform.position, 0.5f);
        if (showObstacleBlocks)
        {
            Vector3 cellSize = new Vector3(gridCellSize, 1.0f, gridCellSize);
            if (obstacleList != null && obstacleList.Length > 0)
            {
                foreach(GameObject data in obstacleList)
                {
                    Gizmos.DrawCube(GetGridCellCenter(GetGridIndex(data.transform.position)), cellSize);
                }
            }
        }
    }

    public void DebugDrawGrid(Vector3 origin,int numRows,int numCols,float cellSize,Color color)
    {
        float width = (numCols * cellSize);
        float height = (numRows * cellSize);


        //Draw the horizontal grid lines
        for(int i = 0; i < numRows + 1; i++)
        {
            Vector3 startPos = origin + i * cellSize * new Vector3(0.0f, 0.0f, 1.0f);
            Vector3 endPos = startPos + width * new Vector3(1.0f, 0.0f, 0.0f);
            Debug.DrawLine(startPos, endPos, color);
        }

        //Draw the vertical grid lines
        for (int i = 0; i < numCols + 1; i++)
        {
            Vector3 startPos = origin + i * cellSize * new Vector3(1.0f, 0.0f, 0.0f);
            Vector3 endPos = startPos + height * new Vector3(0.0f, 0.0f, 1.0f);
            Debug.DrawLine(startPos, endPos, color);
        }
    }
}
