namespace DefaultNamespace
{
    public static class Utils
    {
        public static float Map(float value, float fromSource, float toSource, float fromTarget, float toTarget)
        {
            // Нормализуем значение из исходного диапазона
            float normalized = (value - fromSource) / (toSource - fromSource);
            // Преобразуем его в целевой диапазон
            return fromTarget + normalized * (toTarget - fromTarget);
        }
    }
}