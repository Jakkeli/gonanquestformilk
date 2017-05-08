using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraState { Normal, CutScene, Pause, Gameover };

public class CameraController : MonoBehaviour {

    public CameraState currentState;
    public GameObject player;
    Vector3 playerPos;

    //public float speedLookaheadFactor;

    //Vector3 targetPosGizmo;

    //public float smoothTime;
    //Vector3 currentVelocity = Vector3.zero;
    public float yOffset;
    public float cameraZ;
    public float lerpFactor = 3;
    public float goDownAdd;

    void Start () {
        
    }

	void Update () {

     
	}

    void FixedUpdate() {
        var pos = transform.position;
        var targetPos = pos;
        playerPos = player.transform.position;
        targetPos = player.transform.position;
        targetPos.x = playerPos.x;
        targetPos.z = cameraZ;
        //if (player.GetComponent<Rigidbody>().velocity.y < 0) {
        //    playerPos.y += -goDownAdd + yOffset;
        //}
        //else {
        //    playerPos.y += yOffset;
        //}
        
        //playerPos.z = cameraZ;
        targetPos = Vector3.Lerp(pos, targetPos, Time.deltaTime * lerpFactor);
        targetPos.y += yOffset;
        targetPos.x = playerPos.x;
        transform.position = targetPos;
        //transform.position = new Vector3(playerPos.x, transform.position.y, cameraZ);


        //targetPos.x = playerPos.x;
        //targetPos.y = playerPos.y;

        //targetPos.y += speedLookaheadFactor * player.GetComponent<Player>().smoothedVerticalSpeed + yOffset;
        //targetPos.x = playerPos.x;

        //transform.position = Vector3.SmoothDamp(pos, targetPos, ref currentVelocity, smoothTime);

        //transform.position = new Vector3(playerPos.x, transform.position.y, cameraZ);
        //targetPosGizmo = targetPos;
    }

    void OnDrawGizmos() {
        //Gizmos.DrawWireSphere(targetPosGizmo, 1f);
    }
}
