using System;
using System.Diagnostics.Tracing;
using UnityEngine.UI;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private Rigidbody _rb;
    public float speed = 5f, finalSpeed = 15f, rotateSpeed = 350f;
    private bool isClicked;
    private float curPointX, curPointY;
    [NonSerialized] public Vector3 FinalPosition;

    private enum Direction
    {
        Right, Left, Top, Bottom, None
    }

    private Direction CarDirectionX = Direction.None;
    private Direction CarDirectionY = Direction.None;

    public Text CountMoves, CountMoney;
    public GameObject StartGameBtn;
    private static int CountCars = 0;

    private AudioSource _auido;
    public AudioClip AudioCrash, AudioStart;
    public ParticleSystem CrashEffect;

    public enum Axis
    {
        Vertical, Horizontal
    }

    public Axis CarAxis;

    void Awake()
    {
        CountCars++;
        _rb = GetComponent<Rigidbody>();
        _auido = GetComponent<AudioSource>();
    }

    void OnMouseDown()
    {
        if (!StartGame.IsGameStarted) return;

        curPointX = Input.mousePosition.x;
        curPointY = Input.mousePosition.y;
    }

    void OnMouseUp()
    {
        if (!StartGame.IsGameStarted) return;

        if (Input.mousePosition.x - curPointX > 0)
            CarDirectionX = Direction.Right;
        else
            CarDirectionX = Direction.Left;

        if (Input.mousePosition.y - curPointY > 0)
            CarDirectionY = Direction.Top;
        else
            CarDirectionY = Direction.Bottom;

        isClicked = true;

        CountMoves.text = Convert.ToString(Convert.ToInt32(CountMoves.text) - 1);

        _auido.Stop();
        _auido.clip = AudioStart;
        _auido.Play();
    }

    void Update()
    {
        if (CountMoves.text == "0" && CountCars > 0 && !isClicked)
        {
            StartGameBtn.GetComponent<StartGame>().LoseGame();
        }

        if (FinalPosition != Vector3.zero)
        {
            Vector3 direction = FinalPosition - transform.position;
            direction.y = 0;

            if (direction.magnitude > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, FinalPosition, finalSpeed * Time.deltaTime);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotateSpeed);
            }
            else
            {
                PlayerPrefs.SetInt("CarCoins", PlayerPrefs.GetInt("CarCoins") + 1);
                CountMoney.text = Convert.ToString(Convert.ToInt32(CountMoney.text) + 1);
                CountCars--;

                if (CountCars == 0)
                {
                    StartGameBtn.GetComponent<StartGame>().WinGame();
                }

                Destroy(gameObject);
            }
        }

    }
    void FixedUpdate()
    {
        if (isClicked && FinalPosition == Vector3.zero)
        {
            Vector3 whichWay = CarAxis == Axis.Horizontal ? Vector3.forward : Vector3.left;

            speed = Math.Abs(speed);
            if (CarDirectionX == Direction.Left && CarAxis == Axis.Horizontal)
                speed *= -1;
            else if (CarDirectionY == Direction.Bottom && CarAxis == Axis.Vertical)
                speed *= -1;

            _rb.MovePosition(_rb.position + whichWay * speed * Time.fixedDeltaTime);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Car") || other.CompareTag("Barrier"))
        {
            Destroy(
                Instantiate(CrashEffect, other.ClosestPoint(transform.position), Quaternion.Euler(new Vector3(270f, 0, 0))), 2f
                );

            if (_auido.clip != AudioCrash && _auido.isPlaying)
            {
                _auido.Stop();
                _auido.clip = AudioCrash;
                _auido.Play();
            }

            if (CarAxis == Axis.Horizontal && isClicked)
            {
                float adding = CarDirectionX == Direction.Left ? 0.5f : -0.5f;
                transform.position = new Vector3(transform.position.x, 0, transform.position.z + adding);
            }

            if (CarAxis == Axis.Vertical && isClicked)
            {
                float adding = CarDirectionY == Direction.Top ? 0.5f : -0.5f;
                transform.position = new Vector3(transform.position.x  + adding, 0, transform.position.z);
            }

        }
        isClicked = false;
    }
}
