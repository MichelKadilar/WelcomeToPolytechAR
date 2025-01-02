using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Location
{
    public string name;
    public string[] neighbors;

    public Location(string name, string[] neighbors)
    {
        this.name = name;
        this.neighbors = neighbors;
    }

}
