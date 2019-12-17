public class DoorSpawnParameters
{
    public float X;
    public float Y;
    public bool IsHorizontal;

    public DoorSpawnParameters(float X, float Y, bool isHorizontal)
    {
        this.X = X;
        this.Y = Y;
        IsHorizontal = isHorizontal;
    }
}