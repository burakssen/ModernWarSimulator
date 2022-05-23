#include "PerlinNoise.hpp"

#include <time.h>

extern "C"
{
    bool filled = false;
    void fill()
    {
        for (int x = 0; x < 512; x++)
        {
            p[x] = permutation[x % 256];
        }
        filled = true;
    }

    float perlin_values(float x, float y, float z)
    {
        if (!filled)
            fill();

        int xi, yi, zi;
        float xf, yf, zf;

        if (x < 0.0)
        {
            xi = 255 - ((int)(-x) & 255);
            xf = 1 + x - (int)x;
        }
        else
        {
            xi = (int)x & 255;
            xf = x - (int)x;
        }

        if (y < 0.0)
        {
            yi = 255 - ((int)(-y) & 255);
            yf = 1 + y - (int)y;
        }
        else
        {
            yi = (int)y & 255;
            yf = y - (int)y;
        }

        if (z < 0.0)
        {
            zi = 255 - ((int)(-z) & 255);
            zf = 1 + z - (int)z;
        }
        else
        {
            zi = (int)z & 255;
            zf = z - (int)z;
        }
        float u = fade(xf);
        float v = fade(yf);
        float w = fade(zf);

        int a = p[xi] + yi;
        int aa = p[a] + zi;
        int ab = p[a + 1] + zi;
        int b = p[xi + 1] + yi;
        int ba = p[b] + zi;
        int bb = p[b + 1] + zi;

        float x1, x2, y1, y2;
        x1 = lerp(grad(p[aa], xf, yf, zf),
                  grad(p[ba], xf - 1, yf, zf),
                  u);
        x2 = lerp(grad(p[ab], xf, yf - 1, zf),
                  grad(p[bb], xf - 1, yf - 1, zf),
                  u);
        y1 = lerp(x1, x2, v);

        x1 = lerp(grad(p[aa + 1], xf, yf, zf - 1),
                  grad(p[ba + 1], xf - 1, yf, zf - 1),
                  u);
        x2 = lerp(grad(p[ab + 1], xf, yf - 1, zf - 1),
                  grad(p[bb + 1], xf - 1, yf - 1, zf - 1),
                  u);
        y2 = lerp(x1, x2, v);

        return (lerp(y1, y2, w) + 1) / 2;
    }

    float fade(float t)
    {
        return t * t * t * (t * (t * 6 - 15) + 10);
    }

    float grad(int hash, float x, float y, float z)
    {
        int h = hash % 15;
        float u = h < 8 ? x : y;
        float v;
        if (h < 4)
            v = y;
        else if (h == 12 || h == 14)
            v = x;
        else
            v = z;

        return ((h & 1) == 0 ? u : -u) + ((h & 2) == 0 ? v : -v);
    }

    float lerp(float a, float b, float x)
    {
        return a + x * (b - a);
    }

    float max(float first, float second)
    {
        return first > second ? first : second;
    }

    float min(float first, float second)
    {
        return first < second ? first : second;
    }

    float clamp(float v, float minV, float maxV)
    {
        return min(max(v, minV), maxV);
    }

    float inverselerp(float a, float b, float v)
    {
        return clamp((v - a) / (b - a), 0.0f, 1.0f);
    }

    DLL_EXPORT void PerlinNoise(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset, Vector2 *octaveOffsets, float *noiseMap, NormalizeMode normalizeMode)
    {
        std::mt19937 rng(seed);
        std::uniform_real_distribution<float> gen(-100000, 100000);
        float maxPossibleHeight = 0;
        float amplitude = 1;
        float frequency = 1;
        for (int i = 0; i < octaves; i++)
        {
            float offsetX = gen(rng) + offset.x;
            float offsetY = gen(rng) - offset.y;
            octaveOffsets[i].x = offsetX;
            octaveOffsets[i].y = offsetY;
            maxPossibleHeight += amplitude;
            amplitude *= persistance / 2;
        }

        if (scale <= 0.0)
            scale = 0.0001f;

        float maxLocalNoiseHeight = std::numeric_limits<float>::min();
        float minLocalNoiseHeight = std::numeric_limits<float>::max();

        // for zooming in to center of the noisemap
        float halfWidth = mapWidth / 2.0f;
        float halfHeight = mapHeight / 2.0f;

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {

                amplitude = 1;
                frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = (x - halfWidth + octaveOffsets[i].x) / scale * frequency;
                    float sampleY = (y - halfHeight + octaveOffsets[i].y) / scale * frequency;
                    float perlinValue = perlin_values(sampleX, sampleY, 0.0f);
                    noiseHeight += perlinValue * amplitude;
                    amplitude *= persistance / 2;
                    frequency *= lacunarity / 2;
                }

                if (noiseHeight > maxLocalNoiseHeight)
                {
                    maxLocalNoiseHeight = noiseHeight;
                }
                else if (noiseHeight < minLocalNoiseHeight)
                {
                    minLocalNoiseHeight = noiseHeight;
                }

                noiseMap[x * mapHeight + y] = noiseHeight;
            }
        }

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                if (normalizeMode == NormalizeMode::Local)
                {
                    noiseMap[x * mapHeight + y] = inverselerp(minLocalNoiseHeight, maxLocalNoiseHeight, noiseMap[x * mapHeight + y]);
                }
                else
                {
                    float normalizedHeight = (noiseMap[x * mapHeight + y]) / (maxPossibleHeight * 0.535f);
                    noiseMap[x * mapHeight + y] = clamp(normalizedHeight, 0.0f, std::numeric_limits<float>::max()) - 0.35f;
                }
            }
        }
    }
}
