using UnityEngine;

public class Cloud : MonoBehaviour
{

    public int width = 256;
    public int height = 256;
    public float scale = 20f;
    public float timeScale = 0.1f;
    public float alpha = 1f;
    public float threshold = 0.5f;

    private Texture2D texture;
    private Color[] pixels;
    private PerlinNoise noise;
    public GameObject cloudParticlePrefab;
    public float cloudParticleSize;
    public int cloudParticleCount;
    public float noiseScale;
    private PerlinNoise noiseGenerator;

    void Start()
    {
        // Create a new texture to hold the cloud data
        texture = new Texture2D(width, height);

        // Initialize the Perlin noise generator
        noise = new PerlinNoise();

        // Generate the cloud data and apply it to the texture
    
        texture.Apply();

        // Assign the texture to the material of the game object
        GetComponent<Renderer>().material.mainTexture = texture;
        noiseGenerator = new PerlinNoise();
        GenerateCloud();
    }
    void GenerateCloud()
    {
        for (int x = 0; x < cloudParticleCount; x++)
        {
            for (int y = 0; y < cloudParticleCount; y++)
            {
                for (int z = 0; z < cloudParticleCount; z++)
                {
                    float noiseValue = noiseGenerator.Generate(
                        x * noiseScale, y * noiseScale, z * noiseScale);

                    if (noiseValue > threshold)
                    {
                        Vector3 position = new Vector3(x, y, z);
                        GameObject particle = Instantiate(cloudParticlePrefab, position, Quaternion.identity);
                        particle.transform.localScale = Vector3.one * cloudParticleSize;
                        particle.transform.parent = transform;
                    }
                }
            }
        }

        void Update()
        {
            // Generate new cloud data every frame based on time
            GenerateClouds();
            texture.Apply();
        }

        void GenerateClouds()
        {
            // Create a new array to hold the cloud colors
            pixels = new Color[width * height];

            // Loop through each pixel and set its color based on the noise function
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float value = noise.Generate(x * scale, y * scale, Time.time * timeScale);

                    if (value > threshold)
                    {
                        pixels[x + y * width] = new Color(1f, 1f, 1f, alpha);
                    }
                    else
                    {
                        pixels[x + y * width] = new Color(0f, 0f, 0f, 0f);
                    }
                }
            }

            // Set the pixels of the texture
            texture.SetPixels(pixels);
        }
    }

    public class PerlinNoise
    {
        private int[] permutation;
        private static readonly int[] Permutation = {
        151, 160, 137, 91, 90, 15, 131, 13, 201, 95, 96, 53, 194, 233, 7, 225,
        140, 36, 103, 30, 69, 142, 8, 99, 37, 240, 21, 10, 23, 190, 6, 148,
        247, 120, 234, 75, 0, 26, 197, 62, 94, 252, 219, 203, 117, 35, 11, 32,
        57, 177, 33, 88, 237, 149, 56, 87, 174, 20, 125, 136, 171, 168, 68, 175,
        74, 165, 71, 134, 139, 48, 27, 166, 77, 146, 158, 231, 83, 111, 229, 122,
        60, 211, 133, 230, 220, 105, 92, 41, 55, 46, 245, 40, 244, 102, 143, 54,
        65, 25, 63, 161, 1, 216, 80, 73, 209, 76, 132, 187, 208, 89, 18, 169, 200, 196, 135, 130, 116, 188, 159, 86, 164, 100, 109, 198,
    173, 186, 3, 64, 52, 217, 226, 250, 124, 123, 5, 202, 38, 147, 118,
    126, 255, 82, 85, 212, 207, 206, 59, 227, 47, 16, 58, 17, 182, 189,
    28, 42, 223, 183, 170, 213, 119, 248, 152, 2, 44, 154, 163, 70, 221,
    153, 101, 155, 167, 43, 172, 9, 129, 22, 39, 253, 19, 98, 108, 110,
    79, 113, 224, 232, 178, 185, 112, 104, 218, 246, 97, 228, 251, 34, 242,
    193, 238, 210, 144, 12, 191, 179, 162, 241, 81, 51, 145, 235, 249, 14,
    239, 107, 49, 192, 214, 31, 181, 199, 106, 157, 184, 84, 204, 176, 115,
    121, 50, 45, 127, 4, 150, 254, 138, 236, 205, 93, 222, 114, 67, 29,
    24, 72, 243, 141, 128, 195, 78, 66, 215, 61, 156, 180
};

        private static readonly int[] P = new int[512];

        static PerlinNoise()
        {
            for (int i = 0; i < 256; i++)
            {
                P[i] = Permutation[i];
                P[256 + i] = Permutation[i];
            }
        }

        public float Generate(float x, float y, float z)
        {
            int xi = (int)x & 255;
            int yi = (int)y & 255;
            int zi = (int)z & 255;

            x -= (int)x;
            y -= (int)y;
            z -= (int)z;

            float u = Fade(x);
            float v = Fade(y);
            float w = Fade(z);

            int a = P[xi] + yi;
            int aa = P[a] + zi;
            int ab = P[a + 1] + zi;
            int b = P[xi + 1] + yi;
            int ba = P[b] + zi;
            int bb = P[b + 1] + zi;

            float gradient1 = Grad(P[aa], x, y, z);
            float gradient2 = Grad(P[ba], x - 1, y, z);
            float gradient3 = Grad(P[ab], x, y - 1, z);
            float gradient4 = Grad(P[bb], x - 1, y - 1, z);
            float gradient5 = Grad(P[aa + 1], x, y, z - 1);
            float gradient6 = Grad(P[ba + 1], x - 1, y, z - 1);
            float gradient7 = Grad(P[ab + 1], x, y - 1, z - 1);
            float gradient8 = Grad(P[bb + 1], x - 1, y - 1, z - 1);
            float x1 = Lerp(gradient1, gradient2, u);
            float x2 = Lerp(gradient3, gradient4, u);
            float y1 = Lerp(x1, x2, v);

            x1 = Lerp(gradient5, gradient6, u);
            x2 = Lerp(gradient7, gradient8, u);
            float y2 = Lerp(x1, x2, v);

            return Lerp(y1, y2, w);
        }

        private static float Fade(float t)
        {
            return t * t * t * (t * (t * 6 - 15) + 10);
        }

        private static float Lerp(float a, float b, float x)
        {
            return a + x * (b - a);
        }

        private static float Grad(int hash, float x, float y, float z)
        {
            int h = hash & 15;
            float u = h < 8 ? x : y;
            float v = h < 4 ? y : h == 12 || h == 14 ? x : z;
            return ((h & 1) == 0 ? u : -u) + ((h & 2) == 0 ? v : -v);
        }

    }
}


