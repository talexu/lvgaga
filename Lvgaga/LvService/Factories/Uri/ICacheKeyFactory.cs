namespace LvService.Factories.Uri
{
    public interface ICacheKeyFactory
    {
        string CreateKey(string region, params string[] paths);
    }
}