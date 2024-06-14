namespace PersonalBookLibrary.Services;

public interface IPreferencesService
{
    bool GetIsFirstLaunch();

    void SetIsFirstLaunch(bool isFirstLaunch);
}
