using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    //Cells stuff
    public LevelGrid grid;
    Cell currentCell, nextCell, initialCell;
    Vector3 initialRotation;
    List<Cell> playerNeighbours;
    bool falling = false;
    //Cell[,,] cellMatrix;

    //Movement
    public float speed;
    bool started = false;
    public Collider playerCollider;

    //Camera
    public CameraMovement camMovement;
    public Camera cam;

    //Building 
    public BuildingScript buildScript; //cuando juntemos buildScript con PlayerScript no hará falta 

    //Gravity orientation
    //string actualWall = "azul";

    //public GameObject cubePrefab;
    //public Vector3 forward;

    void Start()
    {
        //playerCollider.enabled = false;
        currentCell = grid.GetCellInWorldPosition(transform.position);
        transform.position = currentCell.globalPosition;
        initialCell = currentCell;
        initialRotation = transform.eulerAngles;
        //forward = transform.forward;
        //cellMatrix = grid.returnGrid();
    }

    void Update()
    {
        //Movement();
        //SetBuildableCells();

    }

    void Movement()
    {
        //Checking if arribed to the center of the next Cell
        if (nextCell != null && transform.position == nextCell.globalPosition)
        {
            currentCell = grid.GetCellInWorldPosition(transform.position);

            Vector3 abovePos = Vector3.zero;

            switch (CameraMovement.Lateral)
            {
                case CameraMovement.Axes.xNeg: abovePos = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z); break;
                case CameraMovement.Axes.xPos: abovePos = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z); break;
                case CameraMovement.Axes.yNeg: abovePos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z); break;
                case CameraMovement.Axes.yPos: abovePos = new Vector3(transform.position.x, transform.position.y - 1, transform.position.z); break;
                case CameraMovement.Axes.zNeg: abovePos = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1); break;
                case CameraMovement.Axes.zPos: abovePos = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1); break;
            }

            if (grid.GetCellInWorldPosition(abovePos) != null)
            {
                if (grid.GetCellInWorldPosition(transform.position - abovePos).actualType == Cell.types.empty)
                    falling = true;
            }

            if (falling)
            {
                nextCell = grid.GetCellInWorldPosition(abovePos);

                if (nextCell == null || grid.GetCellInWorldPosition(transform.position - abovePos).actualType != Cell.types.empty)
                {
                    falling = false;
                }
            }
        }

        //If is not moving, check for new instructions

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            buildScript.rampTransform.up = this.transform.up; //asigna orientación (up) a como colocar la rampa

            if (!started) { started = true; }

            nextCell = grid.GetCellInWorldPosition(transform.position + new Vector3(Mathf.RoundToInt(transform.forward.x), Mathf.RoundToInt(transform.forward.y), Mathf.RoundToInt(transform.forward.z)));

            if (started && nextCell == null)
            {

                Vector2 actualScreenPoint = cam.WorldToScreenPoint(transform.position);
                Vector2 futureScreenPoint = cam.WorldToScreenPoint(transform.position + new Vector3(Mathf.RoundToInt(transform.forward.x), Mathf.RoundToInt(transform.forward.y), Mathf.RoundToInt(transform.forward.z)));

                if (futureScreenPoint.x > actualScreenPoint.x && futureScreenPoint.y > actualScreenPoint.y)
                {
                    camMovement.RotateTo(CameraMovement.Rotations.dRightUp);
                  //  Debug.Log("cambiopantalla");

                    //buildScript.rampTransform.up = this.transform.up;

                }
                else if (futureScreenPoint.x < actualScreenPoint.x && futureScreenPoint.y < actualScreenPoint.y)
                {

                    camMovement.RotateTo(CameraMovement.Rotations.dRightDown);

                }
                else if (futureScreenPoint.x < actualScreenPoint.x && futureScreenPoint.y > actualScreenPoint.y)
                {

                    camMovement.RotateTo(CameraMovement.Rotations.dLeftUp);


                }
                else if (futureScreenPoint.x > actualScreenPoint.x && futureScreenPoint.y < actualScreenPoint.y)
                {

                    camMovement.RotateTo(CameraMovement.Rotations.dLeftDown);


                }

                transform.Rotate(Vector3.right, -90);
                //playerCollider.enabled = true;
            }
        }

        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (!started) { started = true; }
            nextCell = grid.GetCellInWorldPosition(transform.position - new Vector3(Mathf.RoundToInt(transform.forward.x), Mathf.RoundToInt(transform.forward.y), Mathf.RoundToInt(transform.forward.z)));
        }

        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.Rotate(Vector3.up, 90);
        }

        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.Rotate(Vector3.up, -90);
        }

        if (nextCell != null && nextCell.transitable)
        {
            transform.position = Vector3.MoveTowards(transform.position, nextCell.globalPosition, speed * Time.deltaTime);
        }
        else if (nextCell != null && nextCell.gameObject.tag == "RAMP")
        {
            Debug.Log("Ramp detected");
            //Forward ramp and forward player are alienated
            if (nextCell.gameObject.transform.forward == transform.forward)
            {
                Debug.Log("ramo alienated");
                Vector3 offset = new Vector3(0,0,0);

                if (CameraMovement.Lateral == CameraMovement.Axes.xPos) {
                    if (transform.forward.y == 1) offset = new Vector3(1, 1, 0);
                    else if (transform.forward.y == -1) offset = new Vector3(1, -1, 0);
                    else if (transform.forward.z == 1) offset = new Vector3(1, 0, 1);
                    else if (transform.forward.z == -1) offset = new Vector3(1, 0, -1);
                }
                else if (CameraMovement.Lateral == CameraMovement.Axes.xNeg)
                {
                    if (transform.forward.y == 1) offset = new Vector3(-1, 1, 0);
                    else if (transform.forward.y == -1) offset = new Vector3(-1, -1, 0);
                    else if (transform.forward.z == 1) offset = new Vector3(-1, 0, 1);
                    else if (transform.forward.z == -1) offset = new Vector3(-1, 0, -1);
                }



                else if (CameraMovement.Lateral == CameraMovement.Axes.yPos)
                {
                    if (transform.forward.x == 1) offset = new Vector3(1, 1, 0);
                    else if (transform.forward.x == -1) offset = new Vector3(-1, 1, 0);
                    else if (transform.forward.z == 1) offset = new Vector3(0, 1, 1);
                    else if (transform.forward.z == -1) offset = new Vector3(0, 1, -1);
                }
                else if (CameraMovement.Lateral == CameraMovement.Axes.yNeg)
                {
                    if (transform.forward.x == 1) offset = new Vector3(1, -1, 0);
                    else if (transform.forward.x == -1) offset = new Vector3(-1, -1, 0);
                    else if (transform.forward.z == 1) offset = new Vector3(0, -1, 1);
                    else if (transform.forward.z == -1) offset = new Vector3(0, -1, -1);
                }



                else if (CameraMovement.Lateral == CameraMovement.Axes.zPos)
                {
                    if (transform.forward.y == 1) offset = new Vector3(0, 1, 1);
                    else if (transform.forward.y == -1) offset = new Vector3(0, -1, 1);
                    else if (transform.forward.x == 1) offset = new Vector3(1, 0, 1);
                    else if (transform.forward.x == -1) offset = new Vector3(-1, 0, 1);
                }
                else if (CameraMovement.Lateral == CameraMovement.Axes.zNeg)
                {
                    if (transform.forward.y == 1) offset = new Vector3(0, 1, -1);
                    else if (transform.forward.y == -1) offset = new Vector3(0, -1, -1);
                    else if (transform.forward.x == 1) offset = new Vector3(1, 0, -1);
                    else if (transform.forward.x == -1) offset = new Vector3(-1, 0, -1);
                }
                Debug.Log("offset  " + offset);
                if (!started) { started = true; }
                nextCell = grid.GetCellInWorldPosition(transform.position + new Vector3(Mathf.RoundToInt(transform.forward.x + offset.x), Mathf.RoundToInt(transform.forward.y + offset.y), Mathf.RoundToInt(transform.forward.z + offset.z)));
               // Debug.Log(nextCell.globalPosition);

                if (nextCell != null && nextCell.transitable)
                {
                    transform.position = Vector3.MoveTowards(transform.position, nextCell.globalPosition, speed * Time.deltaTime);
                }
            }
        }
    }

  

    void SetBuildableCells()
    {/* 
        // Debug.Log(transform.position);
        //Debug.Log(grid.GetCellInPosition(transform.position).globalPosition);
        playerNeighbours = grid.GetNeighbours(grid.GetCellInPosition(transform.position));
        // Debug.Log(playerNeighbours.Count);
        
         playerNeighbours.ForEach(delegate (Cell cell)
         {
             Debug.Log("hey");

             Debug.Log(cell.gridPosition);
             cell.buildable = true;

         });
         */
        /*
        foreach (Cell neighbourCell in playerNeighbours)
        {
           // Debug.Log("hey");

            Debug.Log(neighbourCell.gridPosition);
            //neighbourCell.buildable = true;

            foreach (Cell gridCell in cellMatrix)
            {
                if (gridCell != neighbourCell)
                {
                    gridCell.buildable = false;
                }
                else
                {
                    gridCell.buildable = true;
                }
            }
        }

        foreach (Cell gridCell in cellMatrix)
        {
            if (playerNeighbours.Contains(gridCell))
            {
                gridCell.buildable = true;

            }
            else
            {
               // gridCell.buildable = false;

            }
        }*/
    }

    /*private void OnTriggerStay(Collider other)
    {


        if (other.gameObject.tag != actualWall)
        {


            switch (actualWall)
            {
                case "azul":
                    switch (other.gameObject.tag)
                    {
                        case "caqui": camMovement.RotateTo(CameraMovement.Rotations.dRightUp); actualWall = "caqui"; playerCollider.enabled = false; break;
                        case "verde": camMovement.RotateTo(CameraMovement.Rotations.dRightDown); actualWall = "verde"; playerCollider.enabled = false; break;
                        case "negro": camMovement.RotateTo(CameraMovement.Rotations.dLeftUp); actualWall = "negro"; playerCollider.enabled = false; break;
                        case "blanco": camMovement.RotateTo(CameraMovement.Rotations.dLeftDown); actualWall = "blanco"; playerCollider.enabled = false; break;
                    }
                    break;

                case "verde":
                    switch (other.gameObject.tag)
                    {
                        case "azul": camMovement.RotateTo(CameraMovement.Rotations.dRightUp); actualWall = "azul"; playerCollider.enabled = false; break;
                        case "rosa": camMovement.RotateTo(CameraMovement.Rotations.dRightDown); actualWall = "rosa"; playerCollider.enabled = false; break;
                        case "negro": camMovement.RotateTo(CameraMovement.Rotations.dLeftUp); actualWall = "negro"; playerCollider.enabled = false; break;
                        case "blanco": camMovement.RotateTo(CameraMovement.Rotations.dLeftDown); actualWall = "blanco"; playerCollider.enabled = false; break;
                    }
                    break;

                case "rosa":
                    switch (other.gameObject.tag)
                    {
                        case "verde": camMovement.RotateTo(CameraMovement.Rotations.dRightUp); actualWall = "verde"; playerCollider.enabled = false; break;
                        case "caqui": camMovement.RotateTo(CameraMovement.Rotations.dRightDown); actualWall = "caqui"; playerCollider.enabled = false; break;
                        case "negro": camMovement.RotateTo(CameraMovement.Rotations.dLeftUp); actualWall = "negro"; playerCollider.enabled = false; break;
                        case "blanco": camMovement.RotateTo(CameraMovement.Rotations.dLeftDown); actualWall = "blanco"; playerCollider.enabled = false; break;
                    }
                    break;

                case "caqui":
                    switch (other.gameObject.tag)
                    {
                        case "rosa": camMovement.RotateTo(CameraMovement.Rotations.dRightUp); actualWall = "rosa"; playerCollider.enabled = false; break;
                        case "azul": camMovement.RotateTo(CameraMovement.Rotations.dRightDown); actualWall = "azul"; playerCollider.enabled = false; break;
                        case "negro": camMovement.RotateTo(CameraMovement.Rotations.dLeftUp); actualWall = "negro"; playerCollider.enabled = false; break;
                        case "blanco": camMovement.RotateTo(CameraMovement.Rotations.dLeftDown); actualWall = "blanco"; playerCollider.enabled = false; break;
                    }
                    break;

                case "blanco":
                    switch (other.gameObject.tag)
                    {
                        case "caqui": camMovement.RotateTo(CameraMovement.Rotations.dRightUp); actualWall = "caqui"; playerCollider.enabled = false; break;
                        case "verde": camMovement.RotateTo(CameraMovement.Rotations.dRightDown); actualWall = "verde"; playerCollider.enabled = false; break;
                        case "azul": camMovement.RotateTo(CameraMovement.Rotations.dLeftUp); actualWall = "azul"; playerCollider.enabled = false; break;
                        case "rosa": camMovement.RotateTo(CameraMovement.Rotations.dLeftDown); actualWall = "rosa"; playerCollider.enabled = false; break;
                    }
                    break;

                case "negro":
                    switch (other.gameObject.tag)
                    {
                        case "caqui": camMovement.RotateTo(CameraMovement.Rotations.dRightUp); actualWall = "caqui"; playerCollider.enabled = false; break;
                        case "verde": camMovement.RotateTo(CameraMovement.Rotations.dRightDown); actualWall = "verde"; playerCollider.enabled = false; break;
                        case "rosa": camMovement.RotateTo(CameraMovement.Rotations.dLeftUp); actualWall = "rosa"; playerCollider.enabled = false; break;
                        case "azul": camMovement.RotateTo(CameraMovement.Rotations.dLeftDown); actualWall = "azul"; playerCollider.enabled = false; break;

                    }
                    break;
            }
        }
    }*/
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Victory")
        {
            Debug.Log("Congratulations, you solved the puzzle");
        }

        if (other.tag == "Trap")
        {
            print("OUCH");
            transform.position = initialCell.globalPosition;
            currentCell = initialCell;
            nextCell = null;
        }
    }

    void ResetPlayer()
    {
        transform.position = initialCell.globalPosition;
        currentCell = initialCell;
        transform.eulerAngles = initialRotation;
    }
}
