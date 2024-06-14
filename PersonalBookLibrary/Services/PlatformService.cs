namespace PersonalBookLibrary.Services;

public class PlatformService
    : IPlatformService
{
    public Task<string> DisplayActionSheet(string title, string cancelOption, string destructiveOption, string[] values)
    {
        return Shell.Current.DisplayActionSheet(title, cancelOption, destructiveOption, values);
    }

    public Task DisplayAlertAsync(string title, string text, string okText)
    {
        return Shell.Current.DisplayAlert(title, text, okText);
    }

    public Task<bool> DisplayPromptAsync(string title, string text, string yesText, string noText)
    {
        return Shell.Current.DisplayAlert(title, text, yesText, noText);
    }
}
