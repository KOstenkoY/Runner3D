using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _distanceBetweenLine;
    [SerializeField] private int _countDeath;

    private RoadLine _roadLine;

    private float _currentLine;

    private int _middleLineIndex = 0;

    public float CurrentLine => _currentLine;

    public static Action Notify;

    private void Awake()
    {
        _roadLine = RoadLine.Center;
        _countDeath = 1;
    }

    private void OnEnable()
    {
        SwipeDetection.OnSwipeLeft += ChangeLeftLine;
        SwipeDetection.OnSwipeRight += ChangeRightLine;
    }

    private void OnDisable()
    {
        SwipeDetection.OnSwipeLeft -= ChangeLeftLine;
        SwipeDetection.OnSwipeRight -= ChangeRightLine;
    }

    private void ChangeLeftLine()
    {
        if(_roadLine == RoadLine.Center)
        {
            _roadLine = RoadLine.Left;
            _currentLine -= _distanceBetweenLine;
        }
        else if(_roadLine == RoadLine.Right)
        {
            _roadLine = RoadLine.Center;
            _currentLine = _middleLineIndex;
        }
    }

    private void ChangeRightLine()
    {
        if(_roadLine == RoadLine.Center)
        {
            _roadLine = RoadLine.Right;
            _currentLine = _distanceBetweenLine;
        }
        else if(_roadLine == RoadLine.Left)
        {
            _roadLine = RoadLine.Center;
            _currentLine = _middleLineIndex;
        }
    }

    public enum RoadLine
    {
        Left,
        Center,
        Right
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zone"))
        {
            _countDeath++;
            Notify?.Invoke();
        }
        else
        {
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die()
    {
        yield return new WaitForSeconds(1.5f);

        DestroyImmediate(gameObject);
        GameManager.Instance.SetPaused(true, _countDeath);
    }
}
