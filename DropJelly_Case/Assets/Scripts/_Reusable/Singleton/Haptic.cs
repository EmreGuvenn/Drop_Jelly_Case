namespace _Reusable.Singleton
{
    public class Haptic : NonPersistentSingleton<Haptic>
    {
        public void Warning()
        {
            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.Warning);
        }
        public void Failure()
        {
            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.Failure);
        }
        public void Success()
        {
            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.Success);
        }
        public void Light()
        {
            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactLight);
        }
        public void Medium()
        {
            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactMedium);
        }
        public void Heavy()
        {
            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.ImpactHeavy);
        }
        public void Selection(){
            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.SelectionChange);
        }
        public void None(){
            iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.None);
        }
        public void IsEnabled(bool isEnable){
            iOSHapticFeedback.Instance.IsEnabled = isEnable;
        }
    }
}
