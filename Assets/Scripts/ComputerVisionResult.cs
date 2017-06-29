using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    public class ComputerVisionResult
    {
        public Categories[] categories;
        public Description description;
    }

    [Serializable]
    public class Categories
    {
        public string name;
        public double score;
    }

    [Serializable]
    public class Description
    {
        public string[] tags;
        public DescriptionCaption[] captions;
    }

    [Serializable]
    public class DescriptionCaption
    {
        public string text;
        public double confidence;
    }
}
