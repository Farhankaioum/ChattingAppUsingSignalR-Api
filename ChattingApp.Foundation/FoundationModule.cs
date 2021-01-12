using Autofac;
using ChattingApp.Foundation.Repositories;
using ChattingApp.Foundation.Services;
using ChattingApp.Foundation.UnitOfWorks;

namespace ChattingApp.Foundation
{
    public class FoundationModule : Module
    {
        private readonly string _connectionString;
        private readonly string _migrationAssemblyName;

        public FoundationModule(string connectionString, string migrationAssemblyName)
        {
            _connectionString = connectionString;
            _migrationAssemblyName = migrationAssemblyName;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ChattingUnitOfWork>().As<IChattingUnitOfWork>()
                .InstancePerLifetimeScope();

            builder.RegisterType<UserRepository>().As<IUserRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<MessageRepository>().As<IMessageRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<UserService>().As<IUserService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<MessageService>().As<IMessageService>()
                .InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}
