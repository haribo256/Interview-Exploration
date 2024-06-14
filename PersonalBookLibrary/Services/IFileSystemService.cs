namespace PersonalBookLibrary.Services;

public interface IFileSystemService
{
    public Task<Stream> GetTestDataStreamAsync();

    public Task<Stream> GetSaveDataStreamAsync();
}
