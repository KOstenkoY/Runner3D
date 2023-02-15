using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _roadPrefab;                
    [SerializeField] private GameObject _notifyZone;                  // notify road generator when we can Despawn our map and spawn forward

    [SerializeField] private int _roadCount;                          // how maps will create
    [SerializeField] private float _roadLength;                       

    private int[] _helperArray;
    private int _helperIndex = 0;                                      // must be zero

    private List<GameObject> roads = new List<GameObject>();
    //private List<GameObject> zones = new List<GameObject>();

    private Vector3 _lastPosition;

    private void OnEnable()
    {
        PlayerController.Notify += DespawnMap;
    }

    private void OnDisable()
    {
        PlayerController.Notify -= DespawnMap;
    }

    private void Awake()
    {
        ResetLevel();
        StartLevel();

        _helperArray = new int[_roadCount];

        for(int i = 0; i < _helperArray.Length; i++)
        {
            _helperArray[i] = i;
        }
    }

    public void ResetLevel()
    {
        if (roads.Count > 0)
        {
            int roadsCount = roads.Count;

            while (roadsCount > 0)
            {
                Destroy(roads[roadsCount - 1]);
                roads.RemoveAt(roadsCount - 1);
                roadsCount--;
            }
            _lastPosition = Vector3.zero;
            _helperIndex = 0;
        }

    }

    private void CreateNextRoad(int indexRoad)
    {
        Vector3 position = Vector3.zero;

        if (roads.Count > 0)
        {
            position = roads[roads.Count - 1].transform.position + new Vector3(0, 0, _roadLength);
        }
        else
        {
            position = new Vector3(0, 0, _roadLength);
        }

        GameObject go = PoolManager.Instance.Spawn(_roadPrefab, indexRoad, position, Quaternion.identity);

        go.transform.SetParent(transform);

        roads.Add(go);
    }

    //private void CreateNextNotifyZone(int indexZone)
    //{
    //    Vector3 position = Vector3.zero;

    //    if (zones.Count > 0)
    //    {
    //        position = zones[zones.Count - 1].transform.position + new Vector3(0, 0, _roadLength);
    //    }
    //    else
    //    {
    //        position = new Vector3(0, 0, _roadLength * 2);
    //    }

    //    GameObject go = Instantiate(_notifyZone, position, Quaternion.identity);

    //    go.transform.SetParent(transform);

    //    zones.Add(go);
    //}

    public void StartLevel()
    {
        for (int i = 0; i < _roadCount; i++)
        {
            CreateNextRoad(i);                  
        }
    }

    private void DespawnMap()
    {
        PoolManager.Instance.Despawn(roads[_helperIndex], _helperIndex);

        SpawnMap(_helperIndex);

        if (_helperIndex != _roadCount - 1)
        {
            _helperIndex++;
        }
        else
        {
            _helperIndex = 0;
        }
    }

    private void SpawnMap(int index)
    {
        if(index != 0)
        {
            _lastPosition = roads[roads.Count - 1].transform.position + new Vector3(0, 0, _roadLength * index);
        }
        else
        {
            _lastPosition = roads[roads.Count - 1].transform.position + new Vector3(0, 0, _roadLength);
        }

        PoolManager.Instance.Spawn(
            roads[_helperIndex],
            _helperIndex,
            _lastPosition,
            Quaternion.identity
       );
    }
}