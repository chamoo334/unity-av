using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraView : MonoBehaviour
{
    // Camera mainCam;
    // -1 = topview, 0 = rotation[0], 1 = roatation[1], 2 = rotation[2], 3 = rotation[3], 4 = bottomview
    static float[,] topView = {{0, 100, 0},{90, 0, 0}};
    static float[,] bottomView = {{0,-50,0},{270,0,0}};
    int mainCamPos = -1;
    float[,,] rotation = new float[4,2,3]{
            {{0,50,-100},{50,0,0}}, //frontView
            {{-100,50,0},{0,45,0}}, //leftView
            {{0,50,100},{75,100,0}}, //backView
            {{100,50,0},{0,315,0}} // rightView
        };


    // Start is called before the first frame update
    void Start()
    {
        // mainCam = Camera.main;
        // this.transform.position = new Vector3 (,,);
        // this.transform.rotation = new Vector3 (,,);
    }

    // Update is called once per frame
    void Update()
    {
        // Vector3 newPos = null;
        if (Input.GetKeyDown(KeyCode.UpArrow)){
            mainCamPos = -1;
            this.transform.position = new Vector3(topView[0,0], topView[0,1], topView[0,2]);
            this.transform.rotation = Quaternion.Euler(topView[1,0], topView[1,1], topView[1,2]);

        } else if (Input.GetKeyDown(KeyCode.DownArrow)){
            mainCamPos = 4;
            this.transform.position = new Vector3(bottomView[0,0], bottomView[0,1], bottomView[0,2]);
            this.transform.rotation = Quaternion.Euler(bottomView[1,0], bottomView[1,1], bottomView[1,2]);
        } else if (Input.GetKeyDown(KeyCode.LeftArrow)){
            if (mainCamPos < 0 || mainCamPos > 3){
                mainCamPos = 0;
                
                this.transform.position = new Vector3(rotation[mainCamPos,0,0], rotation[mainCamPos,0,1], rotation[mainCamPos,0,2]);
                this.transform.rotation = Quaternion.Euler(rotation[mainCamPos,1,0], rotation[mainCamPos,1,1], rotation[mainCamPos,1,2]);
            } else if(mainCamPos == 0){
                mainCamPos = 3;
                this.transform.position = new Vector3(rotation[mainCamPos,0,0], rotation[mainCamPos,0,1], rotation[mainCamPos,0,2]);
                this.transform.rotation = Quaternion.Euler(rotation[mainCamPos,1,0], rotation[mainCamPos,1,1], rotation[mainCamPos,1,2]);
            } else {
                mainCamPos -= 1;
                this.transform.position = new Vector3(rotation[mainCamPos,0,0], rotation[mainCamPos,0,1], rotation[mainCamPos,0,2]);
                this.transform.rotation = Quaternion.Euler(rotation[mainCamPos,1,0], rotation[mainCamPos,1,1], rotation[mainCamPos,1,2]);
            }
        } else if (Input.GetKeyDown(KeyCode.RightArrow)){
            if (mainCamPos < 0 || mainCamPos > 3){
                mainCamPos = 3;
                this.transform.position = new Vector3(rotation[mainCamPos,0,0], rotation[mainCamPos,0,1], rotation[mainCamPos,0,2]);
                this.transform.rotation = Quaternion.Euler(rotation[mainCamPos,1,0], rotation[mainCamPos,1,1], rotation[mainCamPos,1,2]);
            } else if(mainCamPos == 3){
                mainCamPos = 0;
                this.transform.position = new Vector3(rotation[mainCamPos,0,0], rotation[mainCamPos,0,1], rotation[mainCamPos,0,2]);
                this.transform.rotation = Quaternion.Euler(rotation[mainCamPos,1,0], rotation[mainCamPos,1,1], rotation[mainCamPos,1,2]);
            } else {
                mainCamPos += 1;
                this.transform.position = new Vector3(rotation[mainCamPos,0,0], rotation[mainCamPos,0,1], rotation[mainCamPos,0,2]);
                this.transform.rotation = Quaternion.Euler(rotation[mainCamPos,1,0], rotation[mainCamPos,1,1], rotation[mainCamPos,1,2]);
            }
        }
    }
}
