using System;
using System.Collections;
using System.Collections.Generic;
using QuadTree;
using UnityEngine;
using Random = UnityEngine.Random;
using Tree = QuadTree.Tree;

public class GameApp : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private GameObject terrain;
    [SerializeField] private List<GameObject> prefabList = new List<GameObject>();

    private Tree tree;
    private Dictionary<string, GameObject> objMap = new Dictionary<string, GameObject>();

    private Bounds bounds;

    private List<string> oldList = new List<string>();
    private List<string> newList = new List<string>();

    void Start()
    {
        bounds = new Bounds(Vector3.zero, new Vector3(1000, 10, 1000));
        tree = new Tree(bounds,5);
        CreateTerrain();
        RefreshCamera();
    }

    void Update()
    {
        bool camera_rotate = false;
        if (Input.GetKey(KeyCode.A))
        {
            RotateCamera(-1);
            camera_rotate = true;
        }

        if (Input.GetKey(KeyCode.D))
        {
            RotateCamera(1);
            camera_rotate = true;
        }

        if (camera_rotate)
        {
            RefreshCamera();
        }
    }

    private void RotateCamera(int angleY)
    {
        camera.transform.Rotate(new Vector3(0, angleY, 0), Space.Self);
    }


    private void RefreshCamera()
    {
        newList.Clear();
        tree.TriggerMove(camera, (data) =>
        {
            newList.Add(data.uid);
            if (oldList.Contains(data.uid))
            {
                oldList.Remove(data.uid);
            }

            objMap[data.uid].gameObject.SetActive(true);
        });
        foreach (var temp in oldList)
        {
            objMap[temp].gameObject.SetActive(false);
        }

        oldList.Clear();
        newList.ForEach(i => oldList.Add(i));
    }


    //创建随机地形。
    private void CreateTerrain()
    {
        objMap.Clear();
        for (int i = 0; i < 100; i++)
        {
            var prefabIndex = Random.Range(0, 2);
            var position = new Vector3(Random.Range(-500, 500), Random.Range(1, 5), Random.Range(-500, 500));
            var angle = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
            var obj = GameObject.Instantiate(prefabList[prefabIndex]);
            obj.transform.position = position;
            obj.transform.rotation = Quaternion.Euler(angle);
            obj.transform.localScale = Vector3.one * 10;
            obj.SetActive(false);
            string uid = obj.GetHashCode().ToString();
            objMap.Add(uid, obj);
            tree.InsertObj(new ObjData(uid, obj.transform.position));
        }
    }


    private void OnDrawGizmos()
    {
        if (tree != null)
            tree.DrawBound();
    }
}