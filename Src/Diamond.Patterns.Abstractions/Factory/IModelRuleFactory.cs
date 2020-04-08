using System.Collections.Generic;
using System.Threading.Tasks;

namespace Diamond.Patterns.Abstractions
{
    public interface IModelRuleFactory
    {
        /// <summary>
        /// Get all model rule instances registered based on TInterface
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <returns>list of IModelRule instances</returns>
        Task<IEnumerable<IModelRule<TInterface>>> GetAllAsync<TInterface>();

        /// <summary>
        /// Get all model rule instances registered based on TInterface and group name
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <returns>list of IModelRule instances</returns>
        Task<IEnumerable<IModelRule<TInterface>>> GetAllAsync<TInterface>(string group);
    }
}
