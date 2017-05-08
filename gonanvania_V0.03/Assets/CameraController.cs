using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraState { Normal, CutScene, Pause, Gameover };

public class CameraController : MonoBehaviour {

    public CameraState currentState;
    public GameObject player;
    Vector3 playerPos;

    public float speedLookaheadFactor;

    Vector3 targetPosGizmo;

    public float smoothTime;
    Vector3 currentVelocity = Vector3.zero;
    public float yOffset;
    public float cameraZ;
    public float lerpFactor = 3;

    void Start () {
        
    }

	void Update () {

     
	}

    void FixedUpdate() {
        var pos = transform.position;
        var targetPos = pos;
        playerPos = player.transform.position;
        playerPos += new Vector3(0, yOffset, 0);
        playerPos.z = cameraZ;
        //targetPos.x = playerPos.x;
        //targetPos.y = playerPos.y;

        //targetPos.y += speedLookaheadFactor * player.GetComponent<Player>().smoothedVerticalSpeed + yOffset;
        //targetPos.x = playerPos.x;

        //transform.position = Vector3.SmoothDamp(pos, targetPos, ref currentVelocity, smoothTime);
        transform.position = Vector3.Lerp(pos, playerPos, Time.deltaTime * lerpFactor);
        //transform.position = new Vector3(playerPos.x, transform.position.y, cameraZ);
        targetPosGizmo = targetPos;
    }

    void OnDrawGizmos() {
        Gizmos.DrawWireSphere(targetPosGizmo, 1f);
    }
}
