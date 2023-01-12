using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private GroundDetector _groundDetector;
    private SoundManager _soundManager;
    private bool _isFacingRight = true;

    public bool isFacingRight;

    [Header("Movement Configuration")]
    [SerializeField] private float _speed = 2f;
    [SerializeField] private float _jumpForce = 6f;
    [SerializeField] private ParticleSystem _dustParticle;

    private static class AnimatorPlayerController { public static class Params { public const string Speed = nameof(Speed); } }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _groundDetector = GetComponent<GroundDetector>();
        _soundManager = ServiceLocator.Get<SoundManager>();
    }

    private void Update()
    {
        var horizontalInput = Input.GetAxisRaw("Horizontal");
        var jumpInput = Input.GetButton("Jump");
        var jumpInputReleased = Input.GetButtonUp("Jump");
        var isGrounded = _groundDetector.IsGrounded;
        const string groundedState = "isGrounded";

        Move(horizontalInput, _speed);
        if (jumpInput && isGrounded)
        {
            _soundManager.Play(SoundManager.Sounds.Jump);
            Jump(_jumpForce, groundedState, isGrounded);
        }

        if (jumpInputReleased && _rigidbody.velocity.y > 0)
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0);

        if (horizontalInput > 0 && !_isFacingRight)
        {
            FlipSides();
        }
        else if (horizontalInput < 0 && _isFacingRight)
        {
            FlipSides();
        }
    }

    private void FlipSides()
    {
        var rotationAngles = 180;

        _isFacingRight = !_isFacingRight;
        transform.Rotate(0, rotationAngles, 0);
    }

    private void Move(float horizontalMovement, float speed)
    {
        _animator.SetFloat(AnimatorPlayerController.Params.Speed, Mathf.Abs(horizontalMovement));
        _rigidbody.velocity = new Vector2(horizontalMovement * speed, _rigidbody.velocity.y);
    }

    private void Jump(float jumpForce, string groundedState, bool isGrounded)
    {
        _animator.SetTrigger("Jump");
        PlayDust(_dustParticle);
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, jumpForce);
        _animator.SetBool(groundedState, isGrounded);
    }

    private void PlayDust(ParticleSystem jumpParticles)
    {
        jumpParticles.Play();
    }
}
