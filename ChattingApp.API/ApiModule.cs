using Autofac;

namespace ChattingApp.API
{
    public class ApiModule : Module
    {
        private readonly string _connectionString;
        private readonly string _migrationAssemblyName;

        public ApiModule(string connectionString, string migrationAssemblyName)
        {
            _connectionString = connectionString;
            _migrationAssemblyName = migrationAssemblyName;
        }

        protected override void Load(ContainerBuilder builder)
        {

            base.Load(builder);
        }
    }
}
