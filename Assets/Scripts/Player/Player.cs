using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveController))]
public class Player : MonoBehaviour
{
    //InputController inputController;
    [System.Serializable]
    public class MouseInput
    {
        public Vector2 Damping;
        public Vector2 Sensitivity;
        public bool LockMouse;
    }

    [SerializeField]float runSpeed;
    [SerializeField]float walkSpeed;
    [SerializeField]float crouchSpeed;
    [SerializeField]float sprintSpeed;

    [SerializeField]MouseInput MouseControl;

    private MoveController m_MoveController;
    public MoveController MoveController
    {
        get
        {
            if (m_MoveController == null)
            {
                m_MoveController = GetComponent<MoveController>();
            }
            return m_MoveController;
        }
    }

    private Crosshair m_Crosshair;
    private Crosshair Crosshair
    {
        get
        {
            if(m_Crosshair == null){
                m_Crosshair = GetComponentInChildren<Crosshair>();
            }
            return m_Crosshair;
        }
    }

    InputController playerInput;
    Vector2 mouseInput;

    // Start is called before the first frame update
    void Awake()
    {
        //inputController = GameManager.Instance.InputController;
        playerInput = GameManager.Instance.InputController;
        GameManager.Instance.LocalPlayer = this;

        if (MouseControl.LockMouse)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        LookAround();
    }


    void Move()
    {
        float moveSpeed = runSpeed;

        if (playerInput.IsWalking)
        {
            moveSpeed = walkSpeed;
        }
        if (playerInput.IsSprinting)
        {
            moveSpeed = sprintSpeed;
        }
        if (playerInput.IsCrouched)
        {
            moveSpeed = crouchSpeed;
        }


        Vector2 direction = new Vector2(playerInput.Vertical * moveSpeed, playerInput.Horizontal * moveSpeed);
        MoveController.Move(direction);
    }

    void LookAround()
    {
        mouseInput.x = Mathf.Lerp(mouseInput.x, playerInput.MouseInput.x, 1f / MouseControl.Damping.x);
        mouseInput.y = Mathf.Lerp(mouseInput.y, playerInput.MouseInput.y, 1f / MouseControl.Damping.y);

        transform.Rotate(Vector3.up * mouseInput.x * MouseControl.Sensitivity.x);

        Crosshair.LookHeight(mouseInput.y * MouseControl.Sensitivity.y);
    }

}
