using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseManager : MonoBehaviour
{
    public GameObject HousePrefab;
    public House TargetHouse => _houses.Count > _targetHouseIndex ? _houses[_targetHouseIndex] : null;
    public House NextHouse => _houses.Count > _targetHouseIndex + 1 ? _houses[_targetHouseIndex + 1] : null;

    private const int _houseCount = 10;
    private const int _targetHouseIndexOffset = 2;
    private readonly Vector3 _housePlacementHeightGap = new Vector3(0, -3, 0);
    private int _targetHouseIndex = 0;
    private List<House> _houses = new List<House>();

    public void MoveToNextTarget()
    {
        if (_targetHouseIndex == _targetHouseIndexOffset)
        {
            Destroy(_houses[0].gameObject);
            _houses.RemoveAt(0);
            AppendHouse();
        }
        else
        {
            _targetHouseIndex++;
        }
    }
    
    private void Start()
    {
        InitializeHouses();
    }

    private void Update()
    {
        
    }

    private void InitializeHouses()
    {
        _houses.Add(Instantiate(HousePrefab, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<House>());
        _targetHouseIndex = 0;

        for (var i = 1; i < _houseCount; i++)
        {
            AppendHouse();
        }
    }

    private void AppendHouse()
    {
        var isPlacingOnLeft = Random.Range(0, 2) == 0;
        var lastPlacedHouse = _houses[_houses.Count - 1];
        var lastPlacedHouseEdgePosition = isPlacingOnLeft ? lastPlacedHouse.LeftEdgePosition : lastPlacedHouse.RightEdgePosition;
        _houses.Add(Instantiate(HousePrefab, lastPlacedHouseEdgePosition + _housePlacementHeightGap, Quaternion.identity).GetComponent<House>());
    }
}
