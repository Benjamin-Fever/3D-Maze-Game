using System;

public static class Util{
    public static float deg2rad(float degrees){
        return (float)((Math.PI / 180) * degrees);
    }

    public static float rad2deg(float raddians){
        return (float)(raddians * 180 / Math.PI);
    }
}