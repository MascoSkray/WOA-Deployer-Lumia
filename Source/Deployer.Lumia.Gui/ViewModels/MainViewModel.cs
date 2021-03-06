using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Deployer.Gui.Common;
using ReactiveUI;

namespace Deployer.Lumia.Gui.ViewModels
{
    public class MainViewModel : ReactiveObject
    {
        private readonly IFileSystemOperations fileSystemOperations;
        private ObservableAsPropertyHelper<bool> isBusyHelper;
        private const string DonationLink = "https://github.com/WoA-project/WOA-Deployer/blob/master/Docs/Donations.md";
        private const string HelpLink = "https://github.com/WOA-Project/WOA-Deployer-Lumia#need-help";

        public MainViewModel(IFileSystemOperations fileSystemOperations, IEnumerable<IBusy> busies)
        {
            this.fileSystemOperations = fileSystemOperations;
            var isBusyObs = busies.Select(x => x.IsBusyObservable).Merge();

            DonateCommand = ReactiveCommand.Create(() => { Process.Start(DonationLink); });
            HelpCommand = ReactiveCommand.Create(() => { Process.Start(HelpLink); });
            OpenLogFolder = ReactiveCommand.Create(OpenLogs);
            isBusyHelper = isBusyObs.ToProperty(this, model => model.IsBusy);
        }

        private void OpenLogs()
        {
            fileSystemOperations.EnsureDirectoryExists("Logs");
            Process.Start("Logs");
        }

        public bool IsBusy => isBusyHelper.Value;

        public ReactiveCommand<Unit, Unit> DonateCommand { get; }

        public string Title => string.Format(Resources.AppTitle, AppVersionMixin.VersionString);

        public ReactiveCommand<Unit, Unit> OpenLogFolder { get; }

        public ReactiveCommand<Unit, Unit> HelpCommand { get; set; }
    }
}