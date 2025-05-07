using System.Collections.Generic;
using Dreamteck.Splines;
using UnityEngine;

public class CustomerController : MonoBehaviour
{
    [SerializeField] private SplineFollower _splineFollower;
    public SplineFollower splineFollower { get => _splineFollower; }


    private void Awake()
    {
        splineFollower.onEndReached += OnReachedEnd;
    }

    private void OnReachedEnd(double obj)
    {
        CustomerManager.OnEndReached?.Invoke();
    }

}
