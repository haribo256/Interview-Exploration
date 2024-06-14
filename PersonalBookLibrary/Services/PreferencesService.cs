namespace PersonalBookLibrary.Services;

public class PreferencesService
    : IPreferencesService
{
    private static readonly string PreferencesFirstLaunchKey = nameof(PreferencesFirstLaunchKey);

    public bool GetIsFirstLaunch()
    {
        return Preferences.Default.Get<bool>(PreferencesFirstLaunchKey, true);
    }

    public void SetIsFirstLaunch(bool isFirstLaunch)
    {
        Preferences.Default.Set<bool>(PreferencesFirstLaunchKey, isFirstLaunch);
    }
}
