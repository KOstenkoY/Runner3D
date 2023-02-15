using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneraion : MonoBehaviour
{
    [SerializeField] private int _itemSpace = 7;                            // space between successive obstacles
    [SerializeField] private int _itemCountMap = 7;                         // count obstacles on one map

    [SerializeField] private float _distanceBetweenLine = 3f;               // must be equal how in Player Controller

    [SerializeField] private int _coinsCountnItem = 10;
    [SerializeField] private float _mapSize = 50;
    [SerializeField] private float _coinsHeight = 0.5f;

    // types of obstacles
    [SerializeField] private GameObject ObstacleSlidePrefab;
    [SerializeField] private GameObject ObstacleJumpPrefab;
    [SerializeField] private GameObject ObstacleDodgePrefab;
    [SerializeField] private GameObject RampPrefab;

    [SerializeField] private GameObject CoinPrefab;

    private int[] indexes = new int[4];


    public List<GameObject> maps = new List<GameObject>();
    public List<GameObject> activeMaps = new List<GameObject>();

    enum TrackPosition
    {
        Left = -1,
        Center = 0,
        Right = 1
    };

    enum CoinStyle
    {
        Line,
        Jump,
        Ramp
    };

    private void Awake()
    {
        maps.Add(MakeMap());
        maps.Add(MakeMap());
        maps.Add(MakeMap());
        maps.Add(MakeMap());
        maps.Add(MakeMap());

        foreach (GameObject map in maps)
        {
            map.SetActive(false);
        }

        for (int i = 0; i < indexes.Length; i++)
        {
            indexes[i] = i;
        }

        indexes = ShuffleArray(indexes);
    }

    private void Update()
    {
        if (activeMaps[0].transform.position.z < -_mapSize)
        {
            RemoveFirstActiveMap();
            AddActiveMap();
        }
    }
    private void RemoveFirstActiveMap()
    {
        activeMaps[0].SetActive(false);
        maps.Add(activeMaps[0]);
        activeMaps.RemoveAt(0);
    }

    private void AddActiveMap()
    {
        int rnd = Random.Range(0, maps.Count);

        GameObject go = maps[rnd];
        go.SetActive(true);

        foreach (Transform child in go.transform)
        {
            child.gameObject.SetActive(true);
        }

        go.transform.position = activeMaps.Count > 0 ?
            activeMaps[activeMaps.Count - 1].transform.position + Vector3.forward * _mapSize :
            new Vector3(0, 0, 10);

        maps.RemoveAt(rnd);
        activeMaps.Add(go);
    }

    private GameObject MakeMap()
    {
        GameObject result = new GameObject();

        result.transform.SetParent(transform);

        MapItem item = new MapItem();

        for (int i = 0; i < _itemCountMap; i++)
        {
            if (i == 0)
            {
                item.SetValues(null, TrackPosition.Center, CoinStyle.Line);
            }
            if (i == 2)
            {
                item.SetValues(RampPrefab, TrackPosition.Left, CoinStyle.Ramp);
            }
            else if (i == 3)
            {
                item.SetValues(ObstacleJumpPrefab, TrackPosition.Left, CoinStyle.Jump);
            }
            else if (i == 4)
            {
                item.SetValues(ObstacleSlidePrefab, TrackPosition.Right, CoinStyle.Line);
            }

            Vector3 obstaclePosition = new Vector3((int)item.trackPosition * _distanceBetweenLine, 0, i * _itemSpace);
            CreateCoins(item.coinStyle, obstaclePosition, result);

            if (item.obstacle != null)
            {
                GameObject go = Instantiate(item.obstacle, obstaclePosition, Quaternion.identity);
                go.transform.SetParent(result.transform);
            }
        }
        return result;
    }

    private int[] ShuffleArray(int[] numbers)
    {
        int[] newArray = numbers.Clone() as int[];

        for (int i = 0; i < newArray.Length; i++)
        {
            int tmp = newArray[i];
            int r = Random.Range(i, newArray.Length);
            newArray[i] = newArray[r];
            newArray[r] = tmp;
        }

        return newArray;
    }

    private void CreateCoins(CoinStyle style, Vector3 position, GameObject parentObject)
    {

        Vector3 coinPos = Vector3.zero;
        if (style == CoinStyle.Line)
        {
            for (int i = -_coinsCountnItem / 2; i < _coinsCountnItem / 2; i++)
            {
                coinPos.y = _coinsHeight;
                coinPos.z = i * ((float)_itemSpace / _coinsCountnItem);
                GameObject go = Instantiate(CoinPrefab, coinPos + position, Quaternion.identity);
                go.transform.SetParent(parentObject.transform);
            }
        }
        else if (style == CoinStyle.Jump)
        {
            for (int i = -_coinsCountnItem / 2; i < _coinsCountnItem / 2; i++)
            {
                coinPos.y = Mathf.Min(Mathf.Max(-1 / 2f * Mathf.Pow(i, 2) + 3, _coinsHeight), 3.0f);
                coinPos.z = i * ((float)_itemSpace / _coinsCountnItem);
                GameObject go = Instantiate(CoinPrefab, coinPos + position, Quaternion.identity);
                go.transform.SetParent(parentObject.transform);
            }
        }
    }

    private struct MapItem
    {
        public GameObject obstacle;
        public TrackPosition trackPosition;
        public CoinStyle coinStyle;

        public void SetValues(GameObject obstacle, TrackPosition trackPosition, CoinStyle coinStyle)
        {
            this.obstacle = obstacle;
            this.trackPosition = trackPosition;
            this.coinStyle = coinStyle;
        }
    }
}
