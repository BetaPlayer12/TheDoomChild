using UnityEngine;

public interface IFoliage
{

    bool CanCollideAgain { get; set; }
    bool Reverse { get; set; }
    bool IsReturning { get; set; }

    Vector2 Location{ get; }

    void Initialized();
    void SetToDefault();
    void GetPropertyBlock();
    void ReverseVertices();
    void ReturnToDefault();
    void ApplyChanges();
}


