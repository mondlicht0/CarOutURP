using UnityEngine;

public interface ICarController
{
    public void Move(Vector2 direction, float accelSpeed, float wheelBase, float turnRadius, float rearTrack);
}
