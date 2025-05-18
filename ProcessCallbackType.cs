namespace Exerussus.EcsUI
{
    public enum ProcessCallbackType
    {
        /// <summary> Заменяется при повторном вызове процесса. </summary>
        Replaceable,
        /// <summary> Срабатывает по окончанию процесса, или при повторном вызове и прерыванию предыдущего. </summary>
        Guaranteed,
        /// <summary> Срабатывает по окончанию процесса. </summary>
        PostProcess,
    }
}