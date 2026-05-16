using SpeakerRecognition.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpeakerRecognition.Services;

internal class CosineSimilarity : IEmbeddingSimilarity
{
    public double Compare(float[] a, float[] b)
    {
        double dot = 0, magA = 0, magB = 0;

        for (int i = 0; i < a.Length; i++)
        {
            dot += a[i] * b[i];
            magA += a[i] * a[i];
            magB += b[i] * b[i];
        }

        return dot / (Math.Sqrt(magA) * Math.Sqrt(magB));
    }

}
