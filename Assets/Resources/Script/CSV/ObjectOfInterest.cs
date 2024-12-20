using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectOfInterest
{
    public string name, sourceLoc, sellTime;

    public ObjectOfInterest(string name, string sourceLoc, string sellTime)
    {
        this.name = name;
        this.sourceLoc = sourceLoc;
        this.sellTime = sellTime;
    }

}
