namespace LvFakeData
{
    class Program
    {
        static void Main(string[] args)
        {
            var helper = new FakeHelper();
            helper.UploadTestTumblrs().Wait();
        }
    }
}
