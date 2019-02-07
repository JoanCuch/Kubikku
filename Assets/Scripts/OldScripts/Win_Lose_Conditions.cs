using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Win_Lose_Conditions : MonoBehaviour {

    public bool trapped;
    public AxisMovement axisScript;
    public GameObject particulas;
    public GameObject auraH;
    public GameObject auraV;
    public ParticleSystem aura;
    public ParticleSystemRenderer renderAura;

    // Use this for initialization
    void Start () {
     //   Physics.gravity = new Vector3(0, 0, 10.0f);

        trapped = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(CameraMovement.Lateral);
         
           // GameObject part = Instantiate(particulas);
        }
        switch (CameraMovement.Lateral)
        {
            case CameraMovement.Axes.xPos:
                renderAura.renderMode = ParticleSystemRenderMode.VerticalBillboard;

                break;
            case CameraMovement.Axes.yPos:
                renderAura.renderMode = ParticleSystemRenderMode.HorizontalBillboard;


                break;
            case CameraMovement.Axes.zPos:
                renderAura.renderMode = ParticleSystemRenderMode.VerticalBillboard;

                break;
            case CameraMovement.Axes.xNeg:
                renderAura.renderMode = ParticleSystemRenderMode.VerticalBillboard;


                break;
            case CameraMovement.Axes.yNeg:
                renderAura.renderMode = ParticleSystemRenderMode.HorizontalBillboard;


                break;
            case CameraMovement.Axes.zNeg:
                renderAura.renderMode = ParticleSystemRenderMode.VerticalBillboard;

                break;
            default:
                break;
        }
    }
    /*private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Victory")
        {
            if (CameraMovement.Lateral == GameManager.gManager.requiredLateral &&
                CameraMovement.DiagonalRight == GameManager.gManager.requiredDRight &&
                CameraMovement.DiagonalLeft == GameManager.gManager.requiredDLeft
                )
            {


                Debug.Log("Congratulations, you solved the puzzle");
            }
        }

        if (other.tag == "Trap")
        {
            //axisScript.resetPlayer();
            GameManager.gManager.Loadscene(null);
            trapped = true;
        }
    }*/
}
