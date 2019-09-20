using System.Collections.Generic;
using UnityEngine;

public class DynamicManager : Singleton<DynamicManager>
{
    public const int planeCount = 2;
    public float distance;

    private int switcher = 1;
    public Transform[] plane = new Transform[planeCount];
    public ColliderSender[] colliderSenders = new ColliderSender[planeCount];

    public List<Transform> others = new List<Transform>();

    private void OnEnable()
    {
        for (int i = 0; i < plane.Length; i++)
        {
            colliderSenders[i].OnTriggerEnterCall += OnTriggerEnterCall;
        }
    }
    private void OnDisable()
    {
        for (int i = 0; i < plane.Length; i++)
        {
            colliderSenders[i].OnTriggerEnterCall -= OnTriggerEnterCall;
        }
    }

    private void OnTriggerEnterCall(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            switcher = 1 - switcher;

            if (switcher == 0)
            {
                Vector3 warpPos = plane[0].transform.position;
                warpPos.z += distance;
                plane[0].transform.position = warpPos;
            }
            else if (switcher == 1)
            {
                Vector3 warpPos = plane[1].transform.position;
                warpPos.z += distance;
                plane[1].transform.position = warpPos;
            }
        }
    }

    private void Update()
    {
        for (int i = 0; i < plane.Length; i++)
        {
            //Plane 오브젝트 움직임 Lerp -> Translate
            plane[i].transform.Translate(Map.instance.direction * Map.instance.speed * Time.deltaTime);
        }
        for(int i = 0; i < others.Count; i++)
        {
            if (others[i] == null)
            {
                others.Remove(others[i]);
                continue;
            }
            //구독 오브젝트 움직임 Lerp -> Translate
            others[i].transform.Translate(Map.instance.direction * Map.instance.speed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Dynamic Object를 등록하는 메소드
    /// </summary>
    /// <param name="target">등록할 게임오브젝트</param>
    public void RegisterObject(GameObject target)
    {
        others.Add(target.transform);
    }
}
