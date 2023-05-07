using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public enum State { Alive, Win, Die, End};
    public State state = State.Alive;
    [SerializeField] float moveSpeed = 2.5f;
    private float speed;
    [SerializeField] float jumpHeight = 550.0f;
    private Animator animator;
    public AudioSource jumpAudio;
    public AudioSource walkAudio;
    public AudioSource runAudio;    
    public bool jumpable, moveable = true;
    public bool forceRunning = false;
    private string moveX, moveY, jump, speedUp;

    private void Start()
    {
        animator = GetComponent<Animator>();
        Physics.gravity = new Vector3(0, -35, 0);
        for (int i = 0; i < UIManager.playerCount; i++)
        {
            if (gameObject.tag == "Player" + (i + 1))
            {
                moveX = "Player"+ UIManager.joystickNumbers[i] +"MoveX";
                moveY = "Player"+ UIManager.joystickNumbers[i] + "MoveY";
                jump = "Player"+ UIManager.joystickNumbers[i] + "Jump";
                speedUp = "Player" + UIManager.joystickNumbers[i] + "SpeedUp";
            }
        }
    }
    private void Update()
    {
        if (state != State.Alive)
            return;
        if (jumpable)
            JumpProcess();
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

        /*float x = Input.GetAxis(moveX);
        float y = Input.GetAxis(moveY);
        gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(x, 0, y) * speed);
        if (y > 0 && x == 0)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (y < 0 && x == 0)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else if (x < 0 && y == 0)
        {
            transform.localRotation = Quaternion.Euler(0, -90, 0);
        }
        else if (x > 0 && y == 0)
        {
            transform.localRotation = Quaternion.Euler(0, 90, 0);
        }
        else if (y > 0 && x > 0)
        {
            transform.localRotation = Quaternion.Euler(0, 45, 0);
        }
        else if (y > 0 && x < 0)
        {
            transform.localRotation = Quaternion.Euler(0, -45, 0);
        }
        else if (y < 0 && x > 0)
        {
            transform.localRotation = Quaternion.Euler(0, 135, 0);
        }
        else if (y < 0 && x < 0)
        {
            transform.localRotation = Quaternion.Euler(0, -135, 0);
        }*/
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
