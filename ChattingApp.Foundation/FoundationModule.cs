using Autofac;
using ChattingApp.Foundation.Contexts;
using ChattingApp.Foundation.Helpers;
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
            builder.RegisterType<ChattingContext>()
                .WithParameter("connectionString", _connectionString)
                .WithParameter("migrationAssemblyName", _migrationAssemblyName)
                .InstancePerLifetimeScope();

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

            builder.RegisterType<Validator>().As<IValidator>()
                .SingleInstance();

            base.Load(builder);
        }
    }
}
