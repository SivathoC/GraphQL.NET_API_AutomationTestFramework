namespace GraphQL.NET_API_AutomationTestFramework.Base
{
    public interface IRestFactory
    {
        IRestBuilder Create();
    }

    public class RestFactory : IRestFactory
    {
        private readonly IRestBuilder _restBUilder;

        public RestFactory(IRestBuilder restBuilder)
        {
            this._restBUilder = restBuilder;
        }

        public IRestBuilder Create()
        {
            return _restBUilder;
        }
    }
}
