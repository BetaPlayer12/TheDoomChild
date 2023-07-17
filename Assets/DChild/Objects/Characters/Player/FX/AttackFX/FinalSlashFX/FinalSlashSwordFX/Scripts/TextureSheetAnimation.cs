using UnityEngine;

public class TextureSheetAnimation : MonoBehaviour
{
    public int columns = 5;
    public int rows = 5;
    public float frameRate = 30;

    private int currentFrame = 0;
    private SpriteRenderer spriteRenderer;
    private Sprite[] sprites;
    private int spriteWidth;
    private int spriteHeight;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        sprites = Resources.LoadAll<Sprite>(spriteRenderer.sprite.texture.name);
        spriteWidth = Mathf.RoundToInt(sprites[0].rect.width);
        spriteHeight = Mathf.RoundToInt(sprites[0].rect.height);
        InvokeRepeating("NextFrame", 0, 1 / frameRate);
    }

    void NextFrame()
    {
        currentFrame = (currentFrame + 1) % (rows * columns);
        int column = currentFrame % columns;
        int row = currentFrame / columns;
        int index = row * columns + column;
        spriteRenderer.sprite = Sprite.Create(
            sprites[index].texture,
            new Rect(sprites[index].rect.x, sprites[index].rect.y, spriteWidth, spriteHeight),
            new Vector2(0.5f, 0.5f),
            spriteRenderer.sprite.pixelsPerUnit
        );
    }
}
