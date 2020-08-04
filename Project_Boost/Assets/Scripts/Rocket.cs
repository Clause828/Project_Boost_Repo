using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidbody;
    AudioSource audioSource;
    [SerializeField] float rotationThrust = 100f;
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip death;
    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem deathParticles;

    private int level;

    enum State{Alive, Dying, Transcending };
    State state = State.Alive;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive)
        {
            RespondToRotate();
            RespondToThrustInput();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive) return; //ignore collisions when dead

        switch(collision.gameObject.tag)
        {
            case "Friendly":
                print("Safe");
                break;
            case "Fuel":
                print("Fuel");
                break;
            case "finish":
                state = State.Transcending;
                audioSource.Stop();
                audioSource.PlayOneShot(success);
                successParticles.Play();
                Invoke("LoadNextScene" ,1f);
                break;
            default:
                state = State.Dying;
                audioSource.Stop(); //stops the space
                audioSource.PlayOneShot(death);
                deathParticles.Play();
                Invoke("LoadFirstScene" ,1f);
                break;
        }
            
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(++level);
    }
    private void LoadFirstScene()
    {
        level = 0;
        SceneManager.LoadScene(level);
    }

    private void RespondToRotate()
    {

        rigidbody.freezeRotation = true; //take manual control of rotation
        float rotationThisFrame = rotationThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.back * rotationThisFrame);
        }
        rigidbody.freezeRotation = false; //resume physics control of rotation
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            audioSource.Stop();
            mainEngineParticles.Stop();
        }
    }
    private void ApplyThrust()
    {
        float Thrust = rcsThrust * Time.deltaTime;
        rigidbody.AddRelativeForce(Vector3.up * Thrust);
        if (!audioSource.isPlaying)
            audioSource.PlayOneShot(mainEngine);
        mainEngineParticles.Play();
    }
}
