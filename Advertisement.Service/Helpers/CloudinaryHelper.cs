namespace Advertisement.Service.Helpers
{
    public class CloudinaryHelper
    {
        public static string GetCloudinaryPublicId(string url)
        {
            var uri = new Uri(url);
            var pathSegments = uri.AbsolutePath.Split('/');
            return pathSegments[^1].Split('.')[0];
        }
    }
}
