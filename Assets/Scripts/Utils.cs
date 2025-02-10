namespace DefaultNamespace
{
    public static class Utils
    {
        public static float Map(float value, float fromSource, float toSource, float fromTarget, float toTarget)
        {

            float normalized = (value - fromSource) / (toSource - fromSource);

            return fromTarget + normalized * (toTarget - fromTarget);
        }
    }
}