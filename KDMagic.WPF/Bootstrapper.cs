using Caliburn.Micro;
using KDMagic.WPF.ViewModels;
using System.Windows;

namespace KDMagic.WPF
{
    /// <summary>
    /// Bootstrapper class that initialized the application
    /// </summary>
    public class Bootstrapper : BootstrapperBase
    {

        #region Constructors

        /// <summary>
        /// Initialized the bootstrapper
        /// </summary>
        public Bootstrapper()
        {
            Initialize();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sets the default window as an <see cref="ClientViewModel"/> 
        /// </summary>
        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ClientViewModel>();
        }

        #endregion

    }
}
