using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    public class AnalysisResult
    {
        public Tag[] tags;
        public Face[] faces;

    }

    [Serializable]
    public class Tag
    {
        public double confidence;
        public string hint;
        public string name;
    }

    [Serializable]
    public class Face
    {
        public int age;
        public FaceRectangle facerectangle;
        public string gender;
    }

    [Serializable]
    public class FaceRectangle
    {
        public int height;
        public int left;
        public int top;
        public int width;
    }
}
