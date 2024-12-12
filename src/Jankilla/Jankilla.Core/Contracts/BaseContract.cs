using Jankilla.Core.Utils;
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

        protected ValidationResult ValidateContract(BaseContract contract)
        {
            if (contract == null)
            {
                return new ValidationResult(false, "Contract is null");
            }

            if (string.IsNullOrEmpty(contract.Name))
            {
                return new ValidationResult(false, "Contract name is null or empty");
            }

            return new ValidationResult(true, "Contract is valid");
        }

        #endregion

        public override string ToString()
        {
            return Name;
        }
    }
}
