using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NabbR.ViewModels
{
    public class DialogViewModel : ViewModelBase
    {
        private Boolean? dialogResult;

        /// <summary>
        /// Gets or sets the dialog result.
        /// </summary>
        /// <value>
        /// The dialog result.
        /// </value>
        public Boolean? DialogResult
        {
            get { return this.dialogResult; }
            set
            {
                this.Set(ref dialogResult, value);
            }
        }
    }
}
