using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMap : MonoBehaviour
{
    [SerializeField]
    private Transform _positionStart;
    [SerializeField]
    private int _boardSize = 30;

    //тут будет лист Cells с игровым полем

    private void Awake()
    {
        for (int i = _boardSize / 2 - _boardSize; i < _boardSize - _boardSize / 2; i++)
        {
            for (int j = _boardSize / 2 - _boardSize; j < _boardSize - _boardSize / 2; j++)
            {
                var point = new GameObject("point");
                point.transform.SetParent(transform);
                point.transform.position = new Vector3(i, 0, j);
            }
        }
    }
}
