using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingScript : MonoBehaviour {

    public LevelGrid grid;
    public Transform rampTransform;
    public GameObject cubePrefab, rampPrefab;
    public CameraMovement camMovement;
    public GameObject down, left, right, top, front, bottom;
    public bool destroy, built;
    public int limitConstruction;

    // Use this for initialization
    void Start()
    {
        built = false;
        destroy = false;
        rampTransform.rotation = this.transform.rotation;
    }
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButton(0))
        {
            built = false;
            if (!destroy)
            {
                


                    Build(cubePrefab);
                
            }
            else
            {
               Destroy();
            }

        }
        if (Input.GetMouseButton(1))
        {
            if (!destroy)
            {
                Build(rampPrefab);
            }
            else
            {
                Destroy();
            }

        }
        if (Input.GetKeyDown (KeyCode.O))
        {
            destroy = !destroy;
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            RotateRamp();
        }
    }

    void Build(GameObject piecePrefab)
    {

        RaycastHit[] hits;

        hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition), 100.0f); //llista de colliders que troba al apretar ratolí
        foreach (var hit in hits)
        {
          

            
            if (grid.GetCellInWorldPosition(hit.transform.position) != null)
            {
                if (grid.GetCellInWorldPosition(hit.transform.position).buildable)
                {
                    if (grid.GetCellInWorldPosition(hit.transform.position).gameObject == null)
                    {
                        //comprova que on clico esta aprop del player
                        if ((Mathf.Abs(grid.GetCellInWorldPosition(hit.transform.position).globalPosition.x - transform.position.x)) < limitConstruction &&
                            (Mathf.Abs(grid.GetCellInWorldPosition(hit.transform.position).globalPosition.y - transform.position.y)) < limitConstruction &&
                            (Mathf.Abs(grid.GetCellInWorldPosition(hit.transform.position).globalPosition.z - transform.position.z)) < limitConstruction
                            )
                        {
                            if (Vector3.Distance(grid.GetCellInWorldPosition(hit.transform.position).globalPosition, transform.position) > 0.5f) //comproba que no estic apretant la casella del player
                            {
                               


                                    


                                        //instancia cubo i el guarda a la cela que li toca
                                        GameObject piece = Instantiate(piecePrefab, grid.GetCellInWorldPosition(hit.transform.position).gridPosition, rampTransform.rotation);

                                            grid.GetCellInWorldPosition(hit.transform.position).gameObject = piece;
                                            grid.GetCellInWorldPosition(hit.transform.position).transitable = false;
                                           
                                            switch (CameraMovement.Lateral)
                                            {
                                                case CameraMovement.Axes.xNeg: piece.transform.parent = left.transform; break;
                                                case CameraMovement.Axes.xPos: piece.transform.parent = right.transform; break;
                                                case CameraMovement.Axes.yNeg: piece.transform.parent = top.transform; break;
                                                case CameraMovement.Axes.yPos: piece.transform.parent = down.transform; break;
                                                case CameraMovement.Axes.zNeg: piece.transform.parent = front.transform; break;
                                                case CameraMovement.Axes.zPos: piece.transform.parent = bottom.transform; break;
                                            }
                                        
                                    
                                
                            }
                        }
                    }
                    

                }

            }
        }
    }
    void   Destroy()
    {
        RaycastHit[] hits;

        hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition), 100.0f); //llista de colliders que troba al apretar ratolí
        foreach (var hit in hits)
        {
            //Debug.Log(grid.GetCellInPosition(hit.transform.position).gameObject);
            if (grid.GetCellInWorldPosition(hit.transform.position) != null)
            {
                if (grid.GetCellInWorldPosition(hit.transform.position).buildable)
                {
                    if (grid.GetCellInWorldPosition(hit.transform.position).gameObject != null)
                    {
                      
                        DestroyObject((grid.GetCellInWorldPosition(hit.transform.position).gameObject));
                        grid.GetCellInWorldPosition(hit.transform.position).transitable = true;
                        grid.GetCellInWorldPosition(hit.transform.position).buildable = true;

                    }
                }
            }
        }
    }
       /*

        if (Input.GetMouseButtonDown(1))
        {
            //fa el mateix exacte que abans, enlloc de cubo instancia rampa
            RaycastHit[] hits;

            hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition), 100.0f);

            foreach (var hit in hits)
            {
                if (grid.GetCellInPosition(hit.transform.position) != null)
                {
                    if (grid.GetCellInPosition(hit.transform.position).buildable)
                    {
                        if (grid.GetCellInPosition(hit.transform.position).gameObject == null)
                        {
                            if ((Mathf.Abs(grid.GetCellInPosition(hit.transform.position).globalPosition.x - transform.position.x)) < 2 &&
                                (Mathf.Abs(grid.GetCellInPosition(hit.transform.position).globalPosition.y - transform.position.y)) < 2 &&
                                (Mathf.Abs(grid.GetCellInPosition(hit.transform.position).globalPosition.z - transform.position.z)) < 2
                                )
                            {
                                if (Vector3.Distance(grid.GetCellInPosition(hit.transform.position).globalPosition, transform.position) > 0.5f) //comproba que no estic apretant la casella del player
                                {
                                    GameObject ramp = Instantiate(rampPrefab, hit.transform.position, rampTransform.rotation);
                                    //  ramp.transform.up = this.transform.up;
                                    // ramp.transform.rotation = rampTransform.rotation;
                                    grid.GetCellInPosition(hit.transform.position).gameObject = ramp;
                                }
                            }
                        }
                    }
                }

            }
        }
        */
    
    void RotateRamp()
    {
        
            /*
            switch (camMovement.Lateral)
            {
                case CameraMovement.Axes.xPos:
                    rampTransform.Rotate(new Vector3(90.0f, 0.0f, 0.0f));
                    break;
                case CameraMovement.Axes.yPos:
                    rampTransform.Rotate(new Vector3(0.0f, 90.0f, 0.0f));
                    break;
                case CameraMovement.Axes.zPos:
                    rampTransform.Rotate(new Vector3(0.0f, 0.0f, 90.0f));
                    Debug.Log("Z");

                    break;
                case CameraMovement.Axes.xNeg:
                    rampTransform.Rotate(new Vector3(90.0f, 0.0f, 0.0f));

                    break;
                case CameraMovement.Axes.yNeg:
                    rampTransform.Rotate(new Vector3(0.0f, 90.0f, 0.0f));

                    break;
                case CameraMovement.Axes.zNeg:
                    Debug.Log("Z");

                    rampTransform.Rotate(new Vector3(0.0f, 0.0f, 90.0f));

                    break;
                default:
                    break;
            }
            */
            // rampTransform.rotation = Quaternion.Euler(0f, 0f, 0f);
            rampTransform.Rotate(new Vector3(0.0f, 90.0f, 0.0f));
        
    }
}
