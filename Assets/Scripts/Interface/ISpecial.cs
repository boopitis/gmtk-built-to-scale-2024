using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpecial
{
    public void Fire();

    public bool IsQueued(int createScaleLength, int currentNote);
}
