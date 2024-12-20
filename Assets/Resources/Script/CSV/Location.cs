using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Location
{
    public string name;
    public string[] neighbours;

    public Location(string name, string[] neighbours)
    {
        this.name = name;
        this.neighbours = neighbours;
    }

}
