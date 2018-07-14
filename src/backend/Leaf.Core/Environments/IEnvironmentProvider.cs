namespace Leaf.Environments
{
    public interface IEnvironmentProvider
    {
        T GetGlobalEnvironments<T>() where T : new();

        T GetLoginEnvironments<T>() where T : new();

        T GetMainEnvironments<T>() where T : new();
    }
}