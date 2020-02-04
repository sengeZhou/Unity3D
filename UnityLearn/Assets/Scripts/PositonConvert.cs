﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 Canvas 的 Render Mode 设置为 Screen Space - Camera 并设置相机)时:

    Unity 中一个 UI 单位不再是对应一个 Unity 单位 ==> UI节点的position属性值不能直接赋给世界坐标系中3d物体的position属性值
     
     
*/
public class PositonConvert : MonoBehaviour {


    public RectTransform canvasRectTransform; //根canvas的RectTransform
    public RectTransform textRectTransform;   //子UI的canvas的RectTransform

    public Transform cubeTransform;

    public RectTransform localUIRectTransform;
    public RectTransform targetUIRectTransform;

    public RectTransform localUIRectTransform2;
    public RectTransform targetUIRectTransform2;

    public Camera UICamera;

    private Camera mainCamera;


    public Transform cubeTargetTransform;
    public Transform mainCameraTransform;


    // Use this for initialization
    void Start () {


        mainCamera = Camera.main;
        worldToScreenInUICamera();
        LocalUIToScreenAndWorld();

    }

    //UGUI 坐标转屏幕坐标 和世界坐标
    void LocalUIToScreenAndWorld()
    {

        RectTransform ParentRectTransform = (RectTransform)localUIRectTransform.parent;
        //targetUIRectTransform的父节点是根画布
        Vector2 targetScreenPos = localUIRectTransform.anchoredPosition + ParentRectTransform.anchoredPosition; //得到相对于根画布的坐标
        //targetUIRectTransform.anchoredPosition = targetScreenPos;


        Vector3 worldPos1 = localUIRectTransform.position;
        Vector3 screenPosOrgin1 = UICamera.WorldToScreenPoint(worldPos1);
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, screenPosOrgin1, UICamera, out localPos);
        targetUIRectTransform.anchoredPosition = localPos;

        //Vector2 targetScreenPos = screenPosOrgin1;

        Vector3 worldPos2 = localUIRectTransform2.position;
        Vector3 screenPosOrgin2 = UICamera.WorldToScreenPoint(worldPos2);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, screenPosOrgin2, UICamera, out localPos);
        targetUIRectTransform2.anchoredPosition = localPos;


        Vector3 localPostion = targetUIRectTransform.localPosition;
        Vector3 worldPostion = targetUIRectTransform.position;
        Debug.Log("targetScreenPos:" + targetScreenPos.x + "/  " + targetScreenPos.y);
        Debug.Log("localPostion:" + localPostion.x + "/  " + localPostion.y + "/  " + localPostion.z);
        Debug.Log("worldPostion:" + worldPostion.x + "/  " + worldPostion.y + "/  " + worldPostion.z);


        


        //转屏幕坐标
        // 1 有单独摄像机直接用UICamera.WorldToScreenPoint(worldPostion);
        Vector3 screenPosOrgin = UICamera.WorldToScreenPoint(worldPostion);
        Debug.Log("screenPosOrgin:" + screenPosOrgin.x + "/  " + screenPosOrgin.y + "/  " + screenPosOrgin.z);




        // 2 没有单独摄像机 需要使用拉伸比例
        //把一直遍历得到根画布的坐标系里的
        Vector2 RootCanvasPos = localUIRectTransform.anchoredPosition;
        RectTransform cur = localUIRectTransform;
        while (cur.parent)
        {
            cur = (RectTransform)cur.parent;
            if (cur == canvasRectTransform) break;
            RootCanvasPos += cur.anchoredPosition;
        }


        float rax = RootCanvasPos.x / 800; //canvas的宽
        float ray = RootCanvasPos.y / 450; //canvas的高
        Vector2 ratio = new Vector2(rax + 0.5f, ray + 0.5f); //根画布的中心点为中间，先把坐标系转到左下角
        Debug.Log("screenSize:" + Screen.width + "/  " + Screen.height);
        Vector2 screenPos = new Vector2(Screen.width * ratio.x, Screen.height * ratio.y); ; //是相对于根画布的坐标targetScreenPos
        Debug.Log("screenPos:" + screenPos.x + "/  " + screenPos.y);


        Vector2 RootCanvasPos2 = localUIRectTransform2.anchoredPosition;
        cur = localUIRectTransform2;
        while (cur.parent)
        {
            cur = (RectTransform)cur.parent;
            if (cur == canvasRectTransform) break;
            RootCanvasPos2 += cur.anchoredPosition;
        }

         rax = RootCanvasPos2.x / 800; //canvas的宽
         ray = RootCanvasPos2.y / 450; //canvas的高
         ratio = new Vector2(rax + 0.5f, ray + 0.5f); //根画布的中心点为中间，先把坐标系转到左下角
 
        Vector2 screenPos2 = new Vector2(Screen.width * ratio.x, Screen.height * ratio.y); ; //是相对于根画布的坐标targetScreenPos
        Debug.Log("screenPos2:" + screenPos2.x + "/  " + screenPos2.y);
        Debug.Log("screenPosOrgin2:" + screenPosOrgin2.x + "/  " + screenPosOrgin2.y + "/  " + screenPosOrgin2.z);


        //转世界坐标 （不同摄像机用屏幕坐标做中介转换）
        float oldz = cubeTargetTransform.position.z;
        Vector3 worldPosConvert5 = mainCamera.ScreenToWorldPoint(new Vector3(screenPosOrgin.x, screenPosOrgin.y, Mathf.Abs(mainCameraTransform.position.z) + oldz));//z为相机的z轴值绝对值
        Debug.Log("worldPosConvert5:" + worldPosConvert5.x + "/  " + worldPosConvert5.y + "/  " + worldPosConvert5.z);
        cubeTargetTransform.position = worldPosConvert5;
        

    }


    void worldToScreenInUICamera()
    {

        //世界坐标
        Vector3 wPos = cubeTransform.position;

        //转换成屏幕坐标
        Vector3 screenPos = Camera.main.WorldToScreenPoint(wPos);


        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, screenPos, UICamera, out localPos);
        textRectTransform.anchoredPosition = localPos;

        //public static bool ScreenPointToLocalPointInRectangle(RectTransform rect, Vector2 screenPoint, Camera cam, out Vector2 localPoint);
        //public static bool ScreenPointToWorldPointInRectangle(RectTransform rect, Vector2 screenPoint, Camera cam, out Vector3 worldPoint);
        //public static Vector2 WorldToScreenPoint(Camera cam, Vector3 worldPoint);


    }
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 screenPos = Input.mousePosition;
            Debug.Log("screenPos:" + screenPos.x + "/  " + screenPos.y);


            float oldz = cubeTargetTransform.position.z;
            //Vector3 worldPos = mainCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, Mathf.Abs(mainCameraTransform.localPosition.z))); //z为相机的z轴值绝对值

            //ScreenToWorldPoint,传入的vector3的z值 相当于距离摄像机的距离
            //获取的屏幕坐标Input.mousePosition是一个2d坐标，z轴值为0,这个z值是相对于当前camera的，为零表示z轴与相机重合了，
            //因此给ScreenToWorlfdPoint传值时，不能直接传Input.mousePosition，否则获取的世界坐标永远只有一个值；
            Vector3 worldPos = mainCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, Mathf.Abs(mainCameraTransform.position.z) + oldz)); //z为相机的z轴值绝对值
 
            cubeTargetTransform.position = worldPos;

            Vector2 outVec;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, Input.mousePosition, UICamera, out outVec);

            Debug.Log("Setting anchored positiont to: " + outVec);
            //textRectTransform.position = outVec;
            textRectTransform.anchoredPosition = outVec; //anchoredPosition 才是UGUI坐标系的坐标位置（属性面板里position属性）
            //textRectTransform.position = outVec;  //position是代表世界空间的坐标
        }
	}
}
