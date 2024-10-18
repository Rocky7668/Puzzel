using UnityEngine;

public class ImageSlicer: MonoBehaviour
{
    public Texture2D sourceImage; // The source image to slice
    public int rows = 2; // Number of rows to slice into
    public int cols = 2; // Number of columns to slice into

    public Texture2D[] slices; // Array to hold the sliced images

    void Start()
    {
        if (sourceImage == null)
        {
            Debug.LogError("Source image not set!");
            return;
        }

        SliceImage();
    }

    void SliceImage()
    {
        int sliceWidth = sourceImage.width / cols;
        int sliceHeight = sourceImage.height / rows;

        // Initialize the array to hold the slices
        slices = new Texture2D[cols * rows];

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                // Create a new texture for each slice
                Texture2D slice = new Texture2D(sliceWidth, sliceHeight);

                // Copy pixels from the source image to the slice
                Color[] pixels = sourceImage.GetPixels(x * sliceWidth, y * sliceHeight, sliceWidth, sliceHeight);
                slice.SetPixels(pixels);
                slice.Apply();

                // Store the slice in the array
                int index = y * cols + x;
                slices[index] = slice;
            }
        }

        Debug.Log("Image slicing complete. Slices are stored in the array.");
    }

    // Optional: Function to get a specific slice from the array
    public Texture2D GetSlice(int index)
    {
        if (index >= slices.Length || index < 0)
        {
            Debug.LogError("Slice index out of bounds!");
            return null;
        }

        return slices[index];
    }
}
