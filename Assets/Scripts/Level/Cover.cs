using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Cover : MonoBehaviour
{
    [SerializeField] List<Transform> PointTransforms;

    public IEnumerable<Transform> PointTransformsEnum => PointTransforms;
}
