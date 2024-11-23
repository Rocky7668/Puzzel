using UnityEngine;
using System;
using UnityEngine.UI;

public class SpriteCutter : MonoBehaviour
{

    public static SpriteCutter instance;
    public Texture2D spriteToCut; // The sprite you want to cut
    public int columns = 6;    // Number of columns
    public int rows = 4;       // Number of rows
    public float spacing = 0.1f; // Spacing between sprites
    public bool isImage = false;


    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    void Start()
    {
        // GenerateAndDisplaySprites(columns, rows);
    }

    Puzzle pzl = new Puzzle();
    // Method that generates and positions all the sprites on the screen
    public void GenerateAndDisplaySprites(int columns, int rows)
    {
        Texture2D texture = spriteToCut;
        int cellWidth = texture.width / columns;  // Width of each cell
        int cellHeight = texture.height / rows;   // Height of each cell

        int counter = 1; // For naming each cut section with numbers (1, 2, 3...)
        // Loop through each row and column to generate and display sub-sprites
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                // Calculate the pixel coordinates for each cell (from top-left to bottom-right)
                Rect cellRect = new Rect(x * cellWidth, (texture.height - (y + 1) * cellHeight), cellWidth, cellHeight);

                // Extract pixels from the texture
                Color[] pixels = texture.GetPixels((int)cellRect.x, (int)cellRect.y, (int)cellRect.width, (int)cellRect.height);

                // Create a new texture for the cell
                Texture2D cellTexture = new Texture2D(cellWidth, cellHeight);
                cellTexture.SetPixels(pixels);
                cellTexture.Apply(); // Apply the changes to the texture

                // Create a new sprite from the texture
                Sprite newSprite = Sprite.Create(cellTexture, new Rect(0, 0, cellWidth, cellHeight), new Vector2(0.5f, 0.5f));
                pzl.sprites.Add(newSprite);
                // Create a new GameObject to display the sprite
                //GameObject newObject = new GameObject("Sprite_" + counter); // Name as Sprite_1, Sprite_2, etc.

                // Position the sprite in the same order as they appear in the original image
                //newObject.transform.position = new Vector3(x * (cellWidth + spacing) / 100f, -y * (cellHeight + spacing) / 100f, 0);


                //SpriteRenderer renderer = newObject.AddComponent<SpriteRenderer>();
                //renderer.sprite = newSprite;
                //renderer.sprite.name = counter.ToString();

                // Increment the counter for the next sprite name
                counter++;
            }
        }
        isImage = true;


    }

    public void SetImageinPuzzel()
    {
        if (isImage)
        { 
            Debug.Log(pzl + "pzl == nulll " + pzl == null);
            puzzleManager.instance.puzzles.Add(pzl);
            uimanager.instance.imgIdx = puzzleManager.instance.puzzles.Count - 1;
            isImage = false;
        }
    }
}
