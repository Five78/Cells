using System;

[Serializable]
public class StickCoordinates
{
    public bool isVertical;
    public int position;

    public StickCoordinates(bool isVertical, int position)
    {
        this.isVertical = isVertical;
        this.position = position;
    }
}