namespace ExceptionWrapper
{
    public static class Guard
    {
        public static BaseResult Require(bool condition) => condition
            ? new SuccessBaseResult()
            : (BaseResult) new FailureBaseResult(new BaseErrorMessage("Condition failed"));
    }
}
