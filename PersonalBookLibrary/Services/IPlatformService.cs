using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalBookLibrary.Services;

public interface IPlatformService
{
    Task<bool> DisplayPromptAsync(string title, string text, string yesText, string noText);

    Task DisplayAlertAsync(string title, string text, string okText);

    Task<string> DisplayActionSheet(string title, string cancelOption, string destructiveOption, string[] values);
}
