public class DoorSpawnParameters
{
    public float X;
    public float Y;
    public bool IsHorizontal;
    public bool isBasic = true;

    public DoorSpawnParameters(float X, float Y, bool isHorizontal, bool isBasic = true)
    {
        this.X = X;
        this.Y = Y;
        IsHorizontal = isHorizontal;
        this.isBasic = isBasic;
    }
}