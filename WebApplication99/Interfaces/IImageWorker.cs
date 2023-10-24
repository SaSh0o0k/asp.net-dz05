namespace WebApplication99.Interfaces
{
    public interface IImageWorker
    {
        string ImageSave(IFormFile image);
        string ImageSave(string url);
        void RemoveImage(string name);

    }
}
