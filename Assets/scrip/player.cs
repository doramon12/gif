using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    [SerializeField]

    private CharacterController characterController;
    private float horizontal, vertical;

    [SerializeField]

    private float speed = 3f;

    [SerializeField]
    private Vector3 movement; //huong di chuyen

    [SerializeField]
    private Animator animator;

    //kiem tra xem thu player co thuc hien hanh donh danh hay khong
    private bool isAttack;

    [SerializeField]
    private AudioSource audioSource; // Nguồn âm thanh

    [SerializeField]
    private AudioClip attackSound; // Âm thanh khi tấn công




    //trang thai cau nhan vat

    public enum CharacterState
    {
        Normal, Attack
    }

    //trang thai hien tai cau player
    public CharacterState currentState;

    public dameZone dameZone;

    // Start is called before the first frame update
    void Start()
    {
        if (characterController == null)
        {
            characterController = GetComponent<CharacterController>();
        }
    }

    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if (!isAttack)
        {
            isAttack = Input.GetMouseButtonDown(0);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (currentState)
        {
            case CharacterState.Normal:
                Caculate();
                break;
            case CharacterState.Attack:
                break;
        }

        Caculate();
        characterController.Move(movement);
    }


    //tinh toan di chuyen nhan vat

    void Caculate()
    {

        if (isAttack)
        {
            ChangeState(CharacterState.Attack);
            animator.SetFloat("run", 0);

            return;
        }

        movement.Set(horizontal, 0, vertical);
        movement.Normalize();

        movement = Quaternion.Euler(0, -45, 0) * movement;
        movement *= speed * Time.deltaTime;

        //animation run
        animator.SetFloat("run", movement.magnitude);

        if (movement != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(movement);
        }
    }

    //ham thay doi tran thai hien tai cua player
    private void ChangeState(CharacterState newState)
    {
        //clear cache
        isAttack = false;

        //thoat khoi state hien tai
        switch (currentState)
        {
            case CharacterState.Normal:
                break;
            case CharacterState.Attack:
                break;
        }

        //chuyen qua state moi
        switch(newState)
        {
            case CharacterState.Normal:
                //...
                break;
            case CharacterState.Attack:
                animator.SetTrigger("attack");
                // Phát âm thanh khi tấn công
                if (audioSource != null && attackSound != null)
                {
                    audioSource.PlayOneShot(attackSound);
                }
                    break;
        }

        currentState = newState;

    }

    private void OnDisable()
    {
        horizontal = 0;
        vertical = 0;
        isAttack = false;
    }

    public void EndAttack()
    {
        ChangeState(CharacterState.Normal);
    }

    public void beginDame()
    {
        dameZone.beginDame();
    }

    public void endDame()
    {
        dameZone.endDame();
    }
}
