using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jankilla.Core.Contracts
{
    public abstract class BaseContract : IIdentifiable
    {
        #region Public Properties
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Description { get; set; }
        [JsonIgnore]
        public bool IsOpened { get; protected set; }

        #endregion

        #region Abstracts

        public abstract string Discriminator { get; }
        public abstract bool Open();
        public abstract void Close();

        #endregion

        #region Constructors

        #endregion

        #region Protected Helpers


        protected bool ValidateContract(BaseContract contract)
        {
            if (contract == null)
            {
                return false;
            }

            if (string.IsNullOrEmpty(contract.Name))
            {
                return false;
            }

            return true;
        }

        #endregion

        public override string ToString()
        {
            return Name;
        }
    }
}
