using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpecial
{
    public void Fire(Transform position, Quaternion rotation);

    public bool IsQueued(int createScaleLength, int currentNote);
}
