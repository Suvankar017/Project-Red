using UnityEngine;

namespace ShadowMatch
{
    public static class SDFTextureGenerator
    {
        private const float INF = float.PositiveInfinity;
        //private const float SQRT2 = 1.41421356237f;

        private static readonly float SQRT2 = (float)System.Math.Sqrt(2.0);

        public static Texture2D GenerateSDF(Texture2D source)
        {
            int width = source.width;
            int height = source.height;

            Color[] pixels = source.GetPixels();

            // Step 1: Convert to binary mask
            bool[,] inside = ConvertToBinaryMask(width, height, pixels);

            // Step 2: Compute distance fields
            float[,] distToOutside = ComputeDistanceField(inside, width, height, true);
            float[,] distToInside = ComputeDistanceField(inside, width, height, false);

            float maxDistance = ComputeMaxDistance(distToOutside, distToInside, width, height);

            // Step 3: Build signed distance + normalize
            Texture2D sdfTex = new(width, height, TextureFormat.RFloat, false);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float signedDist = distToOutside[x, y] - distToInside[x, y];

                    float normalized = Mathf.Clamp01(0.5f + signedDist / maxDistance);

                    sdfTex.SetPixel(x, y, new Color(normalized, 0.0f, 0.0f, 1.0f));
                }
            }

            sdfTex.Apply();
            return sdfTex;
        }

        private static bool[,] ConvertToBinaryMask(int width, int height, Color[] pixels)
        {
            bool[,] inside = new bool[width, height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    inside[x, y] = pixels[y * width + x].a > 0.5f;
                }
            }

            return inside;
        }

        private static float[,] ComputeDistanceField(bool[,] inside, int width, int height, bool toOutside)
        {
            float[,] dist = new float[width, height];

            // Step 1: Initialize grid
            InitGrid(inside, width, height, dist);

            // Step 2: Forward pass
            ForwardPass(width, height, dist);

            // Step 3: Backward pass
            BackwardPass(width, height, dist);

            // Step 4: Optional masking depending on direction
            MaskDependingOnDirection(inside, width, height, toOutside, dist);

            return dist;
        }

        private static void MaskDependingOnDirection(bool[,] inside, int width, int height, bool toOutside, float[,] dist)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (toOutside)
                    {
                        if (!inside[x, y])
                            dist[x, y] = 0;
                    }
                    else
                    {
                        if (inside[x, y])
                            dist[x, y] = 0;
                    }
                }
            }
        }

        private static void BackwardPass(int width, int height, float[,] dist)
        {
            for (int y = height - 1; y >= 0; y--)
            {
                for (int x = width - 1; x >= 0; x--)
                {
                    float d = dist[x, y];

                    if (x < width - 1)
                        d = Mathf.Min(d, dist[x + 1, y] + 1);

                    if (y < height - 1)
                        d = Mathf.Min(d, dist[x, y + 1] + 1);

                    if (x < width - 1 && y < height - 1)
                        d = Mathf.Min(d, dist[x + 1, y + 1] + SQRT2);

                    if (x > 0 && y < height - 1)
                        d = Mathf.Min(d, dist[x - 1, y + 1] + SQRT2);

                    dist[x, y] = d;
                }
            }
        }

        private static void ForwardPass(int width, int height, float[,] dist)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float d = dist[x, y];

                    if (x > 0)
                        d = Mathf.Min(d, dist[x - 1, y] + 1);

                    if (y > 0)
                        d = Mathf.Min(d, dist[x, y - 1] + 1);

                    if (x > 0 && y > 0)
                        d = Mathf.Min(d, dist[x - 1, y - 1] + SQRT2);

                    if (x < width - 1 && y > 0)
                        d = Mathf.Min(d, dist[x + 1, y - 1] + SQRT2);

                    dist[x, y] = d;
                }
            }
        }

        private static void InitGrid(bool[,] inside, int width, int height, float[,] dist)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    bool current = inside[x, y];
                    bool isEdge = false;

                    for (int oy = -1; oy <= 1; oy++)
                    {
                        for (int ox = -1; ox <= 1; ox++)
                        {
                            int nx = x + ox;
                            int ny = y + oy;

                            if (nx >= 0 && ny >= 0 && nx < width && ny < height)
                            {
                                if (inside[nx, ny] != current)
                                {
                                    isEdge = true;
                                    break;
                                }
                            }
                        }
                        if (isEdge) break;
                    }

                    if (isEdge)
                        dist[x, y] = 0;
                    else
                        dist[x, y] = INF;
                }
            }
        }

        private static float ComputeMaxDistance(float[,] distA, float[,] distB, int width, int height)
        {
            float maxDist = 0f;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float d = Mathf.Abs(distA[x, y] - distB[x, y]);

                    if (d > maxDist)
                        maxDist = d;
                }
            }

            return maxDist;
        }
    }
}
