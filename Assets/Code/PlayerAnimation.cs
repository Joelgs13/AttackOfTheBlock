using System.Collections;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [Header("Animaciones del jugador")]
    [SerializeField] private Sprite[] idleSprites;
    [SerializeField] private Sprite[] hurtSprites;
    [SerializeField] private Sprite[] deadSprites;

    [SerializeField] private float frameRate = 0.1f; // tiempo entre frames
    private Coroutine currentAnimation;
    private bool isDead = false;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        PlayIdle();
    }

    public void PlayIdle()
    {
        if (isDead) return;

        if (currentAnimation != null)
            StopCoroutine(currentAnimation);

        currentAnimation = StartCoroutine(PlayAnimationLoop(idleSprites));
    }

    public void PlayHurt()
    {
        if (isDead) return;

        if (currentAnimation != null)
            StopCoroutine(currentAnimation);

        currentAnimation = StartCoroutine(PlayHurtAnimation());
    }

    public void PlayDead()
    {
        if (isDead) return;

        isDead = true;
        if (currentAnimation != null)
            StopCoroutine(currentAnimation);

        currentAnimation = StartCoroutine(PlayAnimationOnce(deadSprites, null));
    }

    private IEnumerator PlayAnimationLoop(Sprite[] frames)
    {
        int index = 0;
        while (true)
        {
            spriteRenderer.sprite = frames[index];
            index = (index + 1) % frames.Length;
            yield return new WaitForSeconds(frameRate);
        }
    }

    private IEnumerator PlayAnimationOnce(Sprite[] frames, System.Action onFinish)
    {
        for (int i = 0; i < frames.Length; i++)
        {
            spriteRenderer.sprite = frames[i];
            yield return new WaitForSeconds(frameRate);
        }
        onFinish?.Invoke();
    }

    private IEnumerator PlayHurtAnimation()
    {
        // reproducir Hurt tres veces
        for (int repeat = 0; repeat < 3; repeat++)
        {
            for (int i = 0; i < hurtSprites.Length; i++)
            {
                spriteRenderer.sprite = hurtSprites[i];
                yield return new WaitForSeconds(frameRate);
            }
        }

        PlayIdle();
    }
}
