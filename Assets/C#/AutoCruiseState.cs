using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoCruiseState  {
    public static int close = 0;
    public static int moveForwardByFristGear = 1;
    public static int moveForwardBySecondGear = 2;
    public static int moveForwardByThirdGear = 3;
    public static int backUpByFristGear = -1;
    public static int backUpBySecondGear = -2;
    public int state = close;
}
