using Unity.VectorGraphics;

namespace u.movin
{
    public class MovinShapeConfig
    {
        public float strokeWidth;
        public int sort;
        public VectorUtils.TessellationOptions options;

        public MovinShapeConfig()
        {
            options = new VectorUtils.TessellationOptions() {
                StepDistance = 1000.0f,
                MaxCordDeviation = 0.05f,
                MaxTanAngleDeviation = 0.05f,
                // SamplingStepSize = 0.01f
                SamplingStepSize = 0.1f
            };
            sort = 0;
            strokeWidth = 0.5f;
        }
    }
}