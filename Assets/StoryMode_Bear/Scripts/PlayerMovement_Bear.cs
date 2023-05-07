using UnityEngine;

public class PlayerMovement_Bear : MonoBehaviour
{
    public enum State { Alive, Win, Die, End};
    public State state = State.Alive;
    [SerializeField] float moveSpeed = 2.5f;
    private float speed;
    [SerializeField] float jumpHeight = 550.0f;
    private Animator animator;
    public float attackCD = 1.0f;
    private float attackCounter = 0.0f;
    public AudioSource jumpAudio;
    public AudioSource walkAudio;
    public AudioSource runAudio;
    public AudioSource attackAudio;
    public bool jumpable, moveable = true;
    public bool forceRunning = false;
    private string moveX, moveY, jump, speedUp;

    private void Start()
    {
        animator = GetComponent<Animator>();
        Physics.gravity = new Vector3(0, -35, 0);
        moveX = "Player" + 1 + "MoveX";
        moveY = "Player" + 1 + "MoveY";
        jump = "Player" + 1 + "Jump";
        speedUp = "Player" + 1 + "SpeedUp";
    }
    private void Update()
    {
        if (state != State.Alive)
            return;
        if (jumpable)
            JumpProcess();
        AttackProcess();
        attackCounter += Time.deltaTime;
    }

    void FixedUpdate()  
    {
        if (state != State.Alive)
            return;
        if (moveable)
        {
            MoveProcess();
            InputProcess();
        }
    }

    void InputProcess()
    {
        //切換走路與奔跑的速度
        if(animator.GetBool("running") || forceRunning)
            speed = moveSpeed * 1.4f;
        else
            speed = moveSpeed;

        //按鍵效果
        float x = Input.GetAxis(moveX);
        float y = Input.GetAxis(moveY);

        if (y > 0 && x < 0)
        {
            transform.localRotation = Quaternion.Euler(0, -45, 0);
        }
        else if (y > 0 && x > 0)
        {
            transform.localRotation = Quaternion.Euler(0, 45, 0);
        }
        else if (y < 0 && x < 0)
        {
            transform.localRotation = Quaternion.Euler(0, -135, 0);
        }
        else if (y < 0 && x > 0)
        {
            transform.localRotation = Quaternion.Euler(0, 135, 0);
        }
        else if (y > 0)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (y < 0)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else if (x < 0)
        {
            transform.localRotation = Quaternion.Euler(0, -90, 0);
        }
        else if (x > 0)
        {
            transform.localRotation = Quaternion.Euler(0, 90, 0);
        }
        transform.Translate(new Vector3(0, 0, y) * speed * Time.deltaTime, Space.World);
        transform.Translate(new Vector3(x, 0, 0) * speed * Time.deltaTime, Space.World);
    }

    private void MoveProcess()
    {
        //播放奔跑的音效
        if (animator.GetBool("isGrounded"))
        {
            if (animator.GetBool("running"))
            {
                walkAudio.Stop();
                if (!runAudio.isPlaying)
                {
                    runAudio.Play();
                }
            }
            else if (animator.GetBool("walking"))
            {
                runAudio.Stop();
                if (!walkAudio.isPlaying)
                {
                    walkAudio.Play();
                }
            }
            else
            {
                runAudio.Stop();
                walkAudio.Stop();
            }
        }
        else
        {
            runAudio.Stop();
            walkAudio.Stop();
        }
        bool walking = Input.GetAxis(moveX) != 0 || Input.GetAxis(moveY) != 0;
        bool running = (Input.GetButton(speedUp) || forceRunning) && walking;
        animator.SetBool("walking", walking);
        animator.SetBool("running", running);
    }

    private void JumpProcess()
    {
        if(animator.GetBool("isGrounded"))
        {
            if (Input.GetButtonDown(jump))
            {
                jumpAudio.Play();
                GetComponent<Rigidbody>().AddForce(Vector3.up * jumpHeight);
            }
        }
    }

    private void AttackProcess()
    {
        if(Input.GetButtonDown("Player1Attack") && attackCounter > attackCD)
        {
            attackAudio.Play();
            GetComponent<Animator>().SetTrigger("attack");
            attackCounter = 0;
        }
    }

    public void Die()
    {
        runAudio.Stop();
        walkAudio.Stop();
        animator.SetTrigger("die");
        state = State.Die;
        Destroy(this.gameObject, 2f);
    }

    public void Win()
    {
        runAudio.Stop();
        walkAudio.Stop();
        animator.SetTrigger("win");
        state = State.Win;        
    }

    public void End()
    {
        runAudio.Stop();
        walkAudio.Stop();
        state = State.End;
    }
}
