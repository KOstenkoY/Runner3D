using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private int _itemSpace = 7;                             // space between successive obstacles
    [SerializeField] private int _itemCountMap = 6;                          // count obstacles on one map

    private int _countRoads = 3;

    [SerializeField] private int _coinsCountnItem = 10;
    private float _mapSize = 50;
    [SerializeField] private float _coinsHeight = 0.5f;

    //// types of obstacles
    [SerializeField] private GameObject[] obstacles;

    [SerializeField] private GameObject CoinPrefab;

    private int[] _randomItem = new int[4];
    private int[] _randomRoad;

    enum TrackPosition 
    { 
        Left = -1, 
        Center = 0, 
        Right = 1 
    };

    enum CoinStyle 
    { 
        Ramp,
        Jump,
        Line
    };

    private void Awake()
    {
        _randomRoad = new int[_countRoads];
        for (int i = 0; i < _randomItem.Length; i++)
        {
            if (i < _randomRoad.Length)
            {
                _randomItem[i] = i;
                _randomRoad[i] = i;
            }
            else
            {
                _randomItem[i] = i;
            }
        }

        CreateObstacles();
    }

    private void CreateObstacles()
    {
        Vector3 position = new Vector3(0, 0, this.transform.position.z - 22);

        for (int i = 0; i < _itemCountMap; i++)
        {
            ShuffleArray(_randomItem);
            ShuffleArray(_randomRoad);

            for(int j = 0; j < Random.Range(1, _countRoads); j++)
            {
                if(_randomRoad[j] == 2)
                {
                    position.x = -1 * 3;
                }
                else
                {
                    position.x = _randomRoad[j] * 3;
                }

                GameObject go = Instantiate(
                    obstacles[_randomItem[j]], 
                    obstacles[_randomItem[j]].transform.position + position, 
                    obstacles[_randomItem[j]].transform.rotation
                    );

                go.transform.SetParent(transform);

                position.z += _itemSpace;
            }
        }
    }

    //private void CreateObstacles()
    //{
    //    for(int i = 0; i < _itemSpace; i++)
    //    {
    //        TrackPosition trackPosition = TrackPosition.Center;
    //        CoinStyle coinStyle = CoinStyle.Line;

    //        for(int j = 0; j < Random.Range(2, _countRoads); j++)
    //        {
    //            if(i == 1)
    //            {

    //            }
    //            if (i == 0)
    //            {

    //            }

    //            _randomRoad = ShuffleArray(_randomRoad);
    //            _randomItem = ShuffleArray(_randomItem);
    //        }
    //    }
    //}

    //private void Update()
    //{
    //    if (activeMaps[0].transform.position.z < -_mapSize)
    //    {
    //        RemoveFirstActiveMap();
    //        AddActiveMap();
    //    }
    //}
    ////private void RemoveFirstActiveMap()
    ////{
    ////    activeMaps[0].SetActive(false);
    ////    maps.Add(activeMaps[0]);
    ////    activeMaps.RemoveAt(0);
    ////}

    //////private void AddActiveMap()
    //////{
    //////    int rnd = Random.Range(0, maps.Count);

    //////    GameObject go = maps[rnd];
    //////    go.SetActive(true);

    //////    foreach (Transform child in go.transform)
    //////    {
    //////        child.gameObject.SetActive(true);
    //////    }

    //////    go.transform.position = activeMaps.Count > 0 ?
    //////        activeMaps[activeMaps.Count - 1].transform.position + Vector3.forward * _mapSize :
    //////        new Vector3(0, 0, 10);

    //////    maps.RemoveAt(rnd);
    //////    activeMaps.Add(go);
    //////}

    ////private GameObject MakeMap()
    ////{
    ////    GameObject result = new GameObject();

    ////    result.transform.SetParent(transform);

    ////    MapItem item = new MapItem();

    ////    for (int i = 0; i < _currentItemCountMap; i++)
    ////    {
    ////        item.SetValues(null, TrackPosition.Center, CoinStyle.Line);

    ////        if (i == 2)
    ////        {
    ////            item.SetValues(RampPrefab, TrackPosition.Left, CoinStyle.Ramp);
    ////        }
    ////        else if (i == 3)
    ////        {
    ////            item.SetValues(ObstacleJumpPrefab, TrackPosition.Left, CoinStyle.Jump);
    ////        }
    ////        else if (i == 4)
    ////        {
    ////            item.SetValues(ObstacleSlidePrefab, TrackPosition.Right, CoinStyle.Line);
    ////        }

    ////        Vector3 obstaclePosition = new Vector3((int)item.trackPosition * _distanceBetweenLine, 0, i * _itemSpace);
    ////        CreateCoins(item.coinStyle, obstaclePosition, result);

    ////        if (item.obstacle != null)
    ////        {
    ////            GameObject go = Instantiate(item.obstacle, obstaclePosition, Quaternion.identity);
    ////            go.transform.SetParent(result.transform);
    ////        }
    ////    }
    ////    return result;
    ////}

    private void ShuffleArray(int[] numbers)
    {
        for (int i = 0; i < numbers.Length; i++)
        {
            int tmp = numbers[i];
            int r = Random.Range(i, numbers.Length);
            numbers[i] = numbers[r];
            numbers[r] = tmp;
        }
    }

    ////private void CreateCoins(CoinStyle style, Vector3 position, GameObject parentObject)
    ////{

    ////    Vector3 coinPos = Vector3.zero;
    ////    if (style == CoinStyle.Line)
    ////    {
    ////        for (int i = -_coinsCountnItem / 2; i < _coinsCountnItem / 2; i++)
    ////        {
    ////            coinPos.y = _coinsHeight;
    ////            coinPos.z = i * ((float)_itemSpace / _coinsCountnItem);
    ////            GameObject go = Instantiate(CoinPrefab, coinPos + position, Quaternion.identity);
    ////            go.transform.SetParent(parentObject.transform);
    ////        }
    ////    }
    ////    else if (style == CoinStyle.Jump)
    ////    {
    ////        for (int i = -_coinsCountnItem / 2; i < _coinsCountnItem / 2; i++)
    ////        {
    ////            coinPos.y = Mathf.Min(Mathf.Max(-1 / 2f * Mathf.Pow(i, 2) + 3, _coinsHeight), 3.0f);
    ////            coinPos.z = i * ((float)_itemSpace / _coinsCountnItem);
    ////            GameObject go = Instantiate(CoinPrefab, coinPos + position, Quaternion.identity);
    ////            go.transform.SetParent(parentObject.transform);
    ////        }
    ////    }
    ////}

    ////public void GenerationOptions()
    ////{
    ////    obstacles.Add(new MapItem(ObstacleSlidePrefab, CoinStyle.Line));
    ////    obstacles.Add(new MapItem(ObstacleSlidePrefab, CoinStyle.Line));
    ////    obstacles.Add(new MapItem(ObstacleSlidePrefab, CoinStyle.Line));
    ////}

    //private struct MapItem
    //{
    //    public GameObject obstacle;
    //    public TrackPosition trackPosition;
    //    public CoinStyle coinStyle;

    //    public void SetValues(GameObject obstacle, CoinStyle coinStyle)
    //    {
    //        this.obstacle = obstacle;
    //        this.coinStyle = coinStyle;
    //    }
    //}
}
