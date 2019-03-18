using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject enemy;
    public GameObject player;
    public GameObject ufo;
    public GameObject line = null;
    public GameObject lineConnection = null;
    public int HideWallCount = 120;
    public static int row = 15;
    private List<GameObject> listMaze;
    private static List<NodeConnection> listConnected = new List<NodeConnection>();
    public GameObject canvasMenu;
    public Button nextLevelButton;

    public List<Vector2> listPath = new List<Vector2>();

    public class Location
    {
        public float X;
        public float Y;
        public float F;
        public float G;
        public float H;
        public Location Parent;
        public bool isEqual(Vector2 vector)
        {
            if (X == vector.x && Y == vector.y)
                return true;
            return false;
        }
    }
    public class NodeConnection
    {
        public Vector2 start;
        public Vector2 end;
        public NodeConnection(Vector2 s, Vector2 e)
        {
            start = s;
            end = e;
        }
        public bool isContain(Vector2 a, Vector2 b)
        {
            if (a == start && b == end || a == end && b == start)
                return true;
            return false;
        }
       public bool isEqual(NodeConnection b)
        {

            if ((start == b.start && end == b.end)|| (start == b.end && end == b.start))
                return true;
            return false;
        }
    }
    public void MenuButtonPress()
    {
        canvasMenu.SetActive(!canvasMenu.active);
    }
    public void ShowMenu()
    {
        canvasMenu.SetActive(true);
    }
    public void CloseMenu()
    {
        canvasMenu.SetActive(false);
    }
    public void GenerateMaze()
    {
        float newEnemyX = UnityEngine.Random.Range(5, 12) + 0.5f;
        float newEnemyY = UnityEngine.Random.Range(5, 12) + 0.5f;
        enemy.GetComponent<EnemyController>().ChangeLocationRange(newEnemyX, newEnemyY);
        RestartLevel();
        foreach (GameObject maze in listMaze)
        {
            Destroy(maze);
        }

        if (listMaze != null)
            listMaze.Clear();
        if (listConnected != null)
            listConnected.Clear();

        List<Vector2> listPoint = new List<Vector2>();


        listConnected = new List<NodeConnection>();

        if (line != null)
        {
            GenerateBoudary();

            for (int i = 1; i < row ; i++)
            {
                for (int j = 1; j < row ; j++)
                {
                    listPoint.Add(new Vector2(j, i));
                }
            }
            foreach (Vector2 point in listPoint)
            {
                float x = point.x;
                float y = point.y;
                float delta = 1 / 2f;
                int randomDirection = UnityEngine.Random.Range(0, 4);

                NodeConnection nodeConnectionLeft = new NodeConnection(new Vector2(x - delta, y + delta), new Vector2(x - delta, y - delta));
                NodeConnection nodeConnectionDown = new NodeConnection(new Vector2(x - delta, y - delta), new Vector2(x + delta, y - delta));
                NodeConnection nodeConnectionRight = new NodeConnection(new Vector2(x + delta, y - delta), new Vector2(x + delta, y + delta));
                NodeConnection nodeConnectionUp = new NodeConnection(new Vector2(x - delta, y + delta), new Vector2(x + delta, y + delta));

                if(!SearchConnectedList(nodeConnectionLeft))
                    listConnected.Add(nodeConnectionLeft);

                if (!SearchConnectedList(nodeConnectionDown))
                    listConnected.Add(nodeConnectionDown);

                if (!SearchConnectedList(nodeConnectionRight))
                    listConnected.Add(nodeConnectionRight);

                if (!SearchConnectedList(nodeConnectionUp))
                    listConnected.Add(nodeConnectionUp);
            }
            foreach (Vector2 point in listPoint)
            {
                float x = point.x;
                float y = point.y;
                float delta = 1 / 2f;
                int randomDirection = UnityEngine.Random.Range(0, 4);           // 0 North,1 West ,2 South, 3 East

                //Let make maze harder
                //int random2 = UnityEngine.Random.Range(0,10);
                //if (random2 == 0)
                //{
                //    DrawLine(point, point + new Vector2(-1, 0));
                //    NodeConnection nodeConnectionLeft = new NodeConnection(new Vector2(x - delta, y + delta), new Vector2(x - delta, y - delta));
                //    if (SearchConnectedList(nodeConnectionLeft))
                //        DeleteConnected(nodeConnectionLeft);
                //}
               


                switch (randomDirection)
                {
                    case 0:
                        DrawLine(point, point + new Vector2(0, 1));
                        NodeConnection nodeConnectionUp = new NodeConnection(new Vector2(x - delta, y + delta), new Vector2(x + delta, y + delta));
                        if (SearchConnectedList(nodeConnectionUp))
                            DeleteConnected(nodeConnectionUp);
                        break;
                    case 1:
                        DrawLine(point, point + new Vector2(-1, 0));

                        NodeConnection nodeConnectionLeft = new NodeConnection(new Vector2(x - delta, y + delta), new Vector2(x - delta, y - delta));
                        if (SearchConnectedList(nodeConnectionLeft))
                            DeleteConnected(nodeConnectionLeft);
                        break;
                    case 2:
                        DrawLine(point, point + new Vector2(0, -1));

                        NodeConnection nodeConnectionDown = new NodeConnection(new Vector2(x - delta, y - delta), new Vector2(x + delta, y - delta));
                        if (SearchConnectedList(nodeConnectionDown))
                            DeleteConnected(nodeConnectionDown);
                        break;
                    case 3:
                        DrawLine(point, point + new Vector2(1, 0));

                        NodeConnection nodeConnectionRight = new NodeConnection(new Vector2(x + delta, y - delta), new Vector2(x + delta, y + delta));
                        if (SearchConnectedList(nodeConnectionRight))
                            DeleteConnected(nodeConnectionRight);
                        break;
                }
            }

            player.GetComponent<PlayerController>().listConnected = listConnected;
        }
    }
    public static List<Vector2> FindWay(Vector3 startPosition, Vector3 endPosition)
    {
        List<Vector2> path = new List<Vector2>();
        Location current = null;
        var start = new Location { X = startPosition.x, Y = startPosition.y };
        var target = new Location { X = endPosition.x, Y = endPosition.y };
        var openList = new List<Location>();
        var closedList = new List<Location>();
        int g = 0;

        openList.Add(start);
        while (openList.Count > 0)
        {
            // get the square with the lowest F score
            var lowest = openList.Min(l => l.F);
            current = openList.First(l => l.F == lowest);
            // add the current square to the closed list
            closedList.Add(current);


            // remove it from the open list
            openList.Remove(current);

            // if we added the destination to the closed list, we've found a path
            if (closedList.FirstOrDefault(l => l.X == target.X && l.Y == target.Y) != null)
                break;

            var adjacentSquares = GetWalkableAdjacentSquares(current.X, current.Y,target);
            g++;

            foreach (var adjacentSquare in adjacentSquares)
            {
                // if this adjacent square is already in the closed list, ignore it
                if (closedList.FirstOrDefault(l => l.X == adjacentSquare.X
                        && l.Y == adjacentSquare.Y) != null)
                    continue;

                // if it's not in the open list...
                if (openList.FirstOrDefault(l => l.X == adjacentSquare.X
                        && l.Y == adjacentSquare.Y) == null)
                {
                    // compute its score, set the parent
                    adjacentSquare.G = g;
                    adjacentSquare.H = ComputeHScore(adjacentSquare.X, adjacentSquare.Y, target.X, target.Y);
                    adjacentSquare.F = adjacentSquare.G + adjacentSquare.H;
                    adjacentSquare.Parent = current;

                    // and add it to the open list
                    openList.Insert(0, adjacentSquare);
                }
                else
                {
                    // test if using the current G score makes the adjacent square's F score
                    // lower, if yes update the parent because it means it's a better path
                    if (g + adjacentSquare.H < adjacentSquare.F)
                    {
                        adjacentSquare.G = g;
                        adjacentSquare.F = adjacentSquare.G + adjacentSquare.H;
                        adjacentSquare.Parent = current;
                    }
                }
            }
        }
        path.Clear();
        while (current != null)
        {
            path.Add(new Vector2(current.X, current.Y));
            current = current.Parent;
        }
        path.Reverse();
        return path;
    }
    public void RestartLevel()
    {
        isWin = false;
        player.transform.position = new Vector3(0.5f, 0.5f, 0);
        enemy.transform.position = enemy.GetComponent<EnemyController>().locationRange;
        canvasMenu.SetActive(false);
    }
    public void CallUFO()
    {
        var newUFO = Instantiate(ufo, new Vector3(player.transform.position.x, player.transform.position.y, 0), Quaternion.identity);
        newUFO.GetComponent<UFOController>().Go(FindWay(PlayerController.roundingVector(player.transform.position), new Vector3(14.5f, 14.5f, 0)));
    }
    public void StartGoingCalculate()
    {
        StartCoroutine(StartCalculate());
    }
    
    public IEnumerator StartCalculate()
    {
        Location current = null;
        var start = new Location { X = 0.5f, Y = 0.5f };
        var target = new Location  { X = 14.5f, Y = 14.5f };
        var openList = new List<Location>();
        var closedList = new List<Location>();
        int g = 0;

        openList.Add(start);
        while (openList.Count > 0)
        {
            // get the square with the lowest F score
            var lowest = openList.Min(l => l.F);
            current = openList.First(l => l.F == lowest);

            //GameObject newCircle = Instantiate(circle);
            //newCircle.transform.position = new Vector3(current.X, current.Y, 0);
            yield return new WaitForSeconds(0.5f);
            // add the current square to the closed list
            closedList.Add(current);

          
            // remove it from the open list
            openList.Remove(current);

            // if we added the destination to the closed list, we've found a path
            if (closedList.FirstOrDefault(l => l.X == target.X && l.Y == target.Y) != null)
                break;

            var adjacentSquares = GetWalkableAdjacentSquares(current.X, current.Y,target);
            g++;

            foreach (var adjacentSquare in adjacentSquares)
            {
                // if this adjacent square is already in the closed list, ignore it
                if (closedList.FirstOrDefault(l => l.X == adjacentSquare.X
                        && l.Y == adjacentSquare.Y) != null)
                    continue;

                // if it's not in the open list...
                if (openList.FirstOrDefault(l => l.X == adjacentSquare.X
                        && l.Y == adjacentSquare.Y) == null)
                {
                    // compute its score, set the parent
                    adjacentSquare.G = g;
                    adjacentSquare.H = ComputeHScore(adjacentSquare.X, adjacentSquare.Y, target.X, target.Y);
                    adjacentSquare.F = adjacentSquare.G + adjacentSquare.H;
                    adjacentSquare.Parent = current;

                    // and add it to the open list
                    openList.Insert(0, adjacentSquare);
                }
                else
                {
                    // test if using the current G score makes the adjacent square's F score
                    // lower, if yes update the parent because it means it's a better path
                    if (g + adjacentSquare.H < adjacentSquare.F)
                    {
                        adjacentSquare.G = g;
                        adjacentSquare.F = adjacentSquare.G + adjacentSquare.H;
                        adjacentSquare.Parent = current;
                    }
                }
            }
        }
        listPath.Clear();
        while (current != null)
        {
            listPath.Add(new Vector2(current.X, current.Y));
            current = current.Parent;
        }
        listPath.Reverse();

        //player.transform.position = new Vector3(0.5f, 0.5f, 0);
        //player.GetComponent<PlayerController>().ResetMap(listPath);
    }
    static float ComputeHScore(float x, float y, float targetX, float targetY)
    {
        return Math.Abs(targetX - x) + Math.Abs(targetY - y);
    }
    private static List<Location> GetWalkableAdjacentSquares(float x, float y, Location target)
    {
        var proposedLocations = new List<Location>();
        Vector2 currentPos = new Vector2(x, y);
        Vector2 down = new Vector2(x, y - 1);
        Vector2 up = new Vector2(x, y + 1);
        Vector2 left = new Vector2(x - 1, y);
        Vector2 right = new Vector2(x + 1, y);

        if (target.isEqual(up) && listConnected.FirstOrDefault(l => l.isContain(currentPos,up)) != null)
            proposedLocations.Add(new Location { X = x, Y = y + 1 });
        if (target.isEqual(down) && listConnected.FirstOrDefault(l => l.isContain(currentPos, down)) != null)
            proposedLocations.Add(new Location { X = x, Y = y - 1 });
        if (target.isEqual(left) && listConnected.FirstOrDefault(l => l.isContain(currentPos, left)) != null)
            proposedLocations.Add(new Location { X = x - 1, Y = y });
        if (target.isEqual(right) && listConnected.FirstOrDefault(l => l.isContain(currentPos, right)) != null)
            proposedLocations.Add(new Location { X = x + 1, Y = y });

        foreach (NodeConnection n in listConnected)
        {
            if (n.isContain(currentPos, down))
                proposedLocations.Add(new Location { X = x, Y = y - 1 });

            if (n.isContain(currentPos, up))
                proposedLocations.Add(new Location { X = x, Y = y + 1 });

            if (n.isContain(currentPos, left))
                proposedLocations.Add(new Location { X = x - 1, Y = y });

            if (n.isContain(currentPos, right))
                proposedLocations.Add(new Location { X = x + 1, Y = y });
        }
     
        return proposedLocations;
    }
    bool SearchConnectedList(NodeConnection node)
    {
        foreach (NodeConnection n in listConnected)
        {
            if (n.isEqual(node))
                return true;
        }
        return false;
    }
    void DeleteConnected(NodeConnection node)
    {
        for(int i=0;i<listConnected.Count;i++)
        {
            if(listConnected[i].isEqual(node))
            {
                listConnected.RemoveAt(i);
                i--;
            }
        }
    }
    void Start()
    {
        listMaze = new List<GameObject>();
        GenerateMaze();
    }
    void GenerateBoudary()
    {
            for (int i = 0; i < row; i++)
            {
                DrawLine(new Vector3(i, 0, 0), new Vector3(i + 1, 0, 0));
                DrawLine(new Vector3(0, i, 0), new Vector3(0, i + 1, 0));
                DrawLine(new Vector3(i, row, 0), new Vector3(i + 1, row, 0));
                DrawLine(new Vector3(row, i, 0), new Vector3(row, i + 1, 0));
            }
    }
    void DrawConnectionLine(Vector3 start, Vector3 end)
    {
        GameObject sample1 = Instantiate(lineConnection);
        LineRenderer lineRenderer1 = sample1.GetComponent<LineRenderer>();
        lineRenderer1.SetPosition(0, start);
        lineRenderer1.SetPosition(1, end);
        listMaze.Add(sample1);
    }
    void DrawConnectionLine(NodeConnection connect)
    {
        GameObject sample1 = Instantiate(lineConnection);
        LineRenderer lineRenderer1 = sample1.GetComponent<LineRenderer>();
        lineRenderer1.SetPosition(0, connect.start);
        lineRenderer1.SetPosition(1, connect.end);
        listMaze.Add(sample1);
    }
    void DrawLine(Vector3 start, Vector3 end)
    {
        GameObject sample1 = Instantiate(line);
        LineRenderer lineRenderer1 = sample1.GetComponent<LineRenderer>();
        lineRenderer1.SetPosition(0, start);
        lineRenderer1.SetPosition(1, end);
        listMaze.Add(sample1);
    }
    void DrawLine(Vector2 start, Vector2 end)
    {
        GameObject sample1 = Instantiate(line);
        LineRenderer lineRenderer1 = sample1.GetComponent<LineRenderer>();
        lineRenderer1.SetPosition(0, start);
        lineRenderer1.SetPosition(1, end);
        listMaze.Add(sample1);
    }
    // Update is called once per frame
    private bool isWin = false;
    void Update()
    {

        nextLevelButton.interactable = isWin;
        if(Vector3.Distance(player.transform.position,new Vector3(14.5f,14.5f,0)) < 0.01 && !isWin)
        {
            isWin = true;
            canvasMenu.SetActive(true);
        }
        if(Vector3.Distance(player.transform.position, enemy.transform.position) < 0.01)
        {
            isWin = false;
            ShowMenu();
        }
    }
}
