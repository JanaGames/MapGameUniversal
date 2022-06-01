using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMap : MonoBehaviour
{
    //speed of update icon of player
    [Range(0.1f, 1f)]
    public float timingUpdate = 0.1f;


    //    B point______________________________C point
    //    |                                    |
    //    |   The map captured by the image    |
    //    |                                    |
    //    A point______________________________D point
    public Transform pointA;
    public Transform pointB;
    public Transform pointC;
    public Transform pointD;
    //the map set on canvas
    public Image map;
    //set as child of map image
    public Image playerIcon;

    float sideMapX;
    float sideMapZ;

    public KeyCode mapKey;

    void Start() 
    {
        Invisible();
        sideMapX = map.GetComponent<RectTransform>().rect.width;
        sideMapZ = map.GetComponent<RectTransform>().rect.height;
    }
    //UpdateMap()
    //and call when required
    void Update() 
    {
        if (Input.GetKeyDown(mapKey)) 
        {
            if (!map.gameObject.activeSelf) OpenMap();
            else Invisible();
        }
    }
    public void OpenMap() 
    {
        map.gameObject.SetActive(true);
        StartCoroutine(CheckPosPlayer(timingUpdate));
    }
    public void Invisible() 
    {
        StopAllCoroutines();
        map.gameObject.SetActive(false);
    }
    IEnumerator CheckPosPlayer(float time)
    {
        Player player = Player.localPlayer;
        while (true)
        {
            float x = GetSide(pointA.position, pointB.position, player.transform.position);
            float z = GetSide(pointD.position, pointA.position, player.transform.position);
            float posOnMapX = sideMapX * x;
            float posOnMapZ = sideMapZ * z;
            playerIcon.GetComponent<RectTransform>().anchoredPosition = new Vector2(posOnMapX, posOnMapZ);
            yield return new WaitForSeconds(time);
        }
    }
    float GetSide(Vector3 pointA, Vector3 pointB, Vector3 player) 
    {
        float res = 0;
        Vector3 posOnLine = GetPositionOnSegment(pointA, pointB, player);
        float partOfLine = DistanceBetweenPoints(pointA, posOnLine);
        res = partOfLine / DistanceBetweenPoints(pointA, pointB);
        return res;
    }
    float DistanceBetweenPoints(Vector3 pointA, Vector3 pointB) 
    {
        Vector2 vectorA = new Vector2(pointA.x, pointA.z);
        Vector2 vectorB = new Vector2(pointB.x, pointB.z);

        return Vector2.Distance(vectorA, vectorB);
    }
    public Vector3 GetPositionOnSegment( Vector3 A, Vector3 B, Vector3 point)
    {
        Vector3 projection = Vector3.Project( point - A, B - A ) ;
        return projection + A ;
    }
}
