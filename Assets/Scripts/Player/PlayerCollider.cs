using UnityEngine;

public class PlayerCollider : IResettable
{
    CharacterController characterController;
    public float defaultHeight { get; private set; }
    public Vector3 defaultCenter { get; private set; }
    public PlayerCollider(CharacterController characterController)
    {
        this.characterController = characterController;
        defaultHeight = this.characterController.height;
        defaultCenter = this.characterController.center;
    }
    public void ChangeColliderHeight(float newHeight)
    {
        characterController.height = newHeight;
    }
    public void ChangeColliderCenter(Vector3 newCenter)
    {
        characterController.center = newCenter;
    }

    public void ResetToDefault()
    {
        characterController.height = defaultHeight;
        characterController.center = defaultCenter;
    }

    public bool isGrounded => characterController.isGrounded;
}
