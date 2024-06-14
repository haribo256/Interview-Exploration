namespace PersonalBookLibrary.Services;

public class FileSystemService
    : IFileSystemService
{
    private static readonly string SaveDataFilePath = Path.Combine(FileSystem.Current.AppDataDirectory, "books.json");

    public Task<Stream> GetSaveDataStreamAsync()
    {
        return Task.FromResult<Stream>(new FileStream(SaveDataFilePath, FileMode.OpenOrCreate));
    }

    public async Task<Stream> GetTestDataStreamAsync()
    {
        return await FileSystem.OpenAppPackageFileAsync("InitialData.json");
    }
}
