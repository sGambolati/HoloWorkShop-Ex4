using System;

namespace Assets.Scripts
{
    [Serializable]
    public class FaceAPIResult
    {
        public FaceData[] values;
    }

    [Serializable]
    public class FaceData
    {
        public string faceId;
        public FaceAttributes attributes;
    }

    [Serializable]
    public class FaceAttributes
    {
        public double smile;
        public string gender;
        public double age;
        public string glasses;
        public Emotions emotion;
    }

    [Serializable]
    public class Emotions
    {
        public double anger;
        public double contempt;
        public double disgust;
        public double fear;
        public double happiness;
        public double neutral;
        public double sadness;
        public double surprise;
    }
}
